using System.IO;
using UnityEditor;

public class SingletonModel : IPattern
{
    public void Create(string name, string assetPath, string samplePath)
    {
        FileInfo fileInfo = new FileInfo(Path.Combine(assetPath, name + ".cs"));
        FileInfo sampleFile = new FileInfo(Path.Combine(samplePath, "SingletonSample.cs"));

        string content = "";
        using (StreamReader sr = sampleFile.OpenText()) 
        {
            content = sr.ReadToEnd();
        }
        using (StreamWriter sw = fileInfo.CreateText())
        {
            content = content.Replace(nameof(SingletonSample), name);
            sw.Write(content);
        }
    }
}
