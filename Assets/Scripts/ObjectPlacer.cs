using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ObjectPlacer //CONCRETE COMMAND
{
    static List<Transform> spawnedObjects;

    public static void PlaceObject(Vector3 placementPosition)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube); //PLACEHOLDER TEST

        Undo.RecordObject(go, "Placed Object via AssetBrush"); //UNDO FUNCTIONALITY. ESTO NO ANDA AUN
        PrefabUtility.RecordPrefabInstancePropertyModifications(go);

        go.transform.position = placementPosition;

        //LIST MANAGEMENT STUFF FOR UNDO/REDO
        if (spawnedObjects == null) spawnedObjects = new List<Transform>(); // initialize list
        spawnedObjects.Add(go.transform); //add to list
    }

    public static void RemoveObject(Vector3 position)
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i].position == position)
            {
                GameObject.Destroy(spawnedObjects[i].gameObject);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}