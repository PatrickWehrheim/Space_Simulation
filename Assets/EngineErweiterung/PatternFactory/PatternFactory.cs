
using System.IO;
using UnityEditor;

public class PatternFactory : IPatternFactory
{
    public IPattern CreatePattern(string fileName, string assetPath, IPattern pattern)
    {
        string samplePath = Path.Combine(Directory.GetCurrentDirectory(),
                                                        "Assets",
                                                        "EngineErweiterung",
                                                        "PatternFactory",
                                                        "Models",
                                                        "Samples");
        pattern.Create(fileName, assetPath, samplePath);
        AssetDatabase.Refresh();
        return pattern;
    }
}
