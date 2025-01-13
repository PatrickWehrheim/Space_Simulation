using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float LaserColorGreenChannel { get => _laserColorGreenChannel; set => _laserColorGreenChannel = value; }
    [SerializeField, Range(0, 1)] private float _laserColorGreenChannel;
    
    [SerializeField] private float _moveSpeed = 500f;
    [SerializeField] private GameObject _hitPrefab;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private ParticleSystem[] _particleSystems;
    private TrailRenderer _trailRenderer;

    public LayerMask LayerToHit;
    public int Demage;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();

        ChangeLaserColor(_laserColorGreenChannel);
    }

    private void Start()
    {
        _rigidbody.AddForce(transform.forward * _moveSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyAfterSeconds(2));
    }

    public void ChangeLaserColor(float laserColorGreenChannel)
    {
        _laserColorGreenChannel = laserColorGreenChannel;

        foreach (var system in _particleSystems)
        {
            var systemMain = system.main;
            var startColor = systemMain.startColor;
            startColor.colorMax = new Color(systemMain.startColor.colorMax.r, _laserColorGreenChannel, systemMain.startColor.colorMax.b);
            float greenValue = _laserColorGreenChannel;
            if (greenValue > 0.5f)
                greenValue -= 0.2f;
            else
                greenValue += 0.2f;
            startColor.colorMin = new Color(systemMain.startColor.colorMax.r, greenValue, systemMain.startColor.colorMax.b);
            systemMain.startColor = new ParticleSystem.MinMaxGradient(startColor.colorMin, startColor.colorMax);
        }

        if (_trailRenderer == null) 
            _trailRenderer = GetComponentInChildren<TrailRenderer>();

        float trailGreenValue = _laserColorGreenChannel;
        if (trailGreenValue > 0.5f)
            trailGreenValue -= 0.2f;
        else
            trailGreenValue += 0.2f;
        GradientColorKey[] gradientColors = new GradientColorKey[_trailRenderer.colorGradient.colorKeys.Length];
        GradientAlphaKey[] gradientAlphas = new GradientAlphaKey[_trailRenderer.colorGradient.alphaKeys.Length];
        for (int i = 0; i < _trailRenderer.colorGradient.colorKeys.Length; i++)
        {
            gradientColors[i] = new GradientColorKey(new Color(_trailRenderer.colorGradient.colorKeys[i].color.r,
                trailGreenValue, _trailRenderer.colorGradient.colorKeys[i].color.b), _trailRenderer.colorGradient.colorKeys[i].time);
            if (_laserColorGreenChannel > 0.5f)
                trailGreenValue += 0.2f;
            else
                trailGreenValue -= 0.2f;
        }
        for (int i = 0; i < _trailRenderer.colorGradient.alphaKeys.Length; i++)
            gradientAlphas[i] = _trailRenderer.colorGradient.alphaKeys[i];

        Gradient gradient = new Gradient();
        gradient.SetKeys(gradientColors, gradientAlphas);
        _trailRenderer.colorGradient = gradient;
    }


    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Bitshifting
        // 0b0000_0001   << 3
        // 0b0000_1000

        // 0b0001_0010 
        // 0b0000_1000
        // 0b0000_0000

        if (((1 << other.transform.gameObject.layer) & LayerToHit) != 0)
        {
            _collider.isTrigger = false;

            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, collisionPoint.normalized);

            if(_hitPrefab != null)
            {
                Instantiate(_hitPrefab, collisionPoint, rotation);
            }

            IDamagable damagable = other.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.GetDemage(Demage);
            }
            StartCoroutine(DestroyAfterSeconds(0.1f));
        }
    }
}
