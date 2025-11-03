using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum AttributeType{None, Integer, Float, String, Boolean}

[Serializable]
public abstract class StatAttributeBase{
    public string name;
    public abstract object GetValue();
    public abstract void SetValue(object newValue);
}



[Serializable]
public class StatAttributeInt : StatAttributeBase{
    public int value;
    
    public override object GetValue() {return value;}

    public override void SetValue(object newValue){
        value = (int) newValue;
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public StatAttributeInt(string n, int v){
        name = n;
        value = v;
    }
}

[Serializable]
public class StatAttributeFloat : StatAttributeBase{
    public float value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (float) newValue;
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public StatAttributeFloat(string n, float v){
        name = n;
        value = v;
    }
}

[Serializable]
public class StatAttributeString : StatAttributeBase{
    public string value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (string) newValue;
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public StatAttributeString(string n, string v){
        name = n;
        value = v;
    }
}

[Serializable]
public class StatAttributeBool : StatAttributeBase{
    public bool value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (bool) newValue;
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public StatAttributeBool(string n, bool v){
        name = n;
        value = v;
    }
}

// [Serializable]
// public class ItemAttribute<T> : ItemAttributeBase{
//
//     public T value;
//     public override object GetValue(){
//         return value;
//
//     }
//     
//     public string GetValueAsString(){
//         return GetValue().ToString();
//     }
//     
//     public ItemAttribute(string n, T v){
//         name = n;
//         value = v;
//     }
// }