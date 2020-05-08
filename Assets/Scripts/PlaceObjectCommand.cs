using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectCommand : ICommand
{
    Vector3 position;
    GameObject _object;// TO BE IMPLEMENTED

    public PlaceObjectCommand(Vector3 position)
    {
        this.position = position;
    }

    public void Execute()
    {
        ObjectPlacer.PlaceObject(position);
    }

    public void Undo()
    {
        ObjectPlacer.RemoveObject(position);
    }

    public override string ToString()
    {
        return "PlaceCube\t" + position.x + ":" + position.y + ":" + position.z;
    }
}
