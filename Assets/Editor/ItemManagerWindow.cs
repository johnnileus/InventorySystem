
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


    private MenuName currentMenu = MenuName.Main;
    private Vector2 itemListScrollPosition;
    
    private List<ItemSO> itemDatabase = new();
    private List<CategorySO> catDatabase = new();
    private Dictionary<string, Sprite> iconDatabase = new();
    private bool editing = false;
    private ItemSO selectedItem = null;
    private CategorySO selectedCat = null;
    
    //Item Inspector input fields
    private string itemName;
    private CategorySO itemCategory;
    private string itemDescription;
    private int itemMaxStack;
    private Sprite itemIcon;
    
    AttributeType newAtrType = AttributeType.None;
    private List<UIAttributeBase> itemAttributes = new(); //cloned attributes of currently selected item

    private GUIStyle statLine;

    private GUIStyle leftCol;

    private GUIStyle rightCol;

    
    [MenuItem("Window/ItemManager")]
    public static void ShowWindow(){
        GetWindow<ItemManagerWindow>("Item Manager");
    }

    private void OnEnable(){
        


        
        
        GenerateItemDatabase();
        GenerateCatDatabase();
        GenerateIconDatabase();
        RefreshData();  
        
        
        
    }

    private void OnGUI(){
        DrawNavigationBar();
        
        InitialiseGUIStyles();
    }

    private void InitialiseGUIStyles(){
        statLine = new GUIStyle(GUI.skin.box);
        statLine.margin = new RectOffset(0, 0,0,0);

        leftCol = new GUIStyle(GUI.skin.label);
        leftCol.fixedWidth = 120;

        rightCol = new GUIStyle(GUI.skin.label);
        rightCol.fixedWidth = 240;
    }

    private void GenerateItemDatabase(){ 
        itemDatabase.Clear();
        itemDatabase.AddRange(Resources.LoadAll<ItemSO>("Items"));
    }

    private void GenerateCatDatabase(){
        catDatabase.Clear();
        catDatabase.AddRange(Resources.LoadAll<CategorySO>("Categories"));
    }

    private void GenerateIconDatabase(){
        iconDatabase.Clear();
        iconDatabase = Resources.LoadAll<Sprite>("Icons").ToDictionary(sprite => sprite.name, sprite => sprite);
    }

    private void CreateEmptyItem(){
        ItemSO newItem = CreateInstance<ItemSO>();

        newItem.name = "New Item";
        newItem.description = Random.value.ToString();
        newItem.attributes.Add(new StatAttributeFloat("Damage", Random.value));

        AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Items/New Item.asset"));

        Debug.Log("Created new item");
        GenerateItemDatabase();


    }

    private void CreateEmptyCategory(){
        CategorySO newCat = CreateInstance<CategorySO>();
        newCat.name = "New Category";
        Debug.Log("Created new category");
        
        AssetDatabase.CreateAsset(newCat, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Categories/New Category.asset"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        GenerateCatDatabase();

    }
    
    private void RefreshData(){
        itemAttributes.Clear();

        if (!selectedItem) return;
        
        
        
        foreach (var attribute in selectedItem.attributes) {
            UIAttributeBase att = null;
            switch (attribute) {
                case StatAttributeInt:
                    att = new UIAttributeInt(attribute.name,(int) attribute.GetValue());
                    break;
                case StatAttributeFloat:
                    att = new UIAttributeFloat(attribute.name,(float) attribute.GetValue());
                    break;
                case StatAttributeString:
                    att = new UIAttributeString(attribute.name,(string) attribute.GetValue());
                    break;
                case StatAttributeBool:
                    att = new UIAttributeBool(attribute.name,(bool) attribute.GetValue());
                    break;
            }
            itemAttributes.Add(att);
        }
    }
    
    private void CreateAttribute(){
        UIAttributeBase attr = null;
        switch (newAtrType) {
            case AttributeType.Integer:
                attr = new UIAttributeInt("New Int Attribute", 0);
                break;
            case AttributeType.Float:
                attr = new UIAttributeFloat("New Float Attribute", 0.0f);
                break;
            case AttributeType.String:
                attr = new UIAttributeString("New String Attribute", "");
                break;
            case AttributeType.Boolean:
                attr = new UIAttributeBool("New Bool Attribute", false);
                break;
        }

        if (attr != null) {
            itemAttributes.Add(attr);
        }
    }

    private void DeleteItem(){
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedItem));
        GenerateItemDatabase();

    }

    private void DeleteCategory(){
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedCat));
        GenerateCatDatabase();
    }
    
     
    private void DrawNavigationBar(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Epic Item Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Items")) {
            editing = false;
            currentMenu = MenuName.Items;
        }
        if (GUILayout.Button("Categories")) {
            editing = false;
            currentMenu = MenuName.Categories;
        }
        if (GUILayout.Button("Modifiers")) {
            editing = false;
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

    #region ItemMenu
    private void DrawItemsMenu(){

        GUILayout.Label("Items", EditorStyles.boldLabel);

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
            if (editing) itemCategory = (CategorySO)EditorGUILayout.ObjectField(itemCategory, typeof(CategorySO), false);
            else GUILayout.Label(selectedItem.category == null ? "None" : selectedItem.category.name, rightCol);
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
            GUILayout.Label("Maximum Stack", leftCol); 
            if (editing) itemMaxStack = EditorGUILayout.IntField(itemMaxStack);
            else GUILayout.Label(selectedItem.maxStack.ToString(), rightCol);
            EditorGUILayout.EndHorizontal();
            
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Custom Attributes", EditorStyles.boldLabel, GUILayout.Width(120));
            if (editing) {
                if (GUILayout.Button("Add Attribute")) CreateAttribute();

                newAtrType = (AttributeType)EditorGUILayout.EnumPopup(newAtrType);
            }


            EditorGUILayout.EndHorizontal();
            foreach (var att in itemAttributes) {
                EditorGUILayout.BeginHorizontal(statLine);

                if (editing) {
                    att.name = EditorGUILayout.TextField(att.name, GUILayout.Width(120));

                    switch (att) {
                        case UIAttributeInt:
                            int intval = EditorGUILayout.IntField((int)att.GetValue());
                            att.SetValue(intval);
                            break;
                        case UIAttributeFloat:
                            float floatval = EditorGUILayout.FloatField((float)att.GetValue());
                            att.SetValue(floatval);
                            break;
                        case UIAttributeString:
                            string stringval = EditorGUILayout.TextField((string)att.GetValue());
                            att.SetValue(stringval);
                            break;
                        case UIAttributeBool:
                            bool boolval = EditorGUILayout.Toggle((bool)att.GetValue());
                            att.SetValue(boolval);
                            break;
                    }

                    if (GUILayout.Button(att.toDelete ? "Restore" : "Delete")) {
                        att.toDelete = !att.toDelete;
                    }
                    
                }
                else {
                    GUILayout.Label(att.name, leftCol);
                    GUILayout.Label(att.GetValue().ToString());
                }
                EditorGUILayout.EndHorizontal();

            }
            
            if (GUILayout.Button(editing ? "Save" : "Edit", GUILayout.Width(360))) {

                if (editing) { // clicked on Save
                    selectedItem.name = itemName;
                    selectedItem.category = itemCategory;
                    selectedItem.description = itemDescription;
                    selectedItem.maxStack = itemMaxStack;
                    selectedItem.icon = itemIcon;
                    
                    selectedItem.attributes.Clear();
                    foreach (var attr in itemAttributes) {
                        selectedItem.CloneUIAttribute(attr);
                    }
                }
                else { // clicked on Edit
                    itemName = selectedItem.name;
                    itemCategory = selectedItem.category;
                    itemDescription = selectedItem.description;
                    itemMaxStack = selectedItem.maxStack;
                    itemIcon = selectedItem.icon;
                    for (int i = 0; i < selectedItem.attributes.Count; i++) {
                        itemAttributes[i].SetValue(selectedItem.attributes[i].GetValue());
                    }
                }
                
                editing = !editing;
            }
            
        }
        else {
            GUILayout.Label("Select an Item");
        }

        EditorGUILayout.EndVertical();

    }
    #endregion
     
    private void DrawCategoriesMenu(){
        
        GUILayout.Label("Categories", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();

        DrawCategoryList();
        DrawCategoryInspector();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal(); 
        

    }

    private void DrawCategoryList(){
        EditorGUILayout.BeginVertical();

        itemListScrollPosition = EditorGUILayout.BeginScrollView(itemListScrollPosition, GUILayout.Width(250));
        
        foreach (var cat in catDatabase) {
            if (cat == null) continue;
            Color originalColour = GUI.backgroundColor;
            if (cat == selectedCat) {
                GUI.backgroundColor = new Color(0f, 0f, 1f);
            }
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = originalColour;

            GUILayout.Label(cat.name);
            EditorGUILayout.EndHorizontal();
            Rect boxRect = GUILayoutUtility.GetLastRect();
            
            if (GUI.Button(boxRect, GUIContent.none, GUIStyle.none)) {
                Selection.activeObject = cat;
                EditorGUIUtility.PingObject(cat);
                selectedCat = cat;
                RefreshData();

                editing = false;
            }
        }
        EditorGUILayout.EndScrollView();
        
        if (GUILayout.Button("Create New Item")) { 
            CreateEmptyCategory();
            
        }

        if (GUILayout.Button("Delete Selected Item")) {
            DeleteCategory();
        }
        
        EditorGUILayout.EndVertical();

    }

    private void DrawCategoryInspector(){
        
        EditorGUILayout.BeginVertical();


        if (selectedCat) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Category Attributes", EditorStyles.boldLabel, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(statLine);
            GUILayout.Label("Name", leftCol);
            if (editing) itemName = GUILayout.TextField(itemName);
            else GUILayout.Label(selectedCat.name, rightCol);
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button(editing ? "Save" : "Edit", GUILayout.Width(360))) {

            if (editing) { // clicked on Save
                
                selectedCat.name = itemName;


            }
            else { // clicked on Edit
                itemName = selectedCat.name;

            }
                
            editing = !editing;
        }
        EditorGUILayout.EndVertical();
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

}
