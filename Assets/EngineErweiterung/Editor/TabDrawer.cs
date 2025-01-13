using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TabDrawer
{

    private Tabs[] allCategories;
    private string[] allCategoryLabels;
    private Tabs currentlySelectedTab;

    public Tabs CurrentlySelectedTab => currentlySelectedTab;

    public TabDrawer()
    {
        InitTabs();
    }

    public void DrawTabs(VisualElement element)
    {
        int index = (int)currentlySelectedTab;

        VisualElement toolbar = new Toolbar() { tabIndex = index };
        foreach (Tabs category in allCategories)
        {
            toolbar.AddToClassList(category.ToString());
        }
        element.Add(toolbar); 
        //(index, allCategoryLabels));
        currentlySelectedTab = allCategories[index];
    }

    public void InitTabs()
    {
        allCategories = (Tabs[])System.Enum.GetValues(typeof(Tabs));

        allCategoryLabels = allCategories.ToList().ConvertAll(tab => tab.ToString()).ToArray();
    }
}
