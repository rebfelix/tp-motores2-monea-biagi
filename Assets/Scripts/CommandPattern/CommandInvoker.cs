using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    static List<ICommand> commandHistory;
    static int counter;

    private void OnEnable()
    {
        commandBuffer = new Queue<ICommand>();
        commandHistory = new List<ICommand>();
    }

    public static void AddCommand(ICommand command)
    {
        /// Remove redo history if a new command is added
        if (counter < commandHistory.Count)
        {
            while(commandHistory.Count > counter)
            {
                commandHistory.RemoveAt(counter);
            }
        }

        commandBuffer.Enqueue(command);
    }

    void Update()
    {
        if (commandBuffer.Count > 0) 
        {
            ICommand c = commandBuffer.Dequeue();
            c.Execute();

            commandHistory.Add(c);
            counter++;
            Debug.Log("Command history length: " + commandHistory.Count);
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Z)) //Check for Undo
            {
                if (counter > 0)
                {
                    counter--;
                    commandHistory[counter].Undo();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R)) //Check for Redo
            {
                if (counter < commandHistory.Count)
                {
                    commandHistory[counter].Execute();
                    counter++;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ExportLog();
        }
    }

    static void ExportLog()
    {
        List<string> lines = new List<string>();
        foreach (ICommand c in commandHistory)
        {
            lines.Add(c.ToString());
        }
        System.IO.File.WriteAllLines(Application.dataPath + "/commandlog.txt", lines);
    }
}
