using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsConfig : MonoBehaviour
{
    [SerializeField]
    private GameObject boidsContainer = null;

    public GameObject GetBoidsContainer()
    {
        return this.boidsContainer;
    }
}
