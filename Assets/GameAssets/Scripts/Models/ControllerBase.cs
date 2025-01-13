
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public abstract class ControllerBase : MonoBehaviour, IController, IDamagable, ISelectable
{
    [SerializeField] protected bool _showGizmos = true;
    [SerializeField] protected GameObject _explosionPrefab;

    public IData Data { get; set; }

    public Vector3 Position => transform.position;

    public Vector3 Rotation => transform.eulerAngles;

    public GameObject LaserObject { get; set; }

    protected int _healthPoints;
    public int Health => _healthPoints;

    protected int _shieldPoints;
    public int Shield => _shieldPoints;

    protected int _shieldDemage;
    public int ShieldDemage => _shieldDemage;

    private bool _isExploding = false;

    public SelectedUnitsSettings SelectedUnitsSettings { get; set; }

    protected SpriteRenderer _spriteRenderer;
    protected NavMeshAgent _navMeshAgent;
    protected float _attackRateCount = 0f;

    protected ShieldModel _shieldModel;
    protected VisualEffect _shieldVFX;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public virtual void Attack(Collider targetCollider)
    {
        if (_attackRateCount >= Data.AttackRate)
        {
            GameObject laser = Instantiate(LaserObject);
            laser.GetComponent<Laser>().LayerToHit = Data.TargetLayerMask;
            laser.transform.position = transform.position + transform.lossyScale;
            laser.transform.LookAt(targetCollider.bounds.center);
            _attackRateCount = 0;
        }
        else
        {
            _attackRateCount += Time.deltaTime;
        }
    }

    public virtual void Deselect()
    {
        _spriteRenderer.enabled = false;
    }

    public virtual void Die()
    {
        if (!_isExploding)
        {
            _isExploding = true;
            gameObject.layer = 0;
            Explode();
            Destroy(this.gameObject, 3);
        }
    }

    private void Explode()
    {
        if (_explosionPrefab != null)
        {
            GameObject explosion = Instantiate(_explosionPrefab, this.gameObject.transform.position, Quaternion.identity);
            ExplosionModel explosionModel = explosion.GetComponent<ExplosionModel>();
            explosionModel.Explode();
        }
    }

    public virtual void GetDemage(int demagePoints)
    {
        if (_shieldPoints > 0)
        {
            _shieldPoints -= demagePoints;
            _shieldDemage += demagePoints;
            _shieldVFX?.SetInt("ShieldDemage", _shieldDemage);

            Debug.Log($"ShieldPoints: {_shieldPoints} | ShieldDemage: {_shieldDemage}");

            if (_shieldPoints <= 0)
            {
                _shieldPoints = 0;
                _shieldModel?.gameObject.SetActive(false);
            }
        }
        else
        {
            _healthPoints -= demagePoints;
            if (_healthPoints < 0)
            {
                _healthPoints = 0;
                Die();
            }
        }
    }

    public virtual Vector3 GetNextPatrolPoint()
    {
        if (Data.PatrolPositions.Count > 0)
        {
            return Data.PatrolPositions[0];
        }
        return transform.position;
    }

    public virtual void IsClicked()
    {
        _spriteRenderer.enabled = true;
    }

    public virtual void Move(Vector3 direction)
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.Move(direction);
        }
    }

    public virtual void MoveToPoint()
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.destination = GetNextPatrolPoint();
        }
    }

    public virtual void MoveToPoint(Vector3 position)
    {
        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.destination = position;
        }
    }

    public virtual void OnRightClick(Vector3 point)
    {
        Data.PatrolPositions.Add(point);
    }

    public virtual void Rotate(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos)
        {
            return;
        }

        float targetDetectionRadius = Data.TargetDetectionRadius;
        List<Transform> targetColliders = Data.TargetColliders;
        float rayLength = Data.RayLength;
        float ObstacleDetectionRadius = Data.ObstacleDetectionRadius;
        List<Transform> ObstacleColliders = Data.ObstacleColliders;
        List<Vector3> patrolPositions = Data.PatrolPositions;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRadius);

        if (targetColliders != null)
        {
            Gizmos.color = Color.red;

            foreach (Transform item in targetColliders)
            {
                if (item != null)
                    Gizmos.DrawWireSphere(item.position, targetDetectionRadius);
            }
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ObstacleDetectionRadius);

        if (ObstacleColliders != null)
        {
            Gizmos.color = Color.blue;

            foreach (Transform collider in ObstacleColliders)
            {
                if (collider != null)
                {
                    Gizmos.DrawLine(Position, collider.position);
                }
            }
        }

        Gizmos.color = Color.cyan;
        foreach (Vector3 positions in patrolPositions)
        {
            Gizmos.DrawWireSphere(positions, 20f);
        }
    }
}
