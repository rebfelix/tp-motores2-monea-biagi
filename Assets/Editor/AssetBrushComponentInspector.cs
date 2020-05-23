﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(AssetBrushComponent))]
public class AssetBrushComponentInspector : Editor
{
    public GameObject objectSelected; // Objeto a poner
    private bool placingMode = false;
    private RaycastHit mouseHit;
    private AssetBrushComponent assetBrush;
    private bool useCommandWorkflow;
    public List<GameObject> A = new List<GameObject>() { };
    public int selection = -1;
    //   private string objectSetName;
    //   private bool invalidSetName;

    private void OnEnable()
    {
        assetBrush = (AssetBrushComponent)target;
    }

    public override void OnInspectorGUI()
    {
        
        A = assetBrush.objetos;
        selection = assetBrush.IndiseObjeto;
        if (selection > -1 && A.Count > selection)
        {
            objectSelected = A[selection];
        }
        DrawDefaultInspector();
        assetBrush = (AssetBrushComponent)target;
        useCommandWorkflow = EditorGUILayout.Toggle("Use Command Workflow (experimental)", useCommandWorkflow);

        //SELECTOR DE PREFAB
        GameObject preSelectedObject = EditorGUILayout.ObjectField( //preselecciono un objeto y le hago unos chequeos antes de asignarlo
            "Object To Spawn", //Etiqueta
            objectSelected, //objeto
            typeof(GameObject), //tipo
            false) as GameObject; //Ofrecer objetos en escena? No
        if (preSelectedObject != null && PrefabUtility.GetPrefabAssetType(preSelectedObject) != PrefabAssetType.NotAPrefab) //Si es un prefab..
        {
            objectSelected = preSelectedObject; //Lo asigno
            if(A.Contains(objectSelected) == false)
            {
                A.Add(objectSelected);
            }
            Debug.Log(objectSelected);
        }
        else
        {
            objectSelected = null; //Si no, dejo el campo en null
        }

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
        if (placingMode && objectSelected != null) //Creo un handleUtility para detectar el input del mouse
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
                        //Add specific object functionality
                        PlaceObject(mouseHit.point);
                    }
                }
                Event.current.Use();
                placingMode = true;
                Selection.activeGameObject = assetBrush.gameObject;
            }
        }
    }

    void PlaceObject(Vector3 position)
    {
        if (useCommandWorkflow)
        {
            ICommand newCommand = new PlaceObjectCommand(position); //METODO EXPERIMENTAL, NO SE USA POR DEFECTO
            CommandInvoker.AddCommand(newCommand);
        }
        else
        {
            ObjectPlacer.PlaceObject(position, objectSelected); //METODO COMUN PARA COLOCAR OBJETOS USADO POR DEFECTO
        }
    }

}