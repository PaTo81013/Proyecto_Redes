using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class ProyectilJugador : NetworkBehaviour
{
    [SerializeField] float TiempoDeVida = 5f;
    [Server]
    void Start()
    {
        if (base.IsServer == false);
        return;
        
        Invoke(nameof(Destruir),TiempoDeVida);
    }

    
    void Destruir()
    {
        Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().vida--;
        }
    }
}
