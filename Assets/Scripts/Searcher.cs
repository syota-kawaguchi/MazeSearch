using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Searcher
{
    const int PATH = 0;
    const int WALL = 1;
    const int GOAL = 2;
    const int ALREADYPASS = 1;

    private int[,] maze;

    //�ʉ߂����o�H���L�^����z��B
    //0:�ʉ߂��Ă��Ȃ��ʘH, 1:�ʉ߂����ʘH�܂��͕�
    int[,] passedCells;

    public Searcher(int[,] maze) {
        this.maze = maze;
        passedCells = new int[maze.GetLength(0), maze.GetLength(1)];
    }

    //�[���D��T��
    public Tuple<int, int>[] DepthFirst(Tuple<int, int> startPos, Tuple<int, int> goal) {

        if (maze[startPos.Item1, startPos.Item2] != PATH) {
            throw new Exception("�J�n�n�_���ʘH�ł͂���܂���");
        }
        if (maze[goal.Item1, goal.Item2] != PATH) {
            throw new Exception("�S�[�����ʘH�ł͂���܂���");
        }

        var stack = new Stack<Tuple<int, int>>();
        var currentPos = startPos;
        maze[goal.Item1, goal.Item2] = GOAL;
        Array.Copy(maze, passedCells, maze.Length);

        while (true) {
            passedCells[currentPos.Item1, currentPos.Item2] = 1;
            var nextPos = GetCandidate(currentPos);
            Maze.DebugPrint(passedCells);

            if (nextPos == null) {

                if (stack.Count == 0) break;

                currentPos = stack.Pop();
                continue;
            }

            stack.Push(currentPos);

            currentPos = nextPos;

            if (maze[currentPos.Item1, currentPos.Item2] == GOAL) {
                stack.Push(currentPos);
                break;
            }
        }

        var route = stack.ToArray();
        Array.Reverse(route);

        return route;
    }

    //���D��T��
    public Tuple<int, int>[] BreadthFirst(Tuple<int, int> startPos, Tuple<int, int> goal) {

        if (maze[startPos.Item1, startPos.Item2] != PATH) {
            throw new Exception("�J�n�n�_���ʘH�ł͂���܂���");
        }
        if (maze[goal.Item1, goal.Item2] != PATH) {
            throw new Exception("�S�[�����ʘH�ł͂���܂���");
        }

        var isGoal = false;
        var queue = new Queue<Tuple<int, int>>();
        //�i�߂�}�X���ǂ����痈�����L�^���鎫���^
        var dict = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
        var currentPos = startPos;
        maze[goal.Item1, goal.Item2] = GOAL;
        Array.Copy(maze, passedCells, maze.Length);

        while (true) {
            passedCells[currentPos.Item1, currentPos.Item2] = 1;
            var candidates = GetCandidates(currentPos);
            foreach (var candidate in candidates) {
                queue.Enqueue(candidate);
                dict[candidate] = currentPos;
            }

            if (queue.Count == 0) break;

            currentPos = queue.Dequeue();

            if (maze[currentPos.Item1, currentPos.Item2] == GOAL) {
                isGoal = true;
                break;
            }
        }

        var route = new List<Tuple<int, int>>();
        while (isGoal) {
            route.Insert(0, currentPos);
            if (currentPos == startPos) break;
            currentPos = dict[currentPos];
        }

        return route.ToArray();
    }

    //���ݒn����ړ��\�ȃ}�X���擾����
    private Tuple<int, int> GetCandidate(Tuple<int, int> cell) {
        if (IsNotWALL(cell.Item1 + 1, cell.Item2) && !IsAlreadyPassed(cell.Item1 + 1, cell.Item2)) return new Tuple<int, int>(cell.Item1 + 1, cell.Item2);
        if (IsNotWALL(cell.Item1, cell.Item2 + 1) && !IsAlreadyPassed(cell.Item1, cell.Item2 + 1)) return new Tuple<int, int>(cell.Item1, cell.Item2 + 1);
        if (IsNotWALL(cell.Item1 - 1, cell.Item2) && !IsAlreadyPassed(cell.Item1 - 1, cell.Item2)) return new Tuple<int, int>(cell.Item1 - 1, cell.Item2);
        if (IsNotWALL(cell.Item1, cell.Item2 - 1) && !IsAlreadyPassed(cell.Item1, cell.Item2 - 1)) return new Tuple<int, int>(cell.Item1, cell.Item2 - 1);
        return null;
    }

    private List<Tuple<int, int>> GetCandidates(Tuple<int, int> cell) {
        var result = new List<Tuple<int, int>>();
        if (IsNotWALL(cell.Item1 + 1, cell.Item2) && !IsAlreadyPassed(cell.Item1 + 1, cell.Item2)) result.Add(new Tuple<int, int>(cell.Item1 + 1, cell.Item2));
        if (IsNotWALL(cell.Item1, cell.Item2 + 1) && !IsAlreadyPassed(cell.Item1, cell.Item2 + 1)) result.Add(new Tuple<int, int>(cell.Item1, cell.Item2 + 1));
        if (IsNotWALL(cell.Item1 - 1, cell.Item2) && !IsAlreadyPassed(cell.Item1 - 1, cell.Item2)) result.Add(new Tuple<int, int>(cell.Item1 - 1, cell.Item2));
        if (IsNotWALL(cell.Item1, cell.Item2 - 1) && !IsAlreadyPassed(cell.Item1, cell.Item2 - 1)) result.Add(new Tuple<int, int>(cell.Item1, cell.Item2 - 1));
        return result;
    }

    private bool IsNotWALL(int x, int y) {
        return (0 <= x && x < maze.GetLength(0)) && (0 <= y && y < maze.GetLength(1)) && maze[x, y] != WALL;
    }

    private bool IsAlreadyPassed(int x, int y) {
        return passedCells[x, y] == ALREADYPASS;
    }
}
