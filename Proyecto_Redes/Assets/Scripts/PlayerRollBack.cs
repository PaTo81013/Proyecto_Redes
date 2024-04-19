using System.Collections;
using System.Collections.Generic;
using FishNet.Component.ColliderRollback;
using FishNet.Managing.Timing;
using FishNet.Object;
using UnityEngine;

public class PlayerRollBack : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (base.IsOwner == false)
            return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = Camera.main.ScreenPointToRay((Input.mousePosition));
            PreciseTick pt = base.TimeManager.GetPreciseTick(base.TimeManager.Tick);
            FireServerRPC(ray.origin,ray.direction,pt);
        }
    }

    [ServerRpc]
    void FireServerRPC(Vector3 position, Vector3 direction, PreciseTick pt)
    {
        base.RollbackManager.Rollback(pt,RollbackPhysicsType.Physics);
        RaycastHit hit;
        if (Physics.Raycast(position,direction,out hit))
        {
            Debug.Log($"Le dio a {hit.collider}");
        }
        else
        {
            Debug.Log($"no le dio");
        }
        base.RollbackManager.Return();
    }
}
