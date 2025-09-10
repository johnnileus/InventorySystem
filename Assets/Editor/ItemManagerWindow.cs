
using System;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEditor;
using UnityEngine;

public class ItemManagerWindow : EditorWindow{

    private int itemSpriteSize = 32;
    
    private enum MenuName{
        Main,
        Items,
        Categories,
        Modifiers,
        Crafting,
        Currencies,
        Tags
    }

    private MenuName currentMenu = MenuName.Main;
    private Vector2 itemListScrollPosition;
    
    private List<ItemSO> itemDatabase = new List<ItemSO>();
    
    [MenuItem("Window/ItemManager")]
    public static void ShowWindow(){
        GetWindow<ItemManagerWindow>("Item Manager");
    }

    private void OnEnable(){
        GenerateItemDatabase();
    }

    private void OnGUI(){
        DrawNavigationBar();
    }

    private void GenerateItemDatabase(){ 
        itemDatabase.Clear();
        itemDatabase.AddRange(Resources.LoadAll<ItemSO>("Items"));
    }

    #region MenuDraws
    
    private void DrawNavigationBar(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Epic Item Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Items")) {
            currentMenu = MenuName.Items;
        }
        if (GUILayout.Button("Categories")) {
            currentMenu = MenuName.Categories;
        }
        if (GUILayout.Button("Modifiers")) {
            currentMenu = MenuName.Modifiers;
        }
        EditorGUILayout.EndHorizontal();

