using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
namespace v1
{
    class MapNode
    {
        public const string LEFT = "LEFT";
        public const string RIGHT = "RIGHT";
        public const string UP = "UP";
        public const string DOWN = "DOWN";

        enum Direction
        {
            Left,
            Right,
            Up,
            Down

        }
        public int x;
        public int y;
        public string value;
        Dictionary<Direction, MapNode> connectedNodes = new Dictionary<Direction, MapNode>();
        public static List<MapNode> MapNodes = new List<MapNode>();
        public bool visted;
        //A maze in ASCII format is provided as input. The character # represents a wall, the letter . represents a hollow space,
        //the letter T represents your starting position, the letter C represents the control room and the character ? represents a cell that you have not scanned yet.
        //# wall
        //. space
        //T start
        //C control room
        //? unscanned
        public bool isPassable
        {
            get { return (this.value == "."); }
        }
        public MapNode(int x, int y, string value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            MapNodes.Add(this);
        }

        static public void resetNodes()
        {
            MapNodes = new List<MapNode>();
        }
        static public MapNode findClosestUnvistedNode(int currentX, int currentY)
        {
            MapNode tmp = MapNodes.Find(item => item.x == currentX + 1 && item.y == currentY);
            if (tmp.visted == false && tmp.isPassable)
            {
                return tmp;
            }
            tmp = MapNodes.Find(item => item.x == currentX - 1 && item.y == currentY);
            if (tmp.visted == false && tmp.isPassable)
            {
                return tmp;
            }
            tmp = MapNodes.Find(item => item.x == currentX && item.y == currentY + 1);
            if (tmp.visted == false && tmp.isPassable)
            {
                return tmp;
            }
            tmp = MapNodes.Find(item => item.x == currentX - 1 && item.y == currentY - 1);
            if (tmp.visted == false && tmp.isPassable)
            {
                return tmp;
            }
            throw new System.InvalidOperationException();
        }


        static public bool GoalInMap()
        {
            var node = MapNodes.Find(item => item.value == "C");

            if (node == null)
                return false;
            return true;

        }
        static public string getDirectionOfNode(MapNode node, int playerX, int playerY)
        {


            if (node.x < playerX)
                return LEFT;
            if (node.x > playerX)
                return RIGHT;
            if (node.y < playerY)
                return DOWN;
            if (node.y > playerY)
                return UP;
            return "C";
        }
    }
    class Player
    {

        Point position;
        MapNode T;

        Point board;

        public MapNode setStartPoint()
        {
            if (T != null)
                return T;

            return T = MapNode.MapNodes.Find(item => item.value == "T");
        }

        public Player(int x, int y)
        {
            board = new Point(x, y);
        }
        public void movePlayer(string dir)
        {

            switch (dir)
            {
                case MapNode.LEFT:
                    position.X--;
                    break;
                case MapNode.RIGHT:
                    position.X++;
                    break;
                case MapNode.UP:
                    position.Y--;
                    break;
                case MapNode.DOWN:
                    position.Y++;
                    break;
            }

        }
    }
    class Solution
    {
        static bool alarm = false;

        static int RoundEndCounter = 0;
        static bool GoalInSight = false;
        static void MainV1(string[] args)
        {
            //        mapNodes.Add(new mapNode(0, 0));

            //mapNodes = mapNodes.find(item => item.x).ToList();
            //      mapNode tmp =  mapNodes.Find(item => item.x == 0 && item.y == 0);


            string[] inputs;
            inputs = VirtualConsole.ReadLine().Split(' ');
            int R = int.Parse(inputs[0]); // number of rows.
            int C = int.Parse(inputs[1]); // number of columns.
            int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
            RoundEndCounter = A;
            Player player = new Player(R, C);
            // game loop
            while (true)
            {

                inputs = VirtualConsole.ReadLine().Split(' ');
                int pY = int.Parse(inputs[0]); // row where Kirk is located.
                int pX = int.Parse(inputs[1]); // column where Kirk is located.

                for (int y = 0; y < R; y++)
                {
                    string ROW = VirtualConsole.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                    for (int x = 0; x < C; x++)
                    {
                        new MapNode(x, y, ROW[x].ToString());
                    }

                }
                player.setStartPoint();
                string dir = "";
                if (alarm == false)
                { //we have not found the control room yet
                    GoalInSight = MapNode.GoalInMap();
                    var currentNode = MapNode.MapNodes.Find(item => item.value == "T");
                    if (GoalInSight)
                    {

                        var node = MapNode.MapNodes.Find(item => item.value == "C");
                        dir = MapNode.getDirectionOfNode(node, pX, pY);

                        if (dir == "C")
                        {
                            alarm = true;

                            dir = MapNode.getDirectionOfNode(currentNode, pX, pY);
                        }

                    }
                    else
                    {
                        var node = MapNode.findClosestUnvistedNode(pX, pY);
                        dir = MapNode.getDirectionOfNode(node, pX, pY);
                        //move to unvisted tile
                    }
                }
                else
                { //we found the control room and must exit
                    var node = MapNode.MapNodes.Find(item => item.value == "T");
                    dir = MapNode.getDirectionOfNode(node, pX, pY);

                }
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                player.movePlayer(dir);
                VirtualConsole.WriteLine(dir); // Kirk's next move (UP DOWN LEFT or RIGHT).
            }


        }

    }






















    static class VirtualConsole
    {

        static public void debug(string msg)
        {
            Console.Error.WriteLine(msg);
        }
        static public string ReadLine()
        {
#if DEBUG
            string tmp = DummyInput.ReadLine();
#else
        string tmp = Console.ReadLine();
        debug("commandInputs.Add(\"" +tmp+"\");");
#endif

            return tmp;
        }
        static public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
        static public void Write(string s)
        {
            Console.Write(s);
        }
    }
}