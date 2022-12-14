using System;
using System.Text;
using UnityEngine;

public class Maze
{
    const int PATH = 0;
    const int WALL = 1;

    public static int[,] Generate(int width, int height) {
        // 5未満のサイズでは生成できない
        if (height < 5 || width < 5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        // 指定サイズで生成し外周を壁にする
        var maze = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    maze[x, y] = WALL; // 外周はすべて壁
                else
                    maze[x, y] = PATH;  // 外周以外は通路

        // 棒を立て、倒す
        var rnd = new System.Random();
        for (int x = 2; x < width - 1; x += 2) {
            for (int y = 2; y < height - 1; y += 2) {
                maze[x, y] = WALL; // 棒を立てる

                // 倒せるまで繰り返す
                while (true) {
                    // 1行目のみ上に倒せる
                    int direction;
                    if (y == 2)
                        direction = rnd.Next(4);
                    else
                        direction = rnd.Next(3);

                    // 棒を倒す方向を決める
                    int WALLX = x;
                    int WALLY = y;
                    switch (direction) {
                        case 0: // 右
                            WALLX++;
                            break;
                        case 1: // 下
                            WALLY++;
                            break;
                        case 2: // 左
                            WALLX--;
                            break;
                        case 3: // 上
                            WALLY--;
                            break;
                    }
                    // 壁じゃない場合のみ倒して終了
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
