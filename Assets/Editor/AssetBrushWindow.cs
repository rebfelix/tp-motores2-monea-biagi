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
    public Vector3 spawnRandom;
    private bool useCommandWorkflow;
    private RaycastHit HitPos;
    private float distancia;
    private float conparar;
    public bool click = false;
    public float dist;
    public Vector3 _last;
    public Vector3 _position;
    public bool repeat = false;
    public List<GameObject> prefabsRecientes = new List<GameObject>() { };
    public List<float> prefabsSeparacion = new List<float>() { };
    public string prefabSelected;
    public float prefabSelectedDistancia;
    public float random;
    private int distanciPosicionLista;
    //Scrollview
    Vector2 scrollPos;
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
        
        EditorGUILayout.LabelField("Espacio entre objetos");
        conparar = distancia;
        distancia = EditorGUILayout.FloatField(distancia);
        if(conparar != distancia)
        {
            if(prefabsRecientes.Count > 0)
            {
                prefabsSeparacion.RemoveAt(distanciPosicionLista);
                prefabsSeparacion.Insert(distanciPosicionLista, distancia);
            }
            conparar = distancia;
        }
        EditorGUILayout.LabelField("Rendomizar Posicion");
        random = EditorGUILayout.FloatField(random);
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
            if (prefabsRecientes.Contains(objectSelected) == false)
            {
                prefabsRecientes.Add(objectSelected);
                prefabsSeparacion.Add(distancia);
                distanciPosicionLista = prefabsRecientes.Count - 1;
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
            GUI.backgroundColor = Color.grey;
        }

        //SCROLL VIEW

        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400), GUILayout.Height(100));
        GUILayout.Label("Recent Objects");
        for (int i = 0; i < prefabsRecientes.Count; i++)
        {
            prefabSelected = prefabsRecientes[i].name;
            prefabSelectedDistancia = prefabsSeparacion[i];
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("seleccionar  " + prefabSelected))
            {
                objectSelected = prefabsRecientes[i];
                distanciPosicionLista = i;
                distancia = prefabSelectedDistancia;
                placingMode = true;
            }

            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
        {
            prefabsRecientes.Clear();
            prefabsSeparacion.Clear();
        }
            


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
       
        if (repeat == true && placingMode == true && dist > distancia)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f)) //assetBrush.placementSurface ARREGLAR LAYERMASK
            {
                //Add specific object functionality

                spawnRandom = new Vector3(Random.Range(-random, random), Random.Range(0, 0), Random.Range(-random, random)) + mouseHit.point;
                    PlaceObject(spawnRandom);
               
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
