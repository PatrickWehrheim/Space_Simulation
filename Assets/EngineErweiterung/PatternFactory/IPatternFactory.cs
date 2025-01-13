
public interface IPatternFactory
{
    public IPattern CreatePattern(string fileName, string assetPath, IPattern pattern);
}