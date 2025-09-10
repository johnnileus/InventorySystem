using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ItemAttribute {
    public string name;
    
}

[Serializable]
public class IntAttribute : ItemAttribute{
    public int value;

    public IntAttribute(string n, int v){
        name = n;
        value = v;
    }
}

[Serializable]
public class FloatAttribute : ItemAttribute{
    public float value;

    public FloatAttribute(string n, float v){
        name = n;
        value = v;
    }
}

[Serializable]
public class StringAttribute : ItemAttribute{
    public string value;

    public StringAttribute(string n, string v){
        name = n;
        value = v;
    }
}
