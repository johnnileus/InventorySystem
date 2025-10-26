using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSO : ScriptableObject{
    public new string name;
    public CategorySO category;
    public string description;
    public int maxStack;

    public Sprite icon;
    public GameObject worldPrefab;
    
    [SerializeReference]
    public List<StatAttributeBase> attributes = new List<StatAttributeBase>(); //list of custom attributes

    public StatAttributeBase GetAttribute(string att){
        return attributes.FirstOrDefault(attr => attr.name == att);
    }

    public void CloneUIAttribute(UIAttributeBase attr){
        attributes.Add(attr.GenerateItemAttribute());
    }
}
