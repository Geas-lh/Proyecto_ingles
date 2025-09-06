using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "DragDrop/Item")]
public class ItemAsset : ScriptableObject
{
    [Header("Identificadores")]
    public string id;        // clave en inglés (ej: "dog")
    public string label;     // nombre en español (ej: "Perro")

    [Header("Visual")]
    public Sprite icon;      // imagen del objeto
}
