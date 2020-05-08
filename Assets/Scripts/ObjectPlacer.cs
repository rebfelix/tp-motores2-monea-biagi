using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ObjectPlacer //CONCRETE COMMAND
{
    public static void PlaceObject(Vector3 placementPosition)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube); //PLACEHOLDER TEST

        Undo.RecordObject(go, "Placed Object via AssetBrush"); //UNDO FUNCTIONALITY. ESTO NO ANDA AUN
        PrefabUtility.RecordPrefabInstancePropertyModifications(go);

        go.transform.position = placementPosition;
    }
}