using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum AttributeType{Integer, Float, String, Boolean}

[Serializable]

public class ItemAttributeBase{}

public class ItemAttribute{
    public string name;
    public Type type;

    public int intValue;
    public float floatValue;
    public string stringValue;
    public bool boolValue;

    public object GetValue(){
        if (type == typeof(int)) return intValue; 
        if (type == typeof(float)) return floatValue;
        if (type == typeof(string)) return stringValue;
        if (type == typeof(bool)) return boolValue;
        return null;

    }
    
    public string GetValueAsString(){
        return GetValue().ToString();
    }
    
    
    
    public ItemAttribute(string n, Type t){
        name = n;
        type = t;
    }
}