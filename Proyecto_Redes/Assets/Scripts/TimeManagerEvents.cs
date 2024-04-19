using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerEvents : MonoBehaviour
{
    private Queue<Vector3> HistorialPosiciones = new Queue<Vector3>(10);

    void Start()
    {
        //Si el script es en mono behaviour
        FishNet.InstanceFinder.TimeManager.OnTick += OnTick;

        //si heredamos de network behaviour
        //base.TimeManager
    }
    
    //Ciclo de juego FishNet
    void OnTick()//Antes de preparar el envio de paquetes
    {
        HistorialPosiciones.Enqueue(transform.position);
        if (HistorialPosiciones.Count>10)
        {
            HistorialPosiciones.Dequeue();
        }
    }

    void OnPostTick()//Lo ultimo se llama antes de llamar los paquetes
    {
        
    } 

    void OnPreTick()//Los paquetes ya fueron enviados
    {
        
    }
    
    //Se llama cuando se calcula tu nuevo ping/lag
    //Se puede utilizar para desactivar ciertas funciones o mostrar un UI para indicarle al jugador que tiene una mala conexion
    void OnPostPhysicsSimulation(float dt)
    {
        
    }
    
    //Son eventos utilizados si le pides fishnet que controle el update de las fisicas
    void OnPrePhysicsSimulation(float dt)
    {
        
    }

    void OnRoundTripTimeUpdated(float tripTime)
    {
        
    }
    
    //Se llaman igual que los eventos con su mismo nombre en unity
    void OnUpdate()
    {
        
    }

    void OnFixedUpdate()
    {
        
    }

    void OnLatedUpdated()
    {
        
    }
}
