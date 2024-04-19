using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class AnimacionSync : NetworkBehaviour
{
    [SerializeField]  Animator _animator;
    void Update()
    {
        if(base.IsServer == false)
            return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            ReproducirWin(base.TimeManager.Tick);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetBool("Saltar",true); //Network animator no sincroniza con triggers, pero si con bool, float,int
        }
    }
    
    [ObserversRpc(RunLocally = true)]
    void ReproducirWinRPC(uint _serverTick)
    {
        if (base.IsHost)
            return;
        
        float passedTime = (float)base.TimeManager.TimePassed(_serverTick);
        const float duracionWin00 = 3.967f;
        float passedPercent = passedTime * 1f / duracionWin00;
        Debug.Log($"passedTime: {passedTime} = passedPercent: {passedPercent}");
        ReproducirWin();
    }

    void ReproducirWin(float _passedPercent = 0f)
    {
        _animator.CrossFade("WIN00",0.1f,0,_passedPercent);
    }
    
}
