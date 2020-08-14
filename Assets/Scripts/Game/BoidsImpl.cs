using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// User Boids implementation
/// It has knowledge of user's game
/// This class implements boids with leader
/// Cohere and Align express bird-like boids
/// 
/// e.g) what kind of components should this have
/// </summary>
public class BoidsImpl : MonoBehaviour, BoidsInterface
{
    public bool isLeader = false;
    public bool followOnlyLeader = true;
    public float noLeaderAttenuation = 0.1f;

    private Follow follow = null;
    private bool followingLeader = false;

    public bool hasLeader
    {
        get {
            return this.followingLeader;
        }
    }

#if BOID_DEBUG
    GameObject debugGameObject = null;
#endif

    private void Start()
    {
        this.follow = this.GetComponent<Follow>();

#if BOID_DEBUG
        this.debugGameObject = new GameObject();
        this.debugGameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        SpriteRenderer r = this.debugGameObject.AddComponent<SpriteRenderer>();
        r.sprite = Resources.Load<Sprite>("Sprites/circle");
        if (this.isLeader)
        {
            r.color = Color.blue;
        }
        else
        {
            r.color = Color.red;

            if (this.gameObject.name == "BoidsEnitty")
            {
                r.color = Color.green;
            }
        }
#endif
    }

    public void Preprocess(ref Vector3 v)
    {
        this.followingLeader = false;
    }

    // Cohesion
    public void Cohere(ref Vector3 v)
    {
        BoidsEntity boidsEntity = this.GetComponent<BoidsEntity>();
        BoidsImpl closestImpl = this.GetClosestBoidImpl();

        if (closestImpl == null)
        {
            return;
        }

        float distance = Vector3.Distance(this.transform.position, closestImpl.transform.position);

        if (distance > boidsEntity.cohesionDistance)
        {
            return;
        }

        if (distance > boidsEntity.alignmentDistance && distance > boidsEntity.separateDistance)
        {
            v += closestImpl.transform.position - this.transform.position;
        }

        if (this.followOnlyLeader && closestImpl != null)
        {
            this.followingLeader = true;
        }
    }

    // Separation
    public void Separate(ref Vector3 v)
    {
        BoidsEntity boidsEntity = this.GetComponent<BoidsEntity>();
        GameObject boidsContainer = boidsEntity.config.GetBoidsContainer();

        BoidsEntity[] entities = boidsContainer.GetComponentsInChildren<BoidsEntity>();
        Vector3 pos = this.transform.position;
        for (int i = 0; i < entities.Length; i++)
        {
            BoidsEntity entity = entities[i];
            // ignore self
            if (GameObject.ReferenceEquals(this.gameObject, entity.gameObject))
            {
                continue;
            }
            float distance = Vector3.Distance(entity.transform.position, pos);
            if (distance < boidsEntity.separateDistance)
            {
                Vector3 separateVector = -(entity.transform.position - pos) * (boidsEntity.separateDistance / distance);
                float vectorDistance = Vector3.Distance(separateVector, Vector3.zero);
                v += separateVector * (boidsEntity.separateDistance / vectorDistance);
            }
        }
    }

    // Alignment
    public void Align(ref Vector3 v)
    {
        Vector3 alignmentVector = Vector3.zero;
        
        BoidsEntity boidsEntity = this.GetComponent<BoidsEntity>();
        GameObject boidsContainer = boidsEntity.config.GetBoidsContainer();

        BoidsEntity[] entities = boidsContainer.GetComponentsInChildren<BoidsEntity>();
        Vector3 pos = this.transform.position;
        int alignmentBoidsCount = 0;
        for (int i = 0; i < entities.Length; i++)
        {
            BoidsEntity entity = entities[i];
            // ignore self
            if (GameObject.ReferenceEquals(this.gameObject, entity.gameObject))
            {
                continue;
            }

            
            float distance = Vector3.Distance(entity.transform.position, pos);
            if (distance >= boidsEntity.alignmentDistance)
            {
                continue;
            }

            Follow follow = entity.GetComponent<Follow>();
            if (!follow.IsFollowing())
            {
                continue;
            }

            Vector3 destination = entity.transform.position;
            destination = follow.GetDestination();
            alignmentVector += destination - entity.transform.position;
            alignmentBoidsCount++;
        }

        if (alignmentBoidsCount > 0)
        {
            alignmentVector /= alignmentBoidsCount;
            v += alignmentVector - v;
        }
    }

    public void Postprocess(ref Vector3 v)
    {
        if (this.followOnlyLeader && !this.followingLeader)
        {
            v *= 1.0f - this.noLeaderAttenuation;
        }
    }

    public void ApplyVector(Vector3 v)
    {
        if (this.follow == null)
        {
            return;
        }

        Vector3 destination = this.transform.position + v;

        if (this.follow.IsFollowing())
        {
            this.follow.ExtendDestination(destination);
        }
        else
        {
            this.follow.SetDestination(destination);
        }

#if BOID_DEBUG
        this.DrawDot(destination);
#endif
    }

#if BOID_DEBUG
    void DrawDot(Vector3 pos)
    {
        SpriteRenderer r = this.debugGameObject.GetComponent<SpriteRenderer>();
        r.transform.position = pos;
    }
#endif

    private BoidsImpl GetClosestBoidImpl(bool findLeaderBoid = false)
    {
        BoidsImpl closestImpl = null;

        BoidsEntity boidsEntity = this.GetComponent<BoidsEntity>();
        GameObject boidsContainer = boidsEntity.config.GetBoidsContainer();
        if (boidsContainer == null)
        {
            return closestImpl;
        }

        float closestDistance = -1.0f;
        // NOTE: Since this is first person boids, every BoidsEntity checks distance for every each other BoidsEntity.
        // To consider performance, this should be done in third person logic.
        BoidsImpl[] impls = boidsContainer.GetComponentsInChildren<BoidsImpl>();
        Vector3 pos = this.transform.position;
        for (int i = 0; i < impls.Length; i++)
        {
            BoidsImpl impl = impls[i];
            if (this.followOnlyLeader && !impl.isLeader)
            {
                continue;
            }
            // ignore self
            if (GameObject.ReferenceEquals(this.gameObject, impl.gameObject))
            {
                continue;
            }
            float distance = Vector3.Distance(impl.transform.position, pos);
            if (closestDistance < 0 || distance < closestDistance)
            {
                closestDistance = distance;
                closestImpl = impl;
            }
        }

        return closestImpl;
    }
}
