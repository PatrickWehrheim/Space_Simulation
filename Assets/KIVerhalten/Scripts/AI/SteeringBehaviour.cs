using UnityEngine;

public abstract class SteeringBehaviour
{
    public float SteeringStrength { get => _steeringStrength; set => _steeringStrength = value; }
    protected float _steeringStrength; 
    public SteeringBehaviour(float steeringStrength) 
    {
        _steeringStrength = steeringStrength;
    }

    public virtual Vector3 GetSteering(IData data, Vector3 currentPosition, Vector3 targetPosition) { return Vector3.zero; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="maxValue"></param>
    /// <returns>Percantage value</returns>
    protected float GetSteeringStrength(Vector3 targetPosition, Vector3 currentPosition, float maxValue)
    {
        Vector3 distance = targetPosition - currentPosition;
        float magnetude = distance.sqrMagnitude / 100;

        if (magnetude == 0)
        {
            return 0;
        }

        return GetSteeringStrength(magnetude, maxValue);
    }
    private float GetSteeringStrength(float magnetude, float maxValue)
    {
        float steeringstrength = 100 * maxValue / magnetude;
        float result = steeringstrength - 100 < 0 ? 0 : steeringstrength - 100;
        return result;
    }
}
