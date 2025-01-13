
using UnityEngine;
using UnityEngine.VFX;

public class ExplosionModel : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 10);
    }

    public void Explode()
    {
        this.gameObject.GetComponent<VisualEffect>().Play();
    }
}
