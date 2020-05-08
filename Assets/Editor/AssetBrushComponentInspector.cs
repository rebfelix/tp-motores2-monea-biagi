using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetBrushComponent))]
public class AssetBrushComponentInspector : Editor
{
    public GameObject objectToPlace;
    private bool placingMode = false;
    private RaycastHit mouseHit;
    private AssetBrushComponent assetBrush;

 //   private string objectSetName;
 //   private bool invalidSetName;

    private void OnEnable()
    {
        assetBrush = (AssetBrushComponent)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        assetBrush = (AssetBrushComponent)target;

        ///UI Borrador para crear sets
        //EditorGUILayout.Space();
        //EditorGUILayout.BeginHorizontal();
        ////draw path text label
        //GUILayout.Label("New Set Name: ", GUILayout.Height(20));
        //objectSetName = EditorGUILayout.TextField(objectSetName, GUILayout.Height(20));
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.Space();

        if (!placingMode) //NOT IN PLACING MODE: MUESTRO EL BOTON
        {
            if (GUILayout.Button("Click to start placing objects"))
            {
                Debug.Log("Entering interactive mode");
                placingMode = true;
            }
        }
        else //IN PLACING MODE
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("End Editing"))
            {
                placingMode = false;
            }
        }
    }

    void OnSceneGUI()
    {
        if (placingMode) //Creo un handleUtility para detectar el input del mouse
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlID);
            if (Event.current.type == EventType.MouseDown && !Event.current.alt && placingMode) //Chequeo que no este apretando Alt, para permitirle rotar la camara
            {
                if (Event.current.button == 0)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f, assetBrush.placementSurface))
                    {
                        PlaceObject(mouseHit.point);
                    }
                }
                Event.current.Use();
                placingMode = true;
                Selection.activeGameObject = assetBrush.gameObject;
            }
        }
    }

    void PlaceObject(Vector3 placementPosition)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube); //PLACEHOLDER TEST
        Undo.RecordObject(go, "Placed Object via AssetBrush");
        PrefabUtility.RecordPrefabInstancePropertyModifications(go);
        go.transform.position = placementPosition;
    }
}