using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class NetworkRigidbodyV1 : NetworkBehaviour
{
    public Vector2 _direction;
    public float _force;
    Rigidbody2D _rigidbody2D;
    Vector2 _startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (base.IsServer == false)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootRPC();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartRPC();
        }
    }
    
    [ObserversRpc(RunLocally = true)]
    void ShootRPC()
    {
        _rigidbody2D.velocity = _direction * _force;
    }
    
    [ObserversRpc(RunLocally = true)]
    void RestartRPC()
    {
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}