        switch (currentMenu) { 
            case MenuName.Main:
                DrawMainMenu();
                break;
            case MenuName.Categories:
                DrawCategoriesMenu();
                break;
            case MenuName.Items:
                DrawItemsMenu();
                break;
            case MenuName.Modifiers:
                DrawModifiersMenu();
                break;
        }
    }
    
    private void DrawMainMenu(){

    }

    private void DrawItemsMenu(){
        GenerateItemDatabase();
        
        GUILayout.Label("items");
        EditorGUILayout.BeginHorizontal();
        itemListScrollPosition = EditorGUILayout.BeginScrollView(itemListScrollPosition, GUILayout.Width(250));
        DrawItemList();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawItemList(){
        foreach (var item in itemDatabase) {
            if (item == null) continue;
            EditorGUILayout.BeginHorizontal("box");
            if (item.icon == null) {
                GUILayout.Space(itemSpriteSize);
            }
            else {
                GUILayout.Box(item.icon.texture, GUILayout.Width(itemSpriteSize), GUILayout.Height(itemSpriteSize));
            }
            GUILayout.Label(item.name);
            EditorGUILayout.EndHorizontal();
        }
    }
    
    private void DrawCategoriesMenu(){
        GUILayout.Label("categories");
    }
    
    private void DrawModifiersMenu(){
        GUILayout.Label("modifiers");
        if (GUILayout.Button("test")) { 
            ItemSO newItem = ScriptableObject.CreateInstance<ItemSO>();

            newItem.name = "bwah";
            FloatAttribute damag = new FloatAttribute("Damage", 100f);
            newItem.attributes.Add(damag);
            Debug.Log(newItem.GetAttribute("Damage"));

            AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Items/bwah.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    private void DrawCraftingMenu(){
        GUILayout.Label("crafting");
    }
    private void DrawCurrenciesMenu(){
        GUILayout.Label("currencies");
    }
    private void DrawTagsMenu(){
        GUILayout.Label("tags");
    }
    #endregion

}

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEditor.VersionControl;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// public class ItemManagerWindow : EditorWindow{
//     //fields
//     private string itemName;
//     private int itemID;
//     private string itemDescription;
//     private Sprite itemIcon;
//     private ItemType itemType = ItemType.None;
//     private Vector2 scrollPosition;
//
//     private ItemSO selectedItem = null;
//     
//     private int selectedTab = 0;
//     private string[] tabTitles = { "Manage Items", "Create Item" };
//     private string path = "Assets/Resources/Items";
//     private List<ItemSO> allItems = new List<ItemSO>();
//
//
//     [MenuItem("Window/Item Manager")]
//     public static void ShowWindow(){ GetWindow<ItemManagerWindow>("Item Creator"); }
//
//     private void RefreshItemList(){
//         allItems.Clear();
//         allItems.AddRange(Resources.LoadAll<ItemSO>("Items"));
//     }
//     
//     private void OnEnable(){
//         RefreshItemList();
//     }
//
//     private void OnGUI(){
//         selectedTab = GUILayout.Toolbar(selectedTab, tabTitles);
//         switch (selectedTab) {
//             case 0:
//                 DrawManageItemsTab();
//                 break;
//             case 1:
//                 DrawCreateItemTab();
//                 break;
//         }
//     }
//
//     private void DrawCreateItemTab(){
//         GUILayout.Label("Item Icon");
//         itemIcon = (Sprite)EditorGUILayout.ObjectField(itemIcon, typeof(Sprite), false, GUILayout.Width(70), GUILayout.Height(70));
//         EditorGUILayout.Space(5);
//         
//         itemName = EditorGUILayout.TextField("Item Name", itemName);
//         itemID = EditorGUILayout.IntField("Item ID", itemID);
//         
//         GUILayout.Label("Item Description");
//         EditorStyles.textField.wordWrap = true; 
//         itemDescription = EditorGUILayout.TextArea(itemDescription, GUILayout.Height(60));
//         EditorStyles.textField.wordWrap = false;
//         
//         itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);
//         
//         
//         EditorGUILayout.Space(20);
//         
//         
//         if (GUILayout.Button("Create Item")) {
//             CreateItemAsset();
//         }
//     }
//     private void DrawManageItemsTab() {
//         EditorGUILayout.BeginHorizontal();
//         
//         DrawItemList();
//         DrawItemInspector();
//
//         EditorGUILayout.EndHorizontal();
//     }
//     
     // private void DrawItemList(){
     //     
     //     scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400));
     //     
     //     foreach (var item in allItems) {
     //         if (item == null) continue;
     //
     //         Color originalColour = GUI.backgroundColor;
     //         if (selectedItem == item) {
     //             GUI.backgroundColor = new Color(0f,1f,1f);
     //         }
     //         
     //         EditorGUILayout.BeginHorizontal("box");
     //
     //         GUI.backgroundColor = originalColour;
     //
     //         if (item.icon != null) {
     //             GUILayout.Box(item.icon.texture, GUILayout.Width(64), GUILayout.Height(64));
     //         } else {
     //             GUILayout.Space(64); 
     //         }
     //
     //         EditorGUILayout.BeginVertical();
     //         EditorGUILayout.LabelField(item.name, EditorStyles.boldLabel);
     //         EditorGUILayout.LabelField($"ID: {item.id}");
     //
     //         EditorGUILayout.EndVertical();
     //
     //         EditorGUILayout.EndHorizontal();
     //         Rect boxRect = GUILayoutUtility.GetLastRect();
     //
     //         if (GUI.Button(boxRect, GUIContent.none, GUIStyle.none)) {
     //             Selection.activeObject = item;
     //             EditorGUIUtility.PingObject(item);
     //             selectedItem = item;
     //         }
     //
     //     }
     //     EditorGUILayout.EndScrollView();
     // }
//
//     private void DrawItemInspector(){
//         EditorGUI.BeginDisabledGroup(!selectedItem);
//         EditorGUILayout.BeginVertical(GUILayout.Width(400));
//         
//         GUILayout.Label("Manage Existing Items", EditorStyles.boldLabel);
//         
//         EditorGUILayout.ObjectField(itemIcon, typeof(Sprite), false, GUILayout.Width(70), GUILayout.Height(70));
//         EditorGUILayout.TextField("Item Name", itemName);
//         EditorGUILayout.IntField("ID", itemID);
//         
//         EditorGUILayout.EndVertical();
//         EditorGUI.EndDisabledGroup();
//     }
//     
//
//
//     private void CreateItemAsset(){
//         ItemSO newItem = ScriptableObject.CreateInstance<ItemSO>();
//
//         newItem.name = itemName;
//         newItem.id = 0;
//         newItem.icon = itemIcon;
//         
//         AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath($"{path}/{itemName}.asset"));
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         
//         // EditorUtility.FocusProjectWindow();
//         // Selection.activeObject = newItem;
//
//     }
// }
