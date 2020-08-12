using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// First person boids algorithm.
/// It handles logics around boids in unity.
/// </summary>
public class BoidsEntity : MonoBehaviour
{
    [SerializeField]
    private BoidsConfig boidsConfig = null;

    public BoidsConfig config {
        get {
            return this.boidsConfig;
        }
        set {
            this.boidsConfig = value;
        }
    }

    public bool apply = true;

    public float cohesionDistance = 1.0f;
    public float alignmentDistance = 0.5f;
    public float separateDistance = 0.25f;

    [SerializeField]
    private GameObject boidsImpl = null;

    private void Update()
    {
        if (this.boidsConfig == null || !this.apply)
        {
            return;
        }

        if (this.boidsImpl == null)
        {
            return;
        }

        // should cache on start ?
        // will this be changed dynamically ?
        BoidsInterface boids = this.boidsImpl.GetComponent<BoidsInterface>();
        if (boids == null)
        {
            return;
        }

        Vector3 v = Vector3.zero;
        boids.Preprocess(ref v);
        boids.Cohere(ref v);
        boids.Separate(ref v);
        boids.Align(ref v);
        boids.Postprocess(ref v);

        boids.ApplyVector(v);
    }
}
