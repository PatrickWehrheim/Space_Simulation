using System;
using System.IO;

public class ProxyModel : IPattern
{
    public void Create(string name, string assetPath, string samplePath)
    {
        string nameWithSuffix = name + "Proxy";

        string iname = "I" + name;


        FileInfo proxy = new FileInfo(Path.Combine(assetPath, nameWithSuffix + ".cs"));
        FileInfo subject = new FileInfo(Path.Combine(assetPath, name + ".cs"));
        FileInfo iProxySubject = new FileInfo(Path.Combine(assetPath, iname + ".cs"));

        FileInfo sampleFile = new FileInfo(Path.Combine(samplePath, "ProxySample.cs"));
        FileInfo sampleSubjectFile = new FileInfo(Path.Combine(samplePath, "ProxySubjectSample.cs"));
        FileInfo iSampleFile = new FileInfo(Path.Combine(samplePath, "IProxySubjectSample.cs"));

        string proxyContent = "";
        string subjectContent = "";
        string iProxySubjectContent = "";
        using (StreamReader sr = sampleFile.OpenText())
        {
            proxyContent = sr.ReadToEnd();
        }
        using (StreamReader sr = sampleSubjectFile.OpenText())
        {
            subjectContent = sr.ReadToEnd();
        }
        using (StreamReader sr = iSampleFile.OpenText())
        {
            iProxySubjectContent = sr.ReadToEnd();
        }

        using (StreamWriter sw = proxy.CreateText())
        {
            proxyContent = proxyContent.Replace(nameof(IProxySubjectSample), iname);
            proxyContent = proxyContent.Replace(nameof(ProxySubjectSample), name);
            proxyContent = proxyContent.Replace(nameof(ProxySample), nameWithSuffix);
            sw.Write(proxyContent);
        }
        using (StreamWriter sw = subject.CreateText())
        {
            subjectContent = subjectContent.Replace(nameof(ProxySubjectSample), name);
            sw.Write(subjectContent);
        }
        using (StreamWriter sw = iProxySubject.CreateText())
        {
            iProxySubjectContent = iProxySubjectContent.Replace(nameof(IProxySubjectSample), iname);
            sw.Write(iProxySubjectContent);
        }
    }
}
