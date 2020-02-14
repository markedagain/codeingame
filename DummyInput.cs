using System.Collections.Generic;
static class DummyInput{
    static List<string> commandInputs = new List<string>();
    static DummyInput(){
commandInputs.Add("15 30 39");

    }
    static public string ReadLine(){
        string tmp = commandInputs[0];
        commandInputs.RemoveAt(0);

        return tmp;
    }
}