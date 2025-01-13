

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject FirePoint;
    public List<GameObject> Vfx = new List<GameObject> ();

    private GameObject _effectToSpawn;

    void Start()
    {
        _effectToSpawn = Vfx[0];
    }

    public void OnLeftMouseButtonDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnVFX();
        }
    }

    private void SpawnVFX()
    {
        GameObject vfx;

        if (FirePoint != null)
        {
            vfx = Instantiate(_effectToSpawn, FirePoint.transform.position, Quaternion.identity);
            Laser laser = vfx.GetComponent<Laser>();
            laser.LayerToHit = GameManager.Instance.ObstacleLayer;
            laser.Demage = 100;
        }
        else
        {
            Debug.Log("No Fire Point!");
        }
    }
}
