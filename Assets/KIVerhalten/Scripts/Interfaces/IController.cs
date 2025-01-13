
using UnityEngine;

public interface IController
{
    public IData Data { get; set; }
    public Vector3 Position { get; }
    public Vector3 Rotation { get; }
    public SelectedUnitsSettings SelectedUnitsSettings { get; set; }
    public GameObject LaserObject { get; set; }
    public void Move(Vector3 direction);
    public void MoveToPoint();
    public void MoveToPoint(Vector3 position);
    public Vector3 GetNextPatrolPoint();
    public void Rotate(Quaternion rotation);
    public void Attack(Collider targetCollider);
}
