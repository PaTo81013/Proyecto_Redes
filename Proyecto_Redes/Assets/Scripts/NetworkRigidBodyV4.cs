using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class NetworkRigidbodyV4 : NetworkBehaviour
{
    public Vector2 _direction;
    public float _force;
    Rigidbody2D _rigidbody2D;
    Vector2 _startPosition;
    public PredictionManager _predictionManager;

    [SyncVar(SendRate = 1f, OnChange = nameof(OnReconcilationDataChange))]
    private ReconcilationData _reconcilationData;

    void OnReconcilationDataChange(ReconcilationData before,ReconcilationData next, bool asServer)
    {
        if (base.IsServer)
            return;
        float passedTime = (float)base.TimeManager.TimePassed(next.tick);
        float _stepInterval = Time.deltaTime;
        int _Steps = (int)(passedTime / _stepInterval);

        _rigidbody2D.position = next._position;
        (Vector2 _finalPosition, Vector2 _velocity) = _predictionManager.Predict(gameObject, next._velocity, _Steps);
        _rigidbody2D.position = _finalPosition;
        _rigidbody2D.velocity = _velocity;
    }
    
    
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
        
        
        
        //actualiza reconcilation
        _reconcilationData = new ReconcilationData()
        {
            _position = _rigidbody2D.position,
            _velocity = _rigidbody2D.velocity,
            tick = base.TimeManager.Tick
        };
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
    
    [System.Serializable]
    public class ReconcilationData
    {
        public Vector2 _position;
        public Vector2 _velocity;
        public uint tick;
    }
}