using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class NetworkRigidbodyV2 : NetworkBehaviour
{
    public Vector2 _direction;
    public float _force;
    Rigidbody2D _rigidbody2D;
    Vector2 _startPosition;
    public PredictionManager _predictionManager;
    
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
            ShootRPC(base.TimeManager.Tick);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartRPC();
        }
    }
    
    [ObserversRpc(RunLocally = true)]
    void ShootRPC(uint _serverTick)
    {
        float passedTime = (float)base.TimeManager.TimePassed(_serverTick);
        float _stepInterval = Time.deltaTime;
        int _Steps = (int)(passedTime / _stepInterval);

        (Vector2 _finalPosition, Vector2 _velocity) = _predictionManager.Predict(gameObject, _force * _direction, _Steps);
        _rigidbody2D.position = _finalPosition;
        _rigidbody2D.velocity = _velocity;







    }
    
    [ObserversRpc(RunLocally = true)]
    void RestartRPC()
    {
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}