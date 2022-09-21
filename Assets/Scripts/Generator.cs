using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

enum Mode {
    DepthFirst,
    BreadthFirst
}

public class Generator : MonoBehaviour
{
    [SerializeField] private Mode mode = Mode.DepthFirst;

    int[,] maze;
    [SerializeField] private int width = 7;
    [SerializeField] private int height = 7;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject playerPrefab;

    const int PATH = 0;
    const int WALL = 1;

    IEnumerator Start()
    {
        var maze = Maze.Generate(width, height);
        Debug.Log("Maze is generated");
        Maze.DebugPrint(maze);
        yield return StartCoroutine(GenerateStage(maze));
        var searcher = new Searcher(maze);
        var startPos = new Tuple<int, int>(1, 1);
        var goalPos = new Tuple<int, int>(5, 5);
        var route = mode == Mode.DepthFirst ?
            searcher.DepthFirst(startPos, goalPos) :
            searcher.BreadthFirst(startPos, goalPos);
        Debug.Log($"route length : {route.Length}");
        foreach(var cell in route) {
            Debug.Log($"x:{cell.Item1}, y:{cell.Item2}");
        }
        yield return StartCoroutine(Go(route));
    }

    private IEnumerator GenerateStage(int[,] maze) {
        int wallSizeX = 1;
        int wallSizeZ= 1;
        for (int i = 0; i < maze.GetLength(0); i++) {
            for (int j = 0; j < maze.GetLength(1); j++) {
                if (maze[i, j] == WALL) {
                    Instantiate(wall, new Vector3(wallSizeX * i, 0.25f, wallSizeZ * j), Quaternion.identity);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private IEnumerator Go(Tuple<int, int>[] route) {
        var player = Instantiate(playerPrefab, new Vector3(route[0].Item1, 1, route[0].Item2), Quaternion.identity);

        for (int i = 1; i < route.Length; i++) {
            player.transform.position = new Vector3(route[i].Item1, 1, route[i].Item2);
            Debug.Log($"i:{i}, position({route[i].Item1}, 1, {route[i].Item2})");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
