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

    public bool click = false;
    public float dist;
    public Vector3 _last;
    public Vector3 _position;
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
        

        //useCommandWorkflow = EditorGUILayout.Toggle("Use Command Workflow (experimental)", useCommandWorkflow);

        //SELECTOR DE PREFAB
        GameObject preSelectedObject = EditorGUILayout.ObjectField( //preselecciono un objeto y le hago unos chequeos antes de asignarlo
            "Object To Spawn", //Etiqueta
            objectSelected, //objeto
            typeof(GameObject), //tipo
            false) as GameObject; //Ofrecer objetos en escena? No

       

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
            Debug.Log(dist);
        
        
        if (SceneView.lastActiveSceneView == null)
        {
           // Debug.Log("warning! last active sceneview is null");
            return;
        }
           // Debug.Log("OnSceneGUI");
        
            if (placingMode) //Creo un handleUtility para detectar el input del mouse
            {
                //Debug.Log("is in placing mode#");
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                HandleUtility.AddDefaultControl(controlID);
                if (Event.current.type == EventType.MouseDown && !Event.current.alt && placingMode) //Chequeo que no este apretando Alt, para permitirle rotar la camara
                {
                for(int i = 0; i < 2; i++)
                {
                    
                }
                if (_last != null)
                {
                    if (Event.current.button == 0 && dist > 1)
                    {
                        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f)) //assetBrush.placementSurface ARREGLAR LAYERMASK
                        {
                            //Add specific object functionality
                            PlaceObject(mouseHit.point);

                        }
                    }
                }else if(Event.current.button == 0)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction, out mouseHit, 1000f)) //assetBrush.placementSurface ARREGLAR LAYERMASK
                    {
                        //Add specific object functionality
                        PlaceObject(mouseHit.point);

                    }
                }

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
