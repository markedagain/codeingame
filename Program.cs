using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

     
/**

Initialization input
Game Input
Line 1: 3 integers: R C A
R,C are the numbers of rows and columns of the maze.
A, is the number of rounds between the time the alarm countdown is activated and the time the alarm goes off.

Input for one game turn
Line 1: 2 integers: KR and KC. Kirk is located at row KR and column KC within the maze. The cell at row 0 and column 0 is located in the top left corner of the maze.
Next R lines: C characters  # or  . or  T or  C or  ? (i.e. one line of the ASCII maze)

   //A maze in ASCII format is provided as input. The character # represents a wall, the letter . represents a hollow space,
    //the letter T represents your starting position, the letter C represents the control room and the character ? represents a cell that you have not scanned yet.
    //# wall
    //. space
    //T start
    //C control room
    //? unscanned


Output for one game turn
A single line containing one of: UP DOWN LEFT or RIGHT

Constraints
10 ≤ R ≤ 100
20 ≤ C ≤ 200
1 ≤ A ≤ 100
0 ≤ KR < R
0 ≤ KC < C
Response time per turn ≤ 150ms
There at a single T character and a single C character within a maze

**/


class MapNode
{
    public const string LEFT = "LEFT";
    public const string RIGHT = "RIGHT";
    public const string UP = "UP";
    public const string DOWN = "DOWN";
    public int x;
    public int y;
    public string value;
    public  List<MapNode> connectedNodes = new List<MapNode>();
    public static List<MapNode> MapNodes = new List<MapNode>();
    public bool visted = false;

    public bool nul = false;

    public MapNode(int x, int y, string value){
        this.x = x;
        this.y = y;
        this.value = value;
        MapNodes.Add(this);
    }


    public MapNode FindNextNodeToPath(MapNode goal){
        if (connectedNodes.Count == 1)
            return connectedNodes.ElementAt(0);
        this.visted = true; //will force tree not to come back on itself and go down only 1 path
        foreach (var item in connectedNodes)
        {
            var nodeInPath = recureTreeLookingForSpecificNode(item, goal);
            if (nodeInPath != null)
                return item;
        }
        return null;

    }
    public string DirectionOfConnectedNode(MapNode nextNodeInPath){
        
        if (nextNodeInPath.x < this.x)
            return LEFT;
        if (nextNodeInPath.x > this.x)
            return RIGHT;
        if (nextNodeInPath.y < this.y)
            return UP;
        if (nextNodeInPath.y > this.y)
            return DOWN;

        return "R";

    }
    static public void resetNodes(){
        MapNodes = new List<MapNode>();
    }
         
    static public bool GoalInMap(){
      var node  = MapNodes.Find(item => item.value == Solution.Goal);

        if (node == null)
            return false;
        return true;

    }

    static public void connectNodes(){
        foreach (MapNode node in MapNodes)
        {
            if (node.value == "#")
                continue; //this is a wall, no need to track it


            node.Connect();
        }
    }
    public void Connect(){
        MapNode tmp =  MapNodes.Find(item => item.x == this.x + 1 && item.y == this.y);
        if (tmp != null && tmp.value != "#" ){
            connectedNodes.Add(tmp);
        }
                 tmp =  MapNodes.Find(item => item.x == this.x - 1 && item.y == this.y);
        if (tmp != null && tmp.value != "#"){
            connectedNodes.Add(tmp);
        }
                 tmp =  MapNodes.Find(item => item.x == this.x  && item.y == this.y + 1);
        if (tmp != null && tmp.value != "#" ){
            connectedNodes.Add(tmp);
        }
                 tmp =  MapNodes.Find(item => item.x == this.x  && item.y == this.y - 1);
        if (tmp != null && tmp.value != "#" ){
            connectedNodes.Add(tmp);
        }
    }
      public static MapNode findClosestHiddenNode(MapNode node){
        node.visted = true;
         if (node.value == "?")
            return node;
        foreach (var item in node.connectedNodes)
        {
            if (item.visted == false && item != node)
            {
                var tmpNode = findClosestHiddenNode(item);
                if (tmpNode != null)
                    return tmpNode;

            }
        }
        return null;
    }
    public static void resetVistedNodes(){
        foreach (var item in MapNode.MapNodes)
        {
            item.visted = false;
        }
    }

