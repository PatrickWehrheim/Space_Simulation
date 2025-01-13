
using UnityEngine.UIElements;

public interface IPatternCreationView
{
    public void SetController(PatternCreationController controller);
    public void BuildPattern(PatternEnum pattern);
    public VisualElement CreatePatternWindow();
}