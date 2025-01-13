
using UnityEngine;

public class SingletonSample : MonoBehaviour
{
    public static SingletonSample Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
