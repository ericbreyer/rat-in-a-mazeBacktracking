using System;

namespace backtrackingMaze {
    class MainClass {
        public static void Main(string[] args) {

            int width;
            int height;

            int[,] maze;

            void userInput() {
                Console.Clear();
                Console.Write("Height: ");
                height = int.Parse(Console.ReadLine());
                Console.Write("Width: ");
                width = int.Parse(Console.ReadLine());

                maze = new int[height, width];
                for (int i = 0; i < height; ++i) {
                    for (int j = 0; j < width; ++j) {
                        maze[i, j] = 1;
                    }
                }

                        ConsoleKey keyinfo;
                (int, int) keypos = (0 ,0);
                do {
                    
                    if ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) % 250 == 0) {
                        Console.Clear();
                        string outputBuffer = "";
                        for (int i = 0; i < height; ++i) {
                            string lineBuffer = "";
                            for (int j = 0; j < width; ++j) {
                                if (i == height - 1 && j == width - 1) lineBuffer += "¶¶";
                                else if (keypos == (i, j) && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) % 500 < 250) lineBuffer +=  "██";
                                else if (maze[i, j] == 1) lineBuffer +=  "  ";
                                else lineBuffer +=  "▓▓";
                            }
                            outputBuffer += lineBuffer + "\n";
                        }
                        Console.WriteLine(outputBuffer);
                    }

                    if (Console.KeyAvailable) {
                        keyinfo = Console.ReadKey().Key;

                        switch (keyinfo) {
                            case ConsoleKey.Enter:
                                if (keypos == (0, 0) || keypos == (height - 1, width - 1)) break;

                                maze[keypos.Item1,keypos.Item2] = 1 - maze[keypos.Item1, keypos.Item2];
                                break;
                            case ConsoleKey.RightArrow:
                                if (keypos.Item2 < width - 1) {
                                    keypos.Item2 += 1;
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (keypos.Item2 > 0) {
                                    keypos.Item2 -= 1;
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (keypos.Item1 < height - 1) {
                                    keypos.Item1 += 1;
                                }
                                break;
                            case ConsoleKey.UpArrow:
                                if (keypos.Item1 > 0) {
                                    keypos.Item1 -= 1;
                                }
                                break;
                        }
                    }
                    else {
                        keyinfo = ConsoleKey.Spacebar;
                    }
                }
                while (keyinfo != ConsoleKey.X);

            }
            //userInput();
            //.Write(width + " " + height);
            //Console.Read();


            maze = new int[,] { {1, 1, 1, 1, 1, 0, 1, 1, 1},
                                {1, 0, 0, 0, 1, 0, 0, 0, 1},
                                {1, 0, 1, 1, 1, 1, 1, 0, 1},
                                {1, 0, 1, 0, 0, 0, 1, 0, 1},
                                {1, 0, 1, 0, 1, 0, 1, 0, 1},
                                {1, 0, 1, 0, 1, 0, 1, 0, 1},
                                {1, 0, 1, 0, 1, 1, 1, 1, 1},
                                {1, 0, 0, 0, 1, 0, 0, 0, 0},
                                {1, 1, 1, 0, 1, 1, 1, 1, 1} };

            int rowMax = maze.GetLength(0) - 1;
            int colMax = maze.GetLength(1) - 1;

            int[,] path = new int[rowMax + 1, colMax + 1];
            path[0, 0] = 1;



            bool validMove((int, int) pos) {

                if (pos.Item1 < 0 || pos.Item2 < 0 || pos.Item1 > rowMax || pos.Item2 > colMax) return false;
                else if (path[pos.Item1, pos.Item2] == 1) return false;
                else if (maze[pos.Item1, pos.Item2] == 0) return false;
                else return true;
            }

            bool solve((int,int) ratPos) {

                if (path[rowMax, colMax] == 1) return true;

                for(int i = 0; i < 4; i++) {
                    (int, int) newRatPos = ratPos;
                    switch (i) {
                        case 0:
                            newRatPos = (ratPos.Item1 + 1, ratPos.Item2);
                            break;
                        case 1:
                            newRatPos = (ratPos.Item1, ratPos.Item2 + 1);
                            break;
                        case 2:
                            newRatPos = (ratPos.Item1, ratPos.Item2 - 1);
                            break;
                        case 3:
                            newRatPos = (ratPos.Item1 - 1, ratPos.Item2);
                            break;
                    }

                    if (validMove(newRatPos)) {
                        path[newRatPos.Item1, newRatPos.Item2] = 1;
                        printPath();
                        System.Threading.Thread.Sleep(200);

                        if (solve(newRatPos)) {
                            return true;
                        }

                        path[newRatPos.Item1, newRatPos.Item2] = 0;
                    }
                }
                return false;
            }

            solve((0,0));

            void printPath() { 

                Console.Clear();
                Console.Clear();
                //Console.SetWindowSize(1000, rowMax + 2);

                for (int j = 0; j < colMax + 2; j++) {
                    Console.Write("--");
                }
                Console.WriteLine();

                for (int i = 0; i < rowMax + 1; i++) {
                    /*for (int j = 0; j < colMax + 1; j++) {
                        Console.Write(path[i, j]);
                        Console.Write("  ");
                    }
                    Console.Write("    ");
                    for (int j = 0; j < colMax + 1; j++) {
                        Console.Write(maze[i, j]);
                        Console.Write("  ");
                    }*/
                    Console.Write("|");
                    for (int j = 0; j < colMax + 1; j++) {
                        int temp = maze[i, j] + path[i, j];
                        if (temp == 2) Console.Write("╔╗");
                        else if (i == rowMax && j == colMax) Console.Write("¶¶");
                        else if (temp == 1) Console.Write("  ");
                        else Console.Write("▓▓");
                    }
                    Console.Write("|");
                    Console.WriteLine("");
                }
                for (int j = 0; j < colMax + 2; j++) {
                    Console.Write("--");
                }
            }
            printPath();
            if (maze[rowMax, colMax] == 1) Console.WriteLine("\nSolved!!!");
            else Console.WriteLine("Unsolveable");
            Console.Read();
            
        }
    }
}
