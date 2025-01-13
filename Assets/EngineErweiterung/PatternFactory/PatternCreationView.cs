
using UnityEngine.UIElements;

public class PatternCreationView : VisualElement, IPatternCreationView
{
    private PatternCreationController _controller;

    public void SetController(PatternCreationController controller)
    {
        _controller = controller;
    }

    public void BuildPattern(PatternEnum pattern)
    {
        switch (pattern)
        {
            case PatternEnum.Singleton:
                break;
            case PatternEnum.Factory:
                break;
            case PatternEnum.Proxy:
                break;
        }
    }

    public VisualElement CreatePatternWindow()
    {
        this.style.flexDirection = FlexDirection.Row;

        VisualElement verticalGroup0 = new VisualElement()
        {
            style =
            {
                width = 300,
            }
        };
        VisualElement verticalGroup1 = new VisualElement()
        {
            style =
            {
                flexGrow = 1,
            }
        };
        VisualElement verticalGroup2 = new VisualElement()
        {
            style =
            {
                width = 200,
            }
        };

        this.Add(verticalGroup0);
        this.Add(verticalGroup1);
        this.Add(verticalGroup2);

        for (int i = 0; i < (int)PatternEnum.NONE; i++)
        {
            _controller.Patterns.Add(((PatternEnum)i).ToString());
        }

        Label fileLabelPreview = new Label();
        TextField fileName = new TextField();
        DropdownField dropdown = new DropdownField("Patterns", _controller.Patterns, 0) { style = { maxWidth = 300 } };
        PatternEnum pattern = 0;
        dropdown.RegisterValueChangedCallback((callback) =>
        {
            pattern = (PatternEnum)_controller.Patterns.IndexOf(callback.newValue);
            _controller.PatternChanged(callback.newValue);
            _controller.FileNameChanged(fileName.text, fileLabelPreview, pattern);
        });
        dropdown.MarkDirtyRepaint();

        fileName.RegisterValueChangedCallback(callback =>
        {
            _controller.FileNameChanged(callback.newValue, fileLabelPreview, pattern);
        });

        verticalGroup0.Add(dropdown);
        verticalGroup1.Add(fileName);
        verticalGroup2.Add(fileLabelPreview);

        Button createButton = new Button(() =>
        {
            _controller.CreatePattern(fileName.text, pattern);
        })
        { text = "Create Pattern" };

        verticalGroup1.Add(createButton);

        return this;
    }
}
