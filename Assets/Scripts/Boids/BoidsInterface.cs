using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for boids algorithm
/// </summary>
public interface BoidsInterface
{
    void Preprocess(ref Vector3 v);

    /// <summary>
    /// Cohesion
    //
    /// Search other boids entity if not joined.
    /// Other entity is detected within user defined range.
    /// If any entity was found, it moves towards the closest entity.
    /// When the entity moved close enough to the other entity, it begins alignment.
    /// </summary>
    void Cohere(ref Vector3 v);

    /// <summary>
    /// Separation
    /// 
    /// avoid collision if joined
    /// </summary>
    void Separate(ref Vector3 v);

    /// <summary>
    /// Alignment
    ///
    /// align if joined
    /// </summary>
    void Align(ref Vector3 v);

    void Postprocess(ref Vector3 v);

    /// <summary>
    /// Apply given vector to boid itself
    /// </summary>
    /// <param name="v">Appliying vector</param>
    void ApplyVector(Vector3 v);
}
