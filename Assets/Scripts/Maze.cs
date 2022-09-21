using System;
using System.Text;
using UnityEngine;

public class Maze
{
    const int PATH = 0;
    const int WALL = 1;

    public static int[,] Generate(int width, int height) {
        // 5–¢–‚ÌƒTƒCƒY‚Å‚Í¶¬‚Å‚«‚È‚¢
        if (height < 5 || width < 5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        // w’èƒTƒCƒY‚Å¶¬‚µŠOü‚ğ•Ç‚É‚·‚é
        var maze = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    maze[x, y] = WALL; // ŠOü‚Í‚·‚×‚Ä•Ç
                else
                    maze[x, y] = PATH;  // ŠOüˆÈŠO‚Í’Ê˜H

        // –_‚ğ—§‚ÄA“|‚·
        var rnd = new System.Random();
        for (int x = 2; x < width - 1; x += 2) {
            for (int y = 2; y < height - 1; y += 2) {
                maze[x, y] = WALL; // –_‚ğ—§‚Ä‚é

                // “|‚¹‚é‚Ü‚ÅŒJ‚è•Ô‚·
                while (true) {
                    // 1s–Ú‚Ì‚İã‚É“|‚¹‚é
                    int direction;
                    if (y == 2)
                        direction = rnd.Next(4);
                    else
                        direction = rnd.Next(3);

                    // –_‚ğ“|‚·•ûŒü‚ğŒˆ‚ß‚é
                    int WALLX = x;
                    int WALLY = y;
                    switch (direction) {
                        case 0: // ‰E
                            WALLX++;
                            break;
                        case 1: // ‰º
                            WALLY++;
                            break;
                        case 2: // ¶
                            WALLX--;
                            break;
                        case 3: // ã
                            WALLY--;
                            break;
                    }
                    // •Ç‚¶‚á‚È‚¢ê‡‚Ì‚İ“|‚µ‚ÄI—¹
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
