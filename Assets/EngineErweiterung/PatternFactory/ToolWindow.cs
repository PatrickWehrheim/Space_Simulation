
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ToolWindow : EditorWindow
{
    private static ToolWindow _window;

    [MenuItem("Tools/Custom_Unity_Tools %t")] // Öffnen mit Strg + T
    private static void ShowWindow()
    {
        _window = GetWindow<ToolWindow>("Custom Unity Tools");
        _window.Show/*Modal*/();
    }

    private void CreateGUI()
    {
        CreatePatternWindow();
    }

    private void CreateToolbar()
    {
        Toolbar toolbar = new Toolbar();
        ToolbarButton patternButton = new ToolbarButton(CreatePatternWindow) { text = "Patterns" };
        toolbar.Add(patternButton);


        rootVisualElement.Add(toolbar);
    }

    private void CreatePatternWindow()
    {
        rootVisualElement.Clear();
        CreateToolbar();

        PatternCreationController creationController = new PatternCreationController(new PatternCreationView());
        
        VisualElement patternCreationElement = creationController.LoadView();

        rootVisualElement.Add(patternCreationElement);
    }

    
}
