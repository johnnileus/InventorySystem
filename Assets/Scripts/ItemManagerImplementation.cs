using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// used for storing temporary copies of an item's custom attributes to modify.
public abstract class UIAttributeBase{
    public string name;
    public bool toDelete = false;
    
    public abstract object GetValue();
    public abstract void SetValue(object newValue);

    public abstract ItemAttributeBase GenerateItemAttribute();

}


public class UIAttributeInt : UIAttributeBase{
    public int value;
    
    public override object GetValue() {return value;}

    public override void SetValue(object newValue){
        value = (int) newValue;
    }

    public override ItemAttributeBase GenerateItemAttribute(){
        return new ItemAttributeInt(name, value);
    }

    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeInt(string n, int v){
        name = n;
        value = v;
    }
}


public class UIAttributeFloat : UIAttributeBase{
    public float value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (float) newValue;
    }
    
    public override ItemAttributeBase GenerateItemAttribute(){ 
        return new ItemAttributeFloat(name, value);
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeFloat(string n, float v){
        name = n;
        value = v;
    }
}


public class UIAttributeString : UIAttributeBase{
    public string value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (string) newValue;
    }
    
    public override ItemAttributeBase GenerateItemAttribute(){ 
        return new ItemAttributeString(name, value);
    }
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeString(string n, string v){
        name = n;
        value = v;
    }
}


public class UIAttributeBool : UIAttributeBase{
    public bool value;
    
    public override object GetValue() {return value;}
    public override void SetValue(object newValue){
        value = (bool) newValue;
    }
    
    public override ItemAttributeBase GenerateItemAttribute(){ 
        return new ItemAttributeBool(name, value);
    }
    
    public string GetValueAsString(){return GetValue().ToString();}
    
    public UIAttributeBool(string n, bool v){
        name = n;
        value = v;
    }
}