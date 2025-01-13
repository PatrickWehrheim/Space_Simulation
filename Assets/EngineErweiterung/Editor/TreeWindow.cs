using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public enum Tabs
{
    PresetList,
    CurrentTree,
    PatternCreation,
    Settings
}

public class TreeWindow : EditorWindow
{
    private static TreeWindow window;

    private TabDrawer tabDrawer;
    private NodeCreationDrawer nodeCreationDrawer;
    private WindowDrawer windowDrawer;

    private Rect windowRect = new Rect(50, 50, 150, 50);

    private TextField _textField;

    [MenuItem("Tools/MyWindow _i")] //_i Shortcut mit I | %i Ctrl + i | #i Shift + i | %#i Ctrl + Shift + i | &i Alt + i | ...
    private static void ShowWindow()
    {
        window = GetWindow<TreeWindow>("Windows is god");
        window.Show();
    }

    private void OnEnable()
    {
        tabDrawer = new TabDrawer();
        nodeCreationDrawer = new NodeCreationDrawer();
        windowDrawer = new WindowDrawer(nodeCreationDrawer.CompositNodeTypes);
    }

    private void CreateGUI()
    {
        tabDrawer.DrawTabs(rootVisualElement);

        switch (tabDrawer.CurrentlySelectedTab)
        {
            case Tabs.PresetList:
                //nodeCreationDrawer.DrawNodeCreationButtons(windowDrawer);
                break;
            case Tabs.CurrentTree:
                //windowDrawer.RedrawWindows(this);
                break;
            case Tabs.Settings:
                //if (GUI.Button(new Rect(50, 50, 100, 100), "Save"))
                //{
                //    //do Stuff
                //}
                //if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
                //{
                //    //do Stuff
                //}
                break;
            case Tabs.PatternCreation:
                rootVisualElement.Add(new Button(new System.Action(CreatePattern)) { name = "Singleton" });
                _textField = new TextField();
                rootVisualElement.Add(_textField);
                //path = GUI.TextField(new Rect(10, 75, 200, 20), path);
                //if (GUI.Button(new Rect(50, 175, 100, 100), "Singleton"))
                //{
                //    CreatePattern();
                //}
                break;
            default:
                break;
        }
    }
    private void CreatePattern()
    {
        PatternFactory factory = new PatternFactory();
        IPattern pattern = factory.CreatePattern("Test", _textField.text, new SingletonModel());
    }
}
