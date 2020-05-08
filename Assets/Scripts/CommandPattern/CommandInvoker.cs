using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    private void OnEnable()
    {
        commandBuffer = new Queue<ICommand>();
    }

    public static void AddCommand(ICommand command)
    {
        commandBuffer.Enqueue(command);
    }

    void Update()
    {
        if (commandBuffer.Count > 0) 
        {
            commandBuffer.Dequeue().Execute();
        }
    }
}
