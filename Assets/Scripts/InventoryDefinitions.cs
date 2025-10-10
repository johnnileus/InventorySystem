using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType{Integer, Float, String, Boolean}

[Serializable]
public class ItemAttribute{
     public string name;
    public AttributeType type;

    public int intValue;
    public float floatValue;
    public string stringValue;
    public bool boolValue;

    public object GetValue(){
        switch (type) {
            case AttributeType.Integer:
                return intValue;
            case AttributeType.Float:
                return floatValue;
            case AttributeType.String:
                return stringValue;
            case AttributeType.Boolean:
                return boolValue;
            default:
                return null;
        }
    }
    
    public string GetValueAsString(){
        switch (type) {
            case AttributeType.Integer:
                return intValue.ToString();
            case AttributeType.Float:
                return floatValue.ToString();
            case AttributeType.String:
                return stringValue;
            case AttributeType.Boolean:
                return boolValue.ToString();
            default:
                return null;
        }
    }
    
    
    
    public ItemAttribute(string n, AttributeType t){
        name = n;
        type = t;
    }
}