using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

public class FavoritesWindow : EditorWindow
{
    private const string CategoriesKey = "FavoritesWindow_Categories";
    private static readonly Color SelectedColor = new(0.24f, 0.37f, 0.59f);

    private List<Category> _categories = new();
    private string _searchQuery = "";
    private Texture2D _folderIcon;
    private Texture2D _activefolderIcon;
    private Category _selectedCategory;
    private Object _selectedObject;
    private Vector2 _scrollPosition;

    [MenuItem("Window/Favorites")]
    public static void ShowWindow()
    {
        GetWindow<FavoritesWindow>("Favorites");
    }

    private void OnEnable()
    {
        _folderIcon = EditorGUIUtility.FindTexture("Folder Icon");
        _activefolderIcon = EditorGUIUtility.FindTexture("FolderOpened Icon");
        LoadCategories();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        Rect searchRect = GUILayoutUtility.GetRect(100, 15, GUILayout.ExpandWidth(true));
        searchRect.position = new Vector2(3, 3);
        var method = typeof(EditorGUI).GetMethod(
            "ToolbarSearchField",
            BindingFlags.NonPublic | BindingFlags.Static,
            null,
            new[] { typeof(Rect), typeof(string), typeof(bool) },
            null
        );

        if (method != null)
            _searchQuery = (string)method.Invoke(null, new object[] { searchRect, _searchQuery, false });

        if (GUILayout.Button("+", GUILayout.Width(16), GUILayout.Height(18))) AddCategory();
        if (GUILayout.Button("-", GUILayout.Width(16), GUILayout.Height(18))) Remove();

        GUILayout.EndHorizontal();

        var filteredCategories = string.IsNullOrEmpty(_searchQuery)
            ? _categories
            : _categories.Where(c => c.name.ToLower().Contains(_searchQuery.ToLower())).ToList();

        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(0, 0, -1, 0),
            fixedHeight = 25,
            fixedWidth = 25,
        };
        
