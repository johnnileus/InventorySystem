using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// used for storing temporary copies of an item's custom attributes to modify.
[Serializable]
public abstract class UIAttributeBase{
    public string name;
    public bool toDelete = false;
    
    public abstract object GetValue();
    public abstract void SetValue(object newValue);

    public abstract StatAttributeBase GenerateItemAttribute();

}

[Serializable]

public class UIAttributeInt : UIAttributeBase{
    public int value;
    
    public override object GetValue() {return value;}

    public override void SetValue(object newValue){
        value = (int) newValue;
    }

    public override StatAttributeBase GenerateItemAttribute(){
        return new StatAttributeInt(name, value);
    }

    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeInt(string n, int v){
        name = n;
        value = v;
    }
}

[Serializable]

public class UIAttributeFloat : UIAttributeBase{
    public float value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (float) newValue;
    }
    
    public override StatAttributeBase GenerateItemAttribute(){ 
        return new StatAttributeFloat(name, value);
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeFloat(string n, float v){
        name = n;
        value = v;
    }
}

[Serializable]

public class UIAttributeString : UIAttributeBase{
    public string value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (string) newValue;
    }
    
    public override StatAttributeBase GenerateItemAttribute(){ 
        return new StatAttributeString(name, value);
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeString(string n, string v){
        name = n;
        value = v;
    }
}

[Serializable]

public class UIAttributeBool : UIAttributeBase{
    public bool value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (bool) newValue;
    }
    
    public override StatAttributeBase GenerateItemAttribute(){ 
        return new StatAttributeBool(name, value);
    }
    
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeBool(string n, bool v){
        name = n;
        value = v;
    }
}