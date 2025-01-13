
using System.IO;

public class UnitTestModel : IPattern
{
    public void Create(string name, string assetPath, string samplePath)
    {
        samplePath = Path.Combine(samplePath, "Tests");

        string nameWithSuffix = name + "Test";

        FileInfo uintyTest = new FileInfo(Path.Combine(assetPath, nameWithSuffix + ".cs"));

        FileInfo sampleFile = new FileInfo(Path.Combine(samplePath, "UnitTestSample.cs"));

        string uintTestContent = "";
        using (StreamReader sr = sampleFile.OpenText())
        {
            uintTestContent = sr.ReadToEnd();
        }

        using (StreamWriter sw = uintyTest.CreateText())
        {
            uintTestContent = uintTestContent.Replace("UnitTestSample", nameWithSuffix);
            sw.Write(uintTestContent);
        }
    }
}
