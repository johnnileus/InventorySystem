using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSO : ScriptableObject{
    public new string name;
    public string description;
    public bool stackable;

    public Sprite icon;
    public GameObject worldPrefab;
    
    [SerializeReference]
    public List<ItemAttribute> attributes = new List<ItemAttribute>(); //list of custom attributes

    public ItemAttribute GetAttribute(string att){
        return attributes.FirstOrDefault(attr => attr.name == att);
    }
}
