using System;

namespace backtrackingMaze {
    struct tPoint {
        public int x;
        public int y;

        public tPoint(int y, int x) {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(tPoint p1, tPoint p2) {
            return p1.Equals(p2);
        }

        public static bool operator !=(tPoint p1, tPoint p2) {
            return !p1.Equals(p2);
        }
    }

    class MainClass {
        public static void Main(string[] args) {

            //init vars
            int width;
            int height;
            tPoint startPoint = new tPoint();
            tPoint endPoint = new tPoint();
            int[,] maze;


            Console.WriteLine("Use Prebuilt Maze (0) or run Maze Builder (1)");
            if (int.Parse(Console.ReadLine()) == 1){
                userInput();
            }
            else {

                preDetermined();
            }

            //init rat path matrix
            int[,] path = new int[height, width];
            path[startPoint.y, startPoint.x] = 1;

            //solve using the backtracking algorithm
            solve(startPoint);

            //print out the final path
            printPath();

            //check if maze is solved or not
            if (path[endPoint.y, endPoint.x] == 1) Console.WriteLine("\nSolved!!!");
            else Console.WriteLine("\nUnsolveable");
            Console.Read();


            //actuall solve function
            bool solve(tPoint ratPos) {

                //if rat is at the goal, return true
                if (path[endPoint.y, endPoint.x] == 1) return true;

                //loop through all possible moves
                for (int i = 0; i < 4; i++) {
                    tPoint newRatPos = ratPos;
                    switch (i) {
                        case 0:
                            newRatPos = new tPoint(ratPos.y + 1, ratPos.x);
                            break;
                        case 1:
                            newRatPos = new tPoint(ratPos.y, ratPos.x + 1);
                            break;
                        case 2:
                            newRatPos = new tPoint(ratPos.y, ratPos.x - 1);
                            break;
                        case 3:
                            newRatPos = new tPoint(ratPos.y - 1, ratPos.x);
                            break;
                    }

                    //check if move is valid
                    if (validMove(newRatPos)) {
                        path[newRatPos.y, newRatPos.x] = 1;
                        printPath();

                        //delay so that you can see the algorithm figuring out the maze step by step
                        System.Threading.Thread.Sleep(200);

                        //check to see if this move leads to a solution
                        if (solve(newRatPos)) {
                            return true;
                        }

                        //if not reset and try a new move (go though the for loop again)
                        path[newRatPos.y, newRatPos.x] = 0;
                    }
                }
                //if no moves work return false
                return false;
            }

            //check if the point is valid (dosent go backwards, dosent hit a wall)
            bool validMove(tPoint pos) {

                if (pos.y < 0 || pos.x < 0 || pos.x >= width || pos.y >= height) return false;
                else if (path[pos.y, pos.x] == 1) return false;
                else if (maze[pos.y, pos.x] == 0) return false;
                else return true;
            }

            //Takes User through interactive maze building using the console
            void userInput() {


                Console.Clear();

                //Take User Input on height and width
                Console.WriteLine("Enter Height And Width of Grid");
                Console.Write("Height: ");
                height = int.Parse(Console.ReadLine());

                Console.Write("Width: ");
                width = int.Parse(Console.ReadLine());

                //Take User Input on start and end points
                Console.WriteLine("Enter Start and End Points");
                Console.WriteLine("(0,0) is the top left corner and positve Y values are down");


                Console.Write("Start X: ");
                startPoint.x = int.Parse(Console.ReadLine());

                Console.Write("Start Y: ");
                startPoint.y = int.Parse(Console.ReadLine());

                if (!(startPoint.x >= 0 && startPoint.x < width && startPoint.y >= 0 && startPoint.y < height)) {
                    Console.WriteLine("Invalid Start Point");
                    Console.WriteLine("Default: X = 0, Y = 0)");
                    startPoint.x = 0;
                    startPoint.y = 0;
                }

                Console.Write("End X: ");
                endPoint.x = int.Parse(Console.ReadLine());

                Console.Write("End Y: ");
                endPoint.y = int.Parse(Console.ReadLine());

                if (!(endPoint.x >= 0 && endPoint.x < width && endPoint.y >= 0 && endPoint.y < height)) {
                    Console.WriteLine("Invalid End Point");
                    Console.WriteLine("Default: X = " + (width - 1) + ", Y = " + (height - 1));
                    endPoint.x = width - 1;
                    endPoint.y = height - 1;
                }


                //init maze and fill with default values
                maze = new int[height, width];

                for (int i = 0; i < height; ++i) {
                    for (int j = 0; j < width; ++j) {
                        maze[i, j] = 1;
                    }
                }

                Console.WriteLine("Build Maze");
                Console.WriteLine("Enter: Toggle wall at cursor");
                Console.WriteLine("Arrow Keys: Move Cursor");
                Console.WriteLine("Space: Exit");
                Console.WriteLine("Press Enter to Continue");
                Console.ReadLine();
                //main maze building loop, exits when user presses X or Space
                ConsoleKey keyinfo;
                tPoint keypos = new tPoint(0, 0);
                do {

                    // redraw maze every 1/4 second
                    if ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) % 250 == 0) {
                        Console.Clear();
                        string outputBuffer = "";
                        for (int j = 0; j < width + 1; j++) {
                            outputBuffer += "--";
                        }
                        outputBuffer += "\n";

                        for (int i = 0; i < height; ++i) {
                            string lineBuffer = "";
                            lineBuffer += "|";
                            for (int j = 0; j < width; ++j) {
                                if (i == endPoint.y && j == endPoint.x) lineBuffer += "¶¶";
                                else if (i == startPoint.y && j == startPoint.x) lineBuffer += "╔╗";
                                else if (keypos == new tPoint(i, j) && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) % 500 < 250) lineBuffer += "██";
                                else if (maze[i, j] == 1) lineBuffer += "  ";
                                else lineBuffer += "▓▓";
                            }
                            lineBuffer += "|";
                            outputBuffer += lineBuffer + "\n";
                        }

                        for (int j = 0; j < width + 1; j++) {
                            outputBuffer += "--";
                        }

                        Console.WriteLine(outputBuffer);
                    }
                    //move cursur position if on arrow keys and build wall on enter
                    if (Console.KeyAvailable) {
                        keyinfo = Console.ReadKey().Key;

                        switch (keyinfo) {
                            case ConsoleKey.Enter:
                                if (keypos == startPoint || keypos == endPoint) break;

                                maze[keypos.y, keypos.x] = 1 - maze[keypos.y, keypos.x];
                                break;
                            case ConsoleKey.RightArrow:
                                if (keypos.x < width - 1) {
                                    keypos.x += 1;
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (keypos.x > 0) {
                                    keypos.x -= 1;
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (keypos.y < height - 1) {
                                    keypos.y += 1;
                                }
                                break;
                            case ConsoleKey.UpArrow:
                                if (keypos.y > 0) {
                                    keypos.y -= 1;
                                }
                                break;
                        }
                    }
                    else {
                        keyinfo = ConsoleKey.PageDown;
                    }
                }
                while (!(keyinfo == ConsoleKey.X || keyinfo == ConsoleKey.Spacebar));

            }

