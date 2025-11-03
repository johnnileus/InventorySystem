using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using UnityEngine;

public class InventoryItem{
    
    public ItemSO baseItem;
    public List<StatAttributeBase> attributes;


    public InventoryItem(ItemSO item){
        baseItem = item;
        attributes = new List<StatAttributeBase>();
        foreach (var att in baseItem.attributes) {
            switch (att) {
                case StatAttributeInt intAtt:
                    attributes.Add(new StatAttributeInt(intAtt));
                    break;
                case StatAttributeFloat floatAtt:
                    attributes.Add(new StatAttributeFloat(floatAtt));
                    break;
                case StatAttributeString stringAtt:
                    attributes.Add(new StatAttributeString(stringAtt));
                    break;
                case StatAttributeBool boolAtt:
                    attributes.Add(new StatAttributeBool(boolAtt));
                    break;

            }
        }
    }
}
