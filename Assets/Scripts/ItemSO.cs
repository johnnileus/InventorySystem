using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSO : ScriptableObject{
    public new string name;
    public string category;
    public string description;
    public bool stackable;

    public Sprite icon;
    public GameObject worldPrefab;
    
    [SerializeReference]
    public List<ItemAttributeBase> attributes = new List<ItemAttributeBase>(); //list of custom attributes

    public ItemAttributeBase GetAttribute(string att){
        return attributes.FirstOrDefault(attr => attr.name == att);
    }

    public void CloneUIAttribute(UIAttributeBase attr){
        attributes.Add(attr.GenerateItemAttribute());
    }
}
