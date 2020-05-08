using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBrushPrototypeInspector : Editor
{
    public GameObject objectToPlace;
    private bool placingMode = false;
    private RaycastHit mouseHit;
    private AssetBrushPrototype assetBrush;
    private string objectSetName;
    private bool invalidSetName;
    private void OnEnable()
    {
        assetBrush = (AssetBrushPrototype)target;
    }

    public override void OnInspectorGUI()
    {
        assetBrush = (AssetBrushPrototype)target;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        //draw path text label
        GUILayout.Label("New Path Name: ", GUILayout.Height(20));
        objectSetName = EditorGUILayout.TextField(objectSetName, GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

//        if (!placingMode)
//        {
//            if (objectSetName.Length < 1) GUI.backgroundColor = Color.gray;
//            else GUI.backgroundColor = Color.white;

//            if (GUILayout.Button("Click to create Path and start placing"))
//            {
//                if (objectSetName == "" || assetBrush.transform.Find(objectSetName) != null) //INVALID objectSetName
//                {
//                    assetBrush = true;
//                    return;
//                }
//                Debug.Log("Entering interactive mode");
//                pathGO = new GameObject(objectSetName);
//                pathGO.transform.position = assetBrush.gameObject.transform.position;
//                pathGO.transform.parent = assetBrush.gameObject.transform;
//                StartPath();
//                placingMode = true;
//            }
//        }
//        else //IN PLACING MODE
//        {
//            assetBrush = false;
//            GUI.backgroundColor = Color.red;
//            if (GUILayout.Button("Finish Editing"))
//            {
//                path.pathData.positions = TransformToLocalSpace(path.pathData.waypoints);
//                AssetDatabase.CreateAsset(path.pathData, "Assets/Exported Paths/" + path.pathData.objectSetName + ".asset");
//                objectSetName = "";
//                wpList.Clear();
//                placingMode = false;
//            }
//        }

//        if (assetBrush) EditorGUILayout.HelpBox("You must enter a valid and unique path name!", MessageType.Warning);
//    }
}

}
