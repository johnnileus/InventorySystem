using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryContainer{
    
    private List<InventoryItem> slots;

    public InventoryContainer(int size){
        slots = new List<InventoryItem>();
        for (int i = 0; i < size; i++) {
            slots.Add(null);
        }
    }
    
    //return left over stack
    public int InsertItem(InventoryItem item){
        
        if (item.baseItem.maxStack <= 1) {
            for (int i = 0; i < slots.Count; i++) {
                if (slots[i] == null) {
                    slots[i] = item;
                    return 0;
                }
            }
        }
        
        int remainingCount = item.count;

        foreach (var slot in slots) {
            if (slot == null) {
                
            } else if (slot.baseItem == item.baseItem) {
                
            }
        }
        return 0;
    }

    public void RemoveSlot(int slot){
    }
    public int RemoveAmountFromSlot(int slot, int amt){
        return 0;
    }
    public void RemoveItem(ItemSO item, int count){
    }
    public void SwapSlots(int slotA, int slotB){
    }
    public void MergeSlots(int slotA, int slotB){
    }
    public InventoryItem GetItemAt(int slot){
        return null;
    }
    public int GetSize(){
        return slots.Count;
    }
    public int FindEmptySlot(){
        return 0;
    }
    public int GetItemQuantity(ItemSO item){
        return 0;
    }
    

}