            //simply looad alues for a predetermined maze
            void preDetermined() {
                maze = new int[,] { {1, 1, 1, 1, 1, 0, 1, 1, 1},
                                    {1, 0, 0, 0, 1, 0, 0, 0, 1},
                                    {1, 0, 1, 1, 1, 1, 1, 0, 1},
                                    {1, 0, 1, 0, 0, 0, 1, 0, 1},
                                    {1, 0, 1, 0, 1, 0, 1, 0, 1},
                                    {1, 0, 1, 0, 1, 0, 1, 0, 1},
                                    {1, 0, 1, 0, 1, 1, 1, 1, 1},
                                    {1, 0, 0, 0, 1, 0, 0, 0, 0},
                                    {1, 1, 1, 0, 1, 1, 1, 1, 1} };

                width = maze.GetLength(0);
                height = maze.GetLength(1);
                startPoint.x = 0;
                startPoint.y = 0;
                endPoint.x = width - 1;
                endPoint.y = height - 1;
            }


            //print the current path on the maze
            void printPath() {

                Console.Clear();
                Console.Clear();
                //Console.SetWindowSize(1000, rowMax + 2);

                for (int j = 0; j < width + 1; j++) {
                    Console.Write("--");
                }
                Console.WriteLine();

                for (int i = 0; i < height; i++) {
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
                    for (int j = 0; j < width; j++) {
                        int temp = maze[i, j] + path[i, j];
                        if (temp == 2) Console.Write("╔╗");
                        else if (i == endPoint.y && j == endPoint.x) Console.Write("¶¶");
                        else if (temp == 1) Console.Write("  ");
                        else Console.Write("▓▓");
                    }
                    Console.Write("|");
                    Console.WriteLine("");
                }
                for (int j = 0; j < width + 1; j++) {
                    Console.Write("--");
                }
            }
        }
    }
}
