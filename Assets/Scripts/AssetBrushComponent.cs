using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBrushComponent : MonoBehaviour
{
    public int IndiseObjeto;
    public List<GameObject> objetos = new List<GameObject>() { };
    [Header("Choose floor or surfaces for placing objects")]
    public LayerMask placementSurface;

}
