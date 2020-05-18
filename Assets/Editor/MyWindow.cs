//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.VersionControl;
//using System;

//public class MyWindow : EditorWindow
//{
//    private GameObject objectSelected;
//    string Name;
//    List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
//    private bool placingMode = false;
//    private RaycastHit mouseHit;
//    private AssetBrushComponent assetBrush;
//    private bool useCommandWorkflow;
//    private AssetBrushComponent target;
//    public Vector3 point;
//    private GameObject _Object;
//    [MenuItem("MyEditor/Asset brush")]
//    public static void OpenWindow()
//    {
//        GetWindow<MyWindow>();
        

//    }
//    private void Update()
//    {
//        if (placingMode) //Creo un handleUtility para detectar el input del mouse
//        {
//            int controlID = GUIUtility.GetControlID(FocusType.Passive);
//            HandleUtility.AddDefaultControl(controlID);
//            if (Event.current.type == EventType.MouseDown && !Event.current.alt && placingMode) //Chequeo que no este apretando Alt, para permitirle rotar la camara
//            {
//                if (Event.current.button == 0)
//                {
//                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
//                    if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f, assetBrush.placementSurface))
//                    {
//                        //Add specific object functionality
//                        PlaceObject(mouseHit.point);
//                        Instantiate(objectSelected);

//                    }
//                }
//                Event.current.Use();
//                placingMode = true;
//                Selection.activeGameObject = assetBrush.gameObject;
//            }
//        }
//        //OnSceneGUI();
//    }
//    private void OnEnable()
//    {
//        assetBrush = (AssetBrushComponent)target;
//    }
//    private void OnGUI()
//    {
//        //sarasa

//        GameObject preSelectedObject = EditorGUILayout.ObjectField( //preselecciono un objeto y le hago unos chequeos antes de asignarlo
//                    "Object To Spawn", //Etiqueta
//                    objectSelected, //objeto
//                    typeof(GameObject), //tipo
//                    false) as GameObject; //Ofrecer objetos en escena? No
//        if (preSelectedObject != null && PrefabUtility.GetPrefabType(preSelectedObject) == PrefabType.Prefab) //Si es un prefab..
//        {
//            objectSelected = preSelectedObject; //Lo asigno
//        }
//        else
//        {
//            objectSelected = null; //Si no, dejo el campo en null
//        }

//        //Arreglado !

//        assetBrush = (AssetBrushComponent)target;
//        useCommandWorkflow = EditorGUILayout.Toggle("Use Command Workflow (experimental)", useCommandWorkflow);



//        if (!placingMode) //NOT IN PLACING MODE: MUESTRO EL BOTON
//        {
//            if (GUILayout.Button("Click to start placing objects"))
//            {
//                Debug.Log("Entering interactive mode");
//                placingMode = true;
//            }
//        }
//        else //IN PLACING MODE
//        {
//            GUI.backgroundColor = Color.red;
//            if (GUILayout.Button("End Editing"))
//            {
//                placingMode = false;
//            }
//        }
//       /* if (objectSelected)
//        {
//            EditorGUILayout.Space();
//            EditorGUILayout.Space();
//            if (GUILayout.Button("--> spawn <--"))
//            {

                
//                Instantiate(objectSelected);
                
//            }

//            if (AssetDatabase.Contains(objectSelected))
//                search();

//        }
//        else
//        {
//            EditorGUILayout.HelpBox("Selecciona un objeto", MessageType.Info);
//            search();
//        }*/
//    }



//    //private void DrawDefaultInspector() => throw new NotImplementedException();

//    void OnSceneGUI()
//    {
        
//    }
//    void search()
//    {
//        EditorGUILayout.LabelField("introdusca nombre del prefab");

//        EditorGUILayout.BeginHorizontal();

//        var TextName = Name;
//        Name = EditorGUILayout.TextField(TextName);

//        if (GUILayout.Button("Buscar"))
//        {
//            assets.Clear();

//            string[] allPaths = AssetDatabase.FindAssets(Name, new[]{"Assets/prefab"}); 
            

//            for (int i = 0; i < allPaths.Length; i++)
//            {
//                allPaths[i] = AssetDatabase.GUIDToAssetPath(allPaths[i]);      
//                assets.Add(AssetDatabase.LoadAssetAtPath(allPaths[i], typeof(UnityEngine.Object)));
//            }
//        }
//        EditorGUILayout.EndHorizontal();


//    }
//    void PlaceObject(Vector3 position)
//    {
//        if (useCommandWorkflow)
//        {
//            ICommand newCommand = new PlaceObjectCommand(position); //METODO EXPERIMENTAL, NO SE USA POR DEFECTO
//            CommandInvoker.AddCommand(newCommand);
//        }
//        else
//        {
//            ObjectPlacer.PlaceObject(position, objectSelected); //METODO COMUN PARA COLOCAR OBJETOS USADO POR DEFECTO
//        }
//    }
//}
