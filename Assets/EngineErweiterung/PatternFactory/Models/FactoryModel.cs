
using System.IO;

public class FactoryModel : IPattern
{
    public void Create(string name, string assetPath, string samplePath)
    {
        string nameWithSuffix = name + "Factory";

        string nameEnum = name + "Enum";
        string iname = "I" + nameWithSuffix;
        string inamename = "I" + name;


        FileInfo factory = new FileInfo(Path.Combine(assetPath, nameWithSuffix + ".cs"));
        FileInfo factoryEnum = new FileInfo(Path.Combine(assetPath, nameEnum + ".cs"));
        FileInfo iFactory = new FileInfo(Path.Combine(assetPath, iname + ".cs"));
        FileInfo iFactoryName = new FileInfo(Path.Combine(assetPath, inamename + ".cs"));

        FileInfo sampleFile = new FileInfo(Path.Combine(samplePath, "FactorySample.cs"));
        FileInfo sampleEnumFile = new FileInfo(Path.Combine(samplePath, "FactoryEnumSample.cs"));
        FileInfo iSampleFile = new FileInfo(Path.Combine(samplePath, "IFactorySample.cs"));
        FileInfo iSampleNameFile = new FileInfo(Path.Combine(samplePath, "IFactoryNameSample.cs"));

        string factoryContent = "";
        string factoryEnumContent = "";
        string iFactoryContent = "";
        string iFactoryNameContent = "";
        using (StreamReader sr = sampleFile.OpenText())
        {
            factoryContent = sr.ReadToEnd();
        }
        using (StreamReader sr = sampleEnumFile.OpenText())
        {
            factoryEnumContent = sr.ReadToEnd();
        }
        using (StreamReader sr = iSampleFile.OpenText())
        {
            iFactoryContent = sr.ReadToEnd();
        }
        using (StreamReader sr = iSampleNameFile.OpenText())
        {
            iFactoryNameContent = sr.ReadToEnd();
        }

        using (StreamWriter sw = factory.CreateText())
        {
            factoryContent = factoryContent.Replace(nameof(IFactoryNameSample), inamename);
            factoryContent = factoryContent.Replace(nameof(IFactorySample), iname);
            factoryContent = factoryContent.Replace(nameof(FactorySample), nameWithSuffix);
            sw.Write(factoryContent);
        }
        using (StreamWriter sw = factoryEnum.CreateText())
        {
            factoryEnumContent = factoryEnumContent.Replace(nameof(FactoryEnumSample), nameEnum);
            sw.Write(factoryEnumContent);
        }
        using (StreamWriter sw = iFactory.CreateText())
        {
            iFactoryContent = iFactoryContent.Replace(nameof(IFactoryNameSample), inamename);
            iFactoryContent = iFactoryContent.Replace(nameof(IFactorySample), iname);
            sw.Write(iFactoryContent);
        }
        using (StreamWriter sw = iFactoryName.CreateText())
        {
            iFactoryNameContent = iFactoryNameContent.Replace(nameof(IFactoryNameSample), inamename);
            sw.Write(iFactoryNameContent);
        }
    }
}