        GUIStyle foldoutStyleText = new GUIStyle(EditorStyles.label)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(-35, 0, 0, 0),
            margin = new RectOffset(0, 0, 2, 0),
            imagePosition = ImagePosition.ImageAbove,
            fixedHeight = 16,
            fixedWidth = 16,
        };

        GUIStyle textureStyle = new GUIStyle(EditorStyles.label)
        {
            padding = new RectOffset(-37, 0, 0, 0),
            margin = new RectOffset(0, 0, 2, 0),
            alignment = TextAnchor.MiddleLeft,
            fixedHeight = 16,
            fixedWidth = 16,
        };

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        GUILayout.Label("Categories", EditorStyles.boldLabel);
        foreach (var category in filteredCategories)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(-4);
            GUILayout.Label(GUIContent.none, GUILayout.Width(0));
            Rect startRect = GUILayoutUtility.GetLastRect();
            Rect fullRect = startRect;
            fullRect.width = EditorGUIUtility.currentViewWidth;

            if (_selectedCategory == category && _selectedObject == null)
            {
                Texture2D selectedTexture = new Texture2D(1, 1);
                selectedTexture.SetPixel(0, 0, SelectedColor);
                selectedTexture.Apply();
                EditorGUI.DrawPreviewTexture(fullRect, selectedTexture);
            }

            // Теперь сначала рисуем стрелку Foldout, затем иконку папки
            category.isFoldout = EditorGUILayout.Foldout(category.isFoldout, GUIContent.none, true, foldoutStyle);
            GUILayout.Label(category.isFoldout ? _activefolderIcon : _folderIcon, textureStyle);
            GUILayout.Label(category.name, foldoutStyleText); // Текст категории отдельно

            SaveCategories();

            if (Event.current.type == EventType.MouseDown && fullRect.Contains(Event.current.mousePosition))
            {
                _selectedCategory = category;
                _selectedObject = null;
                GUI.FocusControl(null);
                Event.current.Use();
            }

            GUILayout.EndHorizontal();

            if (category.isFoldout)
            {
                foreach (var item in category.items)
                {
                    GUIContent content = EditorGUIUtility.ObjectContent(item, typeof(Object));
                    GUIStyle style = new GUIStyle(EditorStyles.objectField)
                    {
                        imagePosition = ImagePosition.ImageLeft,
                        alignment = TextAnchor.MiddleLeft,
                        fixedHeight = 16
                    };

                    bool isSelected = _selectedObject == item;
                    if (isSelected)
                    {
                        Texture2D selectedTexture = new Texture2D(1, 1);
                        selectedTexture.SetPixel(0, 0, SelectedColor);
                        selectedTexture.Apply();

                        style.normal.background = selectedTexture;

                        if (Event.current.clickCount == 2 && _selectedObject != null)
                        {
                            string path = AssetDatabase.GetAssetPath(_selectedObject);
                            if (AssetDatabase.IsValidFolder(path))
                            {
                                Selection.activeObject = _selectedObject;
                                EditorGUIUtility.PingObject(_selectedObject);
                                OpenDirectory(path);
                            }
                            else
                            {
                                if (AssetDatabase.CanOpenAssetInEditor(_selectedObject.GetInstanceID()))
                                    AssetDatabase.OpenAsset(_selectedObject);
                            }
                        }
                    }
                    else
                    {
                        style.normal.background = null;
                    }

                    if (GUILayout.Button(content, style))
                    {
                        EditorGUIUtility.PingObject(item);
                        _selectedObject = item;
                        _selectedCategory = category;
                    }
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Selected Object", GUILayout.ExpandWidth(true)))
                    AddSelectedObjectToCategory(category);
                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void AddCategory()
    {
        if (!string.IsNullOrEmpty(_searchQuery) && !_categories.Any(c => c.name == _searchQuery))
        {
            _categories.Add(new Category { name = _searchQuery });
            _searchQuery = "";
            SaveCategories();
        }
    }

    private void Remove()
    {
        if (_selectedCategory != null && _selectedObject == null)
        {
            _categories.Remove(_selectedCategory);
            _searchQuery = "";
            SaveCategories();
        }

        if (_selectedObject != null)
        {
            _selectedCategory.items.Remove(_selectedObject);
            _selectedObject = null;
            _searchQuery = "";
            SaveCategories();
        }
    }

    private void AddSelectedObjectToCategory(Category category)
    {
        var selectedObject = Selection.activeObject;

        if (selectedObject != null)
        {
            category.items.Add(selectedObject);
            SaveCategories();
        }
        else
        {
            Debug.LogWarning("No object selected to add.");
        }
    }

    private void LoadCategories()
    {
        if (EditorPrefs.HasKey(CategoriesKey))
        {
            string categoriesData = EditorPrefs.GetString(CategoriesKey);
            _categories = JsonUtility.FromJson<CategoryListWrapper>(categoriesData).categories;
        }

        _categories ??= new List<Category>();
    }

    private void SaveCategories()
    {
        string categoriesData = JsonUtility.ToJson(new CategoryListWrapper { categories = _categories });
        EditorPrefs.SetString(CategoriesKey, categoriesData);
    }

    [Serializable]
    public class CategoryListWrapper
    {
        public List<Category> categories;
    }

    [Serializable]
    public class Category
    {
        public string name;
        public List<Object> items = new();
        public bool isFoldout;
    }

    private static void OpenDirectory(string path)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath(path);
        if (asset == null) return;

        var projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        var lastBrowser = projectBrowserType
            ?.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public)
            ?.GetValue(null);
        var showFolderMethod = projectBrowserType
            ?.GetMethod("ShowFolderContents", BindingFlags.NonPublic | BindingFlags.Instance);

        showFolderMethod?.Invoke(lastBrowser, new object[] { asset.GetInstanceID(), true });
    }
}