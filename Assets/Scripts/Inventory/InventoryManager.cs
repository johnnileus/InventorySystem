using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour{
    [SerializeField] private ItemSO test;
    // Start is called before the first frame update
    void Start(){
        InventoryItem newItem = new InventoryItem(test);
        foreach (var att in newItem.attributes) {
            Debug.Log(att.name + att.GetValue());
        }
        print("a");
        foreach (var att in newItem.baseItem.attributes) {
            Debug.Log(att.name + att.GetValue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
