using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BobController : MonoBehaviour
{
    public float force = 1f;
    
    private Rigidbody2D _rigidbody;
    private float _accruedForce;
    private Random _random;

    private void Start()
    {
        _random = new Random();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnInteract()
    {
        _accruedForce += 1;
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(new Vector2(0, force * _accruedForce));
        _accruedForce = 0;
    }

    public void OnSecondary()
    {
        _accruedForce -= 1;
    }

    public void AddExternalForce(float f)
    {
        _rigidbody.AddForce(new Vector2(_random.Next(101) - 50, f * force));
    }
}
