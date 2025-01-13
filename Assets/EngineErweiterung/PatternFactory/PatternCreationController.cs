
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;

public class PatternCreationController
{
    private IPatternCreationView _view;
    private List<string> _patterns;

    public List<string> Patterns { get => _patterns; }

    public PatternCreationController(IPatternCreationView view)
	{
        _patterns = new List<string>();
		_view = view;
        _view.SetController(this);
    }

    public VisualElement LoadView()
    {
        return _view.CreatePatternWindow();
    }

    public void CreatePattern(string fileName, PatternEnum pattern)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        PatternFactory factory = new PatternFactory();
        IPattern patternToCreate;

        switch (pattern)
        {
            case PatternEnum.Singleton:
                patternToCreate = new SingletonModel();
                break;
            case PatternEnum.Factory:
                patternToCreate = new FactoryModel();
                break;
            case PatternEnum.Proxy:
                patternToCreate = new ProxyModel();
                break;
            case PatternEnum.UnitTest:
                patternToCreate = new UnitTestModel();
                break;
            case PatternEnum.MVC:
                patternToCreate = new MVCModel();
                break;
            default:
                patternToCreate = new SingletonModel();
                break;
        }
        IPattern createdPattern = factory.CreatePattern(fileName, filePath, patternToCreate);
    }

    public void PatternChanged(string newValue)
    {
        if (_patterns == null) return;
        if (!_patterns.Contains(newValue)) return;

        int index = _patterns.IndexOf(newValue);
        _view.BuildPattern((PatternEnum)index);
    }

    public void FileNameChanged(string fileName, Label fileLabelPreview, PatternEnum pattern)
    {
        string suffix = "";

        switch (pattern)
        {
            case PatternEnum.Factory:
                suffix = "Factory";
                break;
            case PatternEnum.Proxy:
                suffix = "Proxy";
                break;
            case PatternEnum.UnitTest:
                suffix = "Test";
                break;
        }
        fileLabelPreview.text = " = " + fileName + suffix + ".cs";
    }
}
