
using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Update.Partial;
using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;




public class ItemManagerWindow : EditorWindow{


    private enum MenuName{
        Main,
        Items,
        Categories,
        Modifiers,
        Crafting,
        Currencies,
        Tags
    }

    private string[] categoryTypesString = Enum.GetNames(typeof(AttributeType));

    private MenuName currentMenu = MenuName.Main;
    private Vector2 itemListScrollPosition;
    
    private List<ItemSO> itemDatabase = new List<ItemSO>();
    private Dictionary<string, Sprite> iconDatabase = new Dictionary<string, Sprite>();
    private bool editing = false;
    private ItemSO selectedItem = null;
    
    //Item Inspector input fields
    private string itemName;
    private string itemCategory;
    private string itemDescription;
    private bool itemStackable;
    private Sprite itemIcon;
    private List<ItemAttributeBase> itemAttributes = new List<ItemAttributeBase>(); //cloned attributes of currently selected item

    
    
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
        newItem.attributes.Add(new ItemAttributeFloat("Damage", Random.value));
        Debug.Log(newItem.GetAttribute("Damage").GetValue());

        AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Items/New Item.asset"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
    }
    
    private void RefreshData(){
        itemAttributes.Clear();
        
        
        
        foreach (var attribute in selectedItem.attributes) {
            ItemAttributeBase att = null;
            switch (attribute) {
                case ItemAttributeInt:
                    att = new ItemAttributeInt(attribute.name,(int) attribute.GetValue());
                    break;
                case ItemAttributeFloat:
                    att = new ItemAttributeFloat(attribute.name,(float) attribute.GetValue());
                    break;
                case ItemAttributeString:
                    att = new ItemAttributeString(attribute.name,(string) attribute.GetValue());
                    break;
                case ItemAttributeBool:
                    att = new ItemAttributeBool(attribute.name,(bool) attribute.GetValue());
                    break;
            }
            itemAttributes.Add(att);
        }
    }
    
    private void CreateAttribute(){
        
    }

    private void DeleteItem(){
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedItem));
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

        EditorGUILayout.BeginHorizontal(GUILayout.Width(250));
        GUILayout.Label("Items", EditorStyles.boldLabel);
        if (GUILayout.Button("Reset")) {
            //TODO
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawItemList();
        DrawItemInspector();
        GUILayout.FlexibleSpace();
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
                RefreshData();

                editing = false;
            }
        }
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Create New Item")) { 
            CreateEmptyItem();
            
        }

        if (GUILayout.Button("Delete Selected Item")) {
            DeleteItem();
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
            if (editing) itemCategory = GUILayout.TextField(itemCategory);
            else GUILayout.Label(selectedItem.category, rightCol);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Icon", leftCol, GUILayout.Height(32));
            if (editing) itemIcon = (Sprite) EditorGUILayout.ObjectField(itemIcon, typeof(Sprite), false);
            else if (selectedItem.icon) GUILayout.Box(selectedItem.icon.texture, GUILayout.Height(32), GUILayout.Width(32));
            else GUILayout.Box(iconDatabase["empty"].texture, GUILayout.Height(32), GUILayout.Width(32));
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
            if (editing) if (GUILayout.Button("Add Attribute")) CreateAttribute();


            EditorGUILayout.EndHorizontal();
            foreach (var att in itemAttributes) {
                EditorGUILayout.BeginHorizontal(statLine);
                GUILayout.Label(att.name, leftCol);

                if (editing) {
                    switch (att) {
                        case ItemAttributeInt:
                            int intval = EditorGUILayout.IntField((int)att.GetValue());
                            att.SetValue(intval);
                            break;
                        case ItemAttributeFloat:
                            float floatval = EditorGUILayout.FloatField((float)att.GetValue());
                            att.SetValue(floatval);
                            break;
                        case ItemAttributeString:
                            string stringval = EditorGUILayout.TextField((string)att.GetValue());
                            att.SetValue(stringval);
                            break;
                        case ItemAttributeBool:
                            bool boolval = EditorGUILayout.Toggle((bool)att.GetValue());
                            att.SetValue(boolval);
                            break;
                    }
                }
                else {
                    GUILayout.Label(att.GetValue().ToString());
                }
                EditorGUILayout.EndHorizontal();

            }
            
            if (GUILayout.Button(editing ? "Save" : "Edit", GUILayout.Width(360))) {

                if (editing) {
                    selectedItem.name = itemName;
                    selectedItem.category = itemCategory;
                    selectedItem.description = itemDescription;
                    selectedItem.stackable = itemStackable;
                    selectedItem.icon = itemIcon;
                }
                else {
                    itemName = selectedItem.name;
                    itemCategory = selectedItem.category;
                    itemDescription = selectedItem.description;
                    itemStackable = selectedItem.stackable;
                    itemIcon = selectedItem.icon;
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