    public static MapNode checkIfGoalConnected(MapNode node){

        var tmpC = MapNode.MapNodes.Find(item => item.value == Solution.Goal);
        if (tmpC == null)
            return null;

        var tmpNode = recureTreeLookingForSpecificNode(node,tmpC);
        resetVistedNodes();
        if (tmpNode != null)
            return tmpNode;
        return null;
    }

    public static MapNode recureTreeLookingForGoal(MapNode node){
        node.visted = true;
        if (node.value == Solution.Goal)
            return node;
        MapNode childNode;
        foreach (var item in node.connectedNodes)
        {
            if (item.visted == true || item == node)
                continue;

            childNode = recureTreeLookingForGoal(item);
            if (childNode != null && childNode.value == Solution.Goal)
                return childNode;

        }
        return null;
    }
    
    public static MapNode recureTreeLookingForSpecificNode(MapNode node, MapNode goal){
        node.visted = true;
        if (node == goal)
            return node;
        MapNode lastChildNode;
        foreach (var child in node.connectedNodes)
        {
            if (child == goal)
                return child;
            if (child.visted == true || child == node || child.value == "?")
                continue;

            lastChildNode = recureTreeLookingForSpecificNode(child, goal);
            if (lastChildNode != null && lastChildNode == goal)
                return lastChildNode;

        }
        return null;
    }
}
class Solution
{
        static bool alarm = false;

    public static string Goal = "C";
    static int RoundEndCounter = 0;
    static bool GoalInSight = false;
    static void Main(string[] args)
    {

         string[] inputs;
        inputs = VirtualConsole.ReadLine().Split(' ');
        int R = int.Parse(inputs[0]); // number of rows.
        int C = int.Parse(inputs[1]); // number of columns.
        int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
        RoundEndCounter = A;


        MapNode searchNode;
        string dir = "";
        // game loop
        while (true)
        {
            
            inputs = VirtualConsole.ReadLine().Split(' ');
            int pY = int.Parse(inputs[0]); // row where Kirk is located.
            int pX = int.Parse(inputs[1]); // column where Kirk is located.

            MapNode.resetNodes();
            for (int y = 0; y < R; y++)
            {
                string ROW = VirtualConsole.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                for (int x = 0; x < C ; x++)
                {
                    new MapNode(x, y, ROW[x].ToString() );
                }

            }
            MapNode.connectNodes();

            var startingNode = MapNode.MapNodes.Find(item => item.value == "T");
            var playerNode =  MapNode.MapNodes.Find(item => item.x == pX && item.y == pY);
            
            if (playerNode.value == Solution.Goal)//we standing on the goal, alarm must of gone off, new goal to get back
                    Solution.Goal = "T";

            var goalNodeConnected = MapNode.checkIfGoalConnected(startingNode);
            
            if (goalNodeConnected == null){//goal is not connected so we keep exploring map
                var closestNode = MapNode.findClosestHiddenNode(playerNode);
                MapNode.resetVistedNodes();
                searchNode = closestNode;
            }else{
                searchNode = goalNodeConnected;
            }
            
            var nextConnectedNodeInPath = playerNode.FindNextNodeToPath(searchNode);
            MapNode.resetVistedNodes();
            dir = playerNode.DirectionOfConnectedNode(nextConnectedNodeInPath);

                
            VirtualConsole.WriteLine(dir); // Kirk's next move (UP DOWN LEFT or RIGHT).
        }


    }


  
}






















static class VirtualConsole{

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
    static public void WriteLine(string s){
        Console.WriteLine(s);
    }
     static public void Write(string s){
        Console.Write(s);
    }
}
