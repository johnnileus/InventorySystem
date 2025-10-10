
using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Update.Partial;
using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManagerWindow : EditorWindow{

    private int itemSpriteSize = 1;
    
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
    private Dictionary<string, Sprite> iconDatabase = new Dictionary<string, Sprite>();
    private bool editing = false;
    private ItemSO selectedItem;
    
    //Item Inspector input fields
    private string itemName;
    private string itemDescription;
    private bool itemStackable;
    private Sprite itemIcon;
    
    
    [MenuItem("Window/ItemManager")]
    public static void ShowWindow(){
        GetWindow<ItemManagerWindow>("Item Manager");
    }

    private void OnEnable(){
        GenerateItemDatabase();
        GenerateIconDatabase();
    }

    private void OnGUI(){
        DrawNavigationBar();
    }

    private void GenerateItemDatabase(){ 
        itemDatabase.Clear();
        itemDatabase.AddRange(Resources.LoadAll<ItemSO>("Items"));
    }

    private void GenerateIconDatabase(){
        iconDatabase.Clear();
        iconDatabase = Resources.LoadAll<Sprite>("Icons").ToDictionary(sprite => sprite.name, sprite => sprite);
    }

    private void CreateEmptyItem(){
        ItemSO newItem = CreateInstance<ItemSO>();

        newItem.name = "New Item";
        newItem.description = Random.value.ToString();
        ItemAttribute damage = new ItemAttribute("Damage", AttributeType.Float);
        newItem.attributes.Add(damage);
        Debug.Log(newItem.GetAttribute("Damage"));

        AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Items/New Item.asset"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
        GUILayout.Label("Main");

    }

    private void DrawItemsMenu(){
        GenerateItemDatabase();

        GUIStyle style = new GUIStyle();
        
        GUILayout.Label("Items");
        EditorGUILayout.BeginHorizontal();
        DrawItemList();
        DrawItemInspector();
        EditorGUILayout.EndHorizontal(); 
    }

    private void DrawItemList(){
        EditorGUILayout.BeginVertical();
        itemListScrollPosition = EditorGUILayout.BeginScrollView(itemListScrollPosition, GUILayout.Width(250));
        
        foreach (var item in itemDatabase) {
            if (item == null) continue;
            Color originalColour = GUI.backgroundColor;
            if (item == selectedItem) {
                GUI.backgroundColor = new Color(0f, 0f, 1f);
            }
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = originalColour;
            if (item.icon == null) {
                GUILayout.Space(16);
            }
            else {
                GUILayout.Box(item.icon.texture, GUILayout.Width(16), GUILayout.Height(16));
            }
            GUILayout.Label(item.name);
            EditorGUILayout.EndHorizontal();
            Rect boxRect = GUILayoutUtility.GetLastRect();
            
            if (GUI.Button(boxRect, GUIContent.none, GUIStyle.none)) {
                Selection.activeObject = item;
                EditorGUIUtility.PingObject(item);
                selectedItem = item;
            }
        }
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Create New Item")) { 
            CreateEmptyItem();
        }
        EditorGUILayout.EndVertical();

    }

    private void DrawItemInspector(){
        
        GUIStyle statLine = new GUIStyle(GUI.skin.box);
        statLine.margin = new RectOffset(0, 0,0,0);

        GUIStyle leftCol = new GUIStyle(GUI.skin.label);
        leftCol.fixedWidth = 120;

        GUIStyle rightCol = new GUIStyle(GUI.skin.label);
        rightCol.fixedWidth = 240;
        
        
        
        EditorGUILayout.BeginVertical();
        
        
        if (selectedItem) {

        
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Default Attributes", EditorStyles.boldLabel, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Name", leftCol);
            if (editing) itemName = GUILayout.TextField(itemName);
            else GUILayout.Label(selectedItem.name, rightCol);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Category", leftCol);
            GUILayout.Label("TODO", rightCol);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Icon", leftCol, GUILayout.Height(32));
            if (editing) itemIcon = (Sprite) EditorGUILayout.ObjectField(itemIcon, typeof(Sprite), false);
            else GUILayout.Box(selectedItem.icon.texture, GUILayout.Height(32), GUILayout.Width(32));

            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("World Prefab", leftCol);
            GUILayout.Label("TODO", rightCol);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Description", leftCol);
            
            if (editing) itemDescription = GUILayout.TextArea(itemDescription);
            else GUILayout.Label(selectedItem.description, rightCol);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Stackable", leftCol);
            
            if (editing) itemStackable = EditorGUILayout.Toggle(itemStackable);
            else GUILayout.Label(selectedItem.stackable.ToString(), rightCol);
            EditorGUILayout.EndHorizontal();
        
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Custom Attributes", EditorStyles.boldLabel, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();

            foreach (var att in selectedItem.attributes) {
                EditorGUILayout.BeginHorizontal(statLine);
                GUILayout.Label(att.name, leftCol);
                GUILayout.Label(att.GetValueAsString(), rightCol);
                EditorGUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button(editing ? "Save" : "Edit", GUILayout.Width(360))) {

                if (editing) {
                    selectedItem.name = itemName;
                    selectedItem.description = itemDescription;
                    selectedItem.stackable = itemStackable;
                    selectedItem.icon = itemIcon;
                }
                
                editing = !editing;
            }
            
        }
        else {
            GUILayout.Label("Select an Item");
        }

        EditorGUILayout.EndVertical();

    }
    
    private void DrawCategoriesMenu(){
        if (GUILayout.Button("create category")) {
            CategorySO newCat = CreateInstance<CategorySO>();

            newCat.name = "sword";
        }
    }
    
    private void DrawModifiersMenu(){
        GUILayout.Label("modifiers");
        
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
