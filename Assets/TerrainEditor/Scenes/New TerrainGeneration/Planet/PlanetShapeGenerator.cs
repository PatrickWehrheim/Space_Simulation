using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlanetShapeGenerator : IShapeGenerator
{
    public MinMax ElevationMinMax { get; set; }

    private PlanetShapeSettings _shapeSettings;
    private PlanetSimpleNoiseFilter[] _noiseFilters;

    private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(); // Smarter lock
    //private object _lock = new object();

    public void UpdateShapeSettings(PlanetShapeSettings shapeSettings)
    {
        _shapeSettings = shapeSettings;
        ElevationMinMax = new MinMax();

        _lock.EnterWriteLock();
        try
        {
            _noiseFilters = new PlanetSimpleNoiseFilter[shapeSettings.NoiseLayer.Length];
            for (int i = 0; i < _noiseFilters.Length; i++)
            {
                _noiseFilters[i] = new PlanetSimpleNoiseFilter(shapeSettings.NoiseLayer[i].NoiseSettings);
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public Vector3 TransformCubeToSpherePosition(Vector3 cubeVertPos, bool useFancySphere)
    {
        if (!useFancySphere)
        {
            return cubeVertPos.normalized;
        }

        float x2 = cubeVertPos.x * cubeVertPos.x;
        float y2 = cubeVertPos.y * cubeVertPos.y;
        float z2 = cubeVertPos.z * cubeVertPos.z;

        //fancySphere Algorythmus: https://catlikecoding.com/unity/tutorials/cube-sphere/
        float xPrime = cubeVertPos.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float yPrime = cubeVertPos.y * Mathf.Sqrt(1 - (x2 + z2) / 2 + (x2 * z2) / 3);
        float zPrime = cubeVertPos.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

        return new Vector3(xPrime, yPrime, zPrime);
    }

    public float CalculateUnscaledElevationOnPlanet(Vector3 spherePosition)
    {
        _lock.EnterReadLock();
        try
        {
            float elevation = 0f;
            float firstLayerElevation = 0f;

            if (_noiseFilters.Length > 0)
            {
                firstLayerElevation = _noiseFilters[0].Evaluate(spherePosition);

                if (_shapeSettings.NoiseLayer[0].Enabled)
                {
                    elevation = firstLayerElevation;
                }

                //Start at Index 1, because first layer is already evaluated
                for (int i = 1; i < _noiseFilters.Length; i++)
                {
                    if (_shapeSettings.NoiseLayer[i].Enabled)
                    {
                        float mask = _shapeSettings.NoiseLayer[i].UseFirstLayerAsMask ? firstLayerElevation : 1f;

                        elevation += _noiseFilters[i].Evaluate(spherePosition) * mask;
                    }
                }
                ElevationMinMax.AddValue(elevation);
            }

            return elevation;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public Vector3 CalculateScaledPointOnPlanet(Vector3 planetPos, float unscaledElevation)
    {
        _lock.EnterReadLock();
        try
        {
            return (1 + Mathf.Max(0, unscaledElevation)) * _shapeSettings.PlanetRadius * planetPos;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public  Vector3 TransformPointWithTransformMatrix(Vector3 basePosition)
    {
        Matrix4x4 transformMatrix = GetTransformMatrix();

        Vector4 basePos = new Vector4(basePosition.x, basePosition.y, basePosition.z, 1);
        basePos = transformMatrix * basePos;

        return new Vector3(basePos.x, basePos.y, basePos.z);
    }

    private Matrix4x4 GetTransformMatrix()
    {
        //float xRotation = rotation.x * Mathf.Deg2Rad;

        Matrix4x4 rotationMatrix = new Matrix4x4();
        rotationMatrix.SetRow(0, new Vector4(0, 0, 0, 0));
        rotationMatrix.SetRow(1, new Vector4(0, 0, 0, 0));
        rotationMatrix.SetRow(2, new Vector4(0, 0, 0, 0));
        rotationMatrix.SetRow(3, new Vector4(0, 0, 0, 0));

        return rotationMatrix;
    }
}
