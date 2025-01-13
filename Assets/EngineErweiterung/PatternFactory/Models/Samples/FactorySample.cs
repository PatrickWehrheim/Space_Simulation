
using UnityEditor;

public class FactorySample : IFactorySample
{
    public void Create(IFactoryNameSample sample)
    {
        sample.Create();
    }
}
