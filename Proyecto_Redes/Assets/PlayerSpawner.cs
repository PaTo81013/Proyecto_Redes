using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] personajes;
    
    void Start()
    {
        InstanceFinder.ServerManager.RegisterBroadcast<PersonajeSelecionadoPackage>(OnPersonajeSeleccionadoBroadcast);
        
    }

    void OnDestroy()
    {
        InstanceFinder.ServerManager.UnregisterBroadcast<PersonajeSelecionadoPackage>(OnPersonajeSeleccionadoBroadcast);
    }

    // Del lado del cliente llamar esta función, UNA VEZ CONECTADO
    public void PersonajeSeleccionado(GameObject prefabPersonaje)
    {
        PersonajeSelecionadoPackage msg = new PersonajeSelecionadoPackage();
        msg.nombrePersonajeSeleccionado = "Mario";
        msg.personajeSeleccionadoIndex = 2;
        msg.personajeSeleccionado = prefabPersonaje;
        
        InstanceFinder.ClientManager.Broadcast(msg);
    }

    // Esto se ejecuta del lado del servidor
    void OnPersonajeSeleccionadoBroadcast(NetworkConnection conn, PersonajeSelecionadoPackage msg)
    {
        // Utilizas msg
        GameObject go = Instantiate(msg.personajeSeleccionado); // opt1
        go = Instantiate(personajes[msg.personajeSeleccionadoIndex]); // opt2
        go = Instantiate(Resources.Load<GameObject>(msg.nombrePersonajeSeleccionado)); // opt 3
        
        
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        InstanceFinder.ServerManager.Spawn(go, conn); // Spawnea este personaje y el que mando el mensaje es el dueño
    }
    
    public struct PersonajeSelecionadoPackage : IBroadcast
    {
        public string usuario;
        public GameObject personajeSeleccionado;  // El prefab debe contener un NetworkObject
        public int personajeSeleccionadoIndex;
        public string nombrePersonajeSeleccionado; // Resources
    }
}