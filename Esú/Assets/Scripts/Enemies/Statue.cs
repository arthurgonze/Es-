using UnityEngine;
using System.Collections;
using System;

public class Statue : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1.265f;
    
    private GameObject hearing;

    private void Start()
    {
        GetComponent<FOV>().SetRotational(true, rotationSpeed);
    }
}