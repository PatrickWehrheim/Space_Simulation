
using System.IO;

public class MVCModel : IPattern
{
    public void Create(string name, string assetPath, string samplePath)
    {
        string nameModel = name + "Model";
        string nameController = name + "Controller";
        string nameView = name + "View";
        string iNameView = "I" + nameView;


        FileInfo mvcModel = new FileInfo(Path.Combine(assetPath, nameModel + ".cs"));
        FileInfo mvcController = new FileInfo(Path.Combine(assetPath, nameController + ".cs"));
        FileInfo mvcView = new FileInfo(Path.Combine(assetPath, nameView + ".cs"));
        FileInfo iMvcView = new FileInfo(Path.Combine(assetPath, iNameView + ".cs"));

        FileInfo sampleModelFile = new FileInfo(Path.Combine(samplePath, "MVCModelSample.cs"));
        FileInfo sampleControllerFile = new FileInfo(Path.Combine(samplePath, "MVCControllerSample.cs"));
        FileInfo SampleViewFile = new FileInfo(Path.Combine(samplePath, "MVCViewSample.cs"));
        FileInfo iSampleViewFile = new FileInfo(Path.Combine(samplePath, "IMVCViewSample.cs"));

        string modelContent = "";
        string controllerContent = "";
        string viewContent = "";
        string iViewContent = "";
        using (StreamReader sr = sampleModelFile.OpenText())
        {
            modelContent = sr.ReadToEnd();
        }
        using (StreamReader sr = sampleControllerFile.OpenText())
        {
            controllerContent = sr.ReadToEnd();
        }
        using (StreamReader sr = SampleViewFile.OpenText())
        {
            viewContent = sr.ReadToEnd();
        }
        using (StreamReader sr = iSampleViewFile.OpenText())
        {
            iViewContent = sr.ReadToEnd();
        }

        using (StreamWriter sw = mvcModel.CreateText())
        {
            modelContent = modelContent.Replace(nameof(MVCModelSample), nameModel);
            sw.Write(modelContent);
        }
        using (StreamWriter sw = mvcController.CreateText())
        {
            controllerContent = controllerContent.Replace(nameof(MVCControllerSample), nameController);
            controllerContent = controllerContent.Replace(nameof(IMVCViewSample), iNameView);
            sw.Write(controllerContent);
        }
        using (StreamWriter sw = mvcView.CreateText())
        {
            viewContent = viewContent.Replace(nameof(MVCViewSample), nameView);
            viewContent = viewContent.Replace(nameof(MVCControllerSample), nameController);
            viewContent = viewContent.Replace(nameof(IMVCViewSample), iNameView);
            sw.Write(viewContent);
        }
        using (StreamWriter sw = iMvcView.CreateText())
        {
            iViewContent = iViewContent.Replace(nameof(IMVCViewSample), iNameView);
            iViewContent = iViewContent.Replace(nameof(MVCControllerSample), nameController);
            sw.Write(iViewContent);
        }
    }
}
