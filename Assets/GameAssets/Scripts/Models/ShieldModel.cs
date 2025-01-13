
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class ShieldModel : MonoBehaviour
{
    [SerializeField] private GameObject _shieldRipples;
    [SerializeField] private float _sphereRadius;

    private VisualEffect _shieldRipplesVFX;

    public UnityEvent<int> ShieldHitEvent;

    private void Awake()
    {
        _shieldRipplesVFX = GetComponent<VisualEffect>();
        ShieldHitEvent.AddListener((x) => { return; });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GameObject ripples = Instantiate(_shieldRipples, transform);
            Laser laser = collision.gameObject.GetComponent<Laser>();

            _shieldRipplesVFX = ripples.GetComponent<VisualEffect>();
            _shieldRipplesVFX.SetVector3("SphereCenter", collision.contacts[0].point);
            _shieldRipplesVFX.SetFloat("SphereRadius", _sphereRadius);
            ShieldHitEvent.Invoke(laser.Demage);
            Destroy(ripples, 2);
        }
    }
}
