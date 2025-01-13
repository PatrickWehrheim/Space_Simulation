using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyScript : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;

    public void RecalculateRotation()
    {
        transform.up = transform.position - _targetTransform.position;
    }

    public void RecalculatePosition()
    {
        transform.position = _targetTransform.position + (transform.position - _targetTransform.position).normalized * (25f + transform.lossyScale.y * 0.5f);
    }
}
