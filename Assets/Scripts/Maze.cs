using System;
using System.Text;
using UnityEngine;

public class Maze
{
    const int PATH = 0;
    const int WALL = 1;

    public static int[,] Generate(int width, int height) {
        // 5�����̃T�C�Y�ł͐����ł��Ȃ�
        if (height < 5 || width < 5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        // �w��T�C�Y�Ő������O����ǂɂ���
        var maze = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    maze[x, y] = WALL; // �O���͂��ׂĕ�
                else
                    maze[x, y] = PATH;  // �O���ȊO�͒ʘH

        // �_�𗧂āA�|��
        var rnd = new System.Random();
        for (int x = 2; x < width - 1; x += 2) {
            for (int y = 2; y < height - 1; y += 2) {
                maze[x, y] = WALL; // �_�𗧂Ă�

                // �|����܂ŌJ��Ԃ�
                while (true) {
                    // 1�s�ڂ̂ݏ�ɓ|����
                    int direction;
                    if (y == 2)
                        direction = rnd.Next(4);
                    else
                        direction = rnd.Next(3);

                    // �_��|�����������߂�
                    int WALLX = x;
                    int WALLY = y;
                    switch (direction) {
                        case 0: // �E
                            WALLX++;
                            break;
                        case 1: // ��
                            WALLY++;
                            break;
                        case 2: // ��
                            WALLX--;
                            break;
                        case 3: // ��
                            WALLY--;
                            break;
                    }
                    // �ǂ���Ȃ��ꍇ�̂ݓ|���ďI��
                    if (maze[WALLX, WALLY] != WALL) {
                        maze[WALLX, WALLY] = WALL;
                        break;
                    }
                }
            }
        }
        return maze;
    }

    public static void DebugPrint(int[,] maze) {
        StringBuilder sb = new StringBuilder();
        for (int y = maze.GetLength(0) - 1; 0 <= y; y--) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                sb.Append($"{maze[x, y]}, ");
            }
            sb.Append("\n");
        }
        Debug.Log(sb);
    }
}
