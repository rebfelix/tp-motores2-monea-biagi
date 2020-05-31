using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBrushWindow : EditorWindow
{
    public GameObject objectSelected; // Objeto a poner
    private bool placingMode = false;
    private RaycastHit mouseHit;
    private AssetBrushComponent assetBrush;
    private bool useCommandWorkflow;
    private RaycastHit HitPos;
    private float distancia;
    public bool click = false;
    public float dist;
    public Vector3 _last;
    public Vector3 _position;
    public bool repeat = false;
    public List<GameObject> prefabResientes = new List<GameObject>() { };
    public string prefabSelected;
    //Scrollview
    Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";
    //   private string objectSetName;
    //   private bool invalidSetName;

    [MenuItem("AssetBrush/AssetBrush Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AssetBrushWindow));
        

    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI; //Hay que suscribirse aca para que la Scene View me avise cuando sucede algo en ella
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    void OnGUI()
    {

        for (int i = 0; i < prefabResientes.Count; i++)
        {
            prefabSelected = prefabResientes[i].name;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("seleccionar  " + prefabSelected))
            {
                objectSelected = prefabResientes[i];
            }

            GUILayout.EndHorizontal();
        }
        distancia = EditorGUILayout.FloatField(distancia);

        //useCommandWorkflow = EditorGUILayout.Toggle("Use Command Workflow (experimental)", useCommandWorkflow);

        //SELECTOR DE PREFAB
        GameObject preSelectedObject = EditorGUILayout.ObjectField( //preselecciono un objeto y le hago unos chequeos antes de asignarlo
            "Object To Spawn", //Etiqueta
            objectSelected, //objeto
            typeof(GameObject), //tipo
            false) as GameObject; //Ofrecer objetos en escena? No
        if (preSelectedObject != null && PrefabUtility.GetPrefabType(preSelectedObject) == PrefabType.Prefab) //Si es un prefab..
        {
            objectSelected = preSelectedObject; //Lo asigno
            if (prefabResientes.Contains(objectSelected) == false)
            {
                prefabResientes.Add(objectSelected);

            }
        }
        else
        {
            objectSelected = null; //Si no, dejo el campo en null
        }


        if (!placingMode) //NOT IN PLACING MODE: MUESTRO EL BOTON
        {
            if (GUILayout.Button("Click to start placing objects"))
            {
               // Debug.Log("Entering interactive mode");
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

        //SCROLL VIEW

        //EditorGUILayout.BeginHorizontal();
        //scrollPos =
        //    EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        //GUILayout.Label(t);
        //EditorGUILayout.EndScrollView();
        //if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
        //    t += " \nAnd this is more text!";
        //EditorGUILayout.EndHorizontal();
        //if (GUILayout.Button("Clear"))
        //    t = "";

        
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Ray RayPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
       
       

        if (Physics.Raycast(RayPos.origin, RayPos.direction, out HitPos, 1000f)) //assetBrush.placementSurface ARREGLAR LAYERMASK
        {
                                                   //Add specific object functionality
            _position = (HitPos.point);

        }
        dist = Vector3.Distance(_last, _position);
           // Debug.Log(repeat);
        if (distancia < 1.5f) distancia = 1.5f;
        if (repeat == true && placingMode == true && dist > distancia)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f)) //assetBrush.placementSurface ARREGLAR LAYERMASK
            {
                //Add specific object functionality
                
                    PlaceObject(mouseHit.point);
               
            }
        }
        if (SceneView.lastActiveSceneView == null)
        {
           // Debug.Log("warning! last active sceneview is null");
            return;
        }
           // Debug.Log("OnSceneGUI");
        
            if (placingMode) //Creo un handleUtility para detectar el input del mouse
            {
            repeat = false;
            //Debug.Log("is in placing mode#");
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
                HandleUtility.AddDefaultControl(controlID);
                if (Event.current.type == EventType.MouseDrag && !Event.current.alt && placingMode) //Chequeo que no este apretando Alt, para permitirle rotar la camara
                {
                if (objectSelected != null)
                { 
                    if (Event.current.button == 0)
                    {
                        repeat = true;
                    }
                }
                else 

                Event.current.Use();
                    placingMode = true;
                    //Selection.activeGameObject = assetBrush.gameObject;
                    SceneView.lastActiveSceneView.Repaint();
             
                
            }

        }
    }

    void PlaceObject(Vector3 position)
    {
        //Debug.Log("place object called on position " + position);
        _last = position;
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
