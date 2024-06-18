using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform[] Spawns = new Transform[0];
    public int _nextSpawn;
    public string _SelectedPlayer ;
    public GameObject _Canvas;


    void Start()
    {
        InstanceFinder.ServerManager.RegisterBroadcast<PersonajeSelecionadoPackage>(OnPersonajeSeleccionadoBroadcast);
        InstanceFinder.ClientManager.OnClientConnectionState+= ClientManagerOnOnClientConnectionState;
        
    }

    private void ClientManagerOnOnClientConnectionState(ClientConnectionStateArgs ClientState)
    {
        if (ClientState.ConnectionState== LocalConnectionState.Started)
        {
            PersonajeSeleccionado();
            _Canvas.SetActive(false);

        }
    }

    void OnDestroy()
    {
        InstanceFinder.ServerManager.UnregisterBroadcast<PersonajeSelecionadoPackage>(OnPersonajeSeleccionadoBroadcast);
    }

    // Del lado del cliente llamar esta función, UNA VEZ CONECTADO
    public void PersonajeSeleccionado()
    {
        PersonajeSelecionadoPackage msg = new PersonajeSelecionadoPackage();
        msg.nombrePersonajeSeleccionado = _SelectedPlayer;
       //msg.personajeSeleccionadoIndex = 2;
        //msg.personajeSeleccionado = prefabPersonaje;
        
        InstanceFinder.ClientManager.Broadcast(msg);
    }

    // Esto se ejecuta del lado del servidor
    void OnPersonajeSeleccionadoBroadcast(NetworkConnection conn, PersonajeSelecionadoPackage msg)
    {
        GameObject go;
        // Utilizas msg
        //GameObject go = Instantiate(msg.personajeSeleccionado); // opt1
        //go = Instantiate(personajes[msg.personajeSeleccionadoIndex]); // opt2
        go = Instantiate(Resources.Load<GameObject>(msg.nombrePersonajeSeleccionado)); // opt 3
        
        SetSpawn(go.transform,out Vector3 pos,out Quaternion rot);
        go.transform.position = pos;
        go.transform.rotation = rot;
        InstanceFinder.ServerManager.Spawn(go, conn); // Spawnea este personaje y el que mando el mensaje es el dueño
        InstanceFinder.SceneManager.AddOwnerToDefaultScene(go.GetComponent<NetworkObject>()); 
    }
    
    public struct PersonajeSelecionadoPackage : IBroadcast
    {
        public string usuario;
        public GameObject personajeSeleccionado;  // El prefab debe contener un NetworkObject
        public int personajeSeleccionadoIndex;
        public string nombrePersonajeSeleccionado; // Resources
    }
    private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        //No spawns specified.
        if (Spawns.Length == 0)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
            return;
        }

        Transform result = Spawns[_nextSpawn];
        if (result == null)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
        }
        else
        {
            pos = result.position;
            rot = result.rotation;
        }

        //Increase next spawn and reset if needed.
        _nextSpawn++;
        if (_nextSpawn >= Spawns.Length)
            _nextSpawn = 0;
    }
    private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        pos = prefab.position;
        rot = prefab.rotation;
    }

    public void SetPersonaje(string _namePlayer)
    {
        _SelectedPlayer = _namePlayer;
    }

}