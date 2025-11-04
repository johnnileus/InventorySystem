using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour{
    [SerializeField] private ItemSO test;

    private InventoryContainer container;
    // Start is called before the first frame update
    void Start(){
        container = new InventoryContainer(6);
        InventoryItem item = new InventoryItem(test);
        container.InsertItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
