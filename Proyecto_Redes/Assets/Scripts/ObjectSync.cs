using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Random = UnityEngine.Random;

public class ObjectSync : NetworkBehaviour
{
   [SyncObject] 
   readonly SyncList<int> miArreglo = new SyncList<int>();

   private void Start()
   {
      miArreglo.OnChange += OnMiArregloChange;
   }

   private void Update()
   {
      if(base.IsServer ==false)
         return;
      if (Input.GetKeyDown(KeyCode.A))
      {
        
         miArreglo.Add(Random.Range(1,100));
      }
      
      if (Input.GetKeyDown(KeyCode.S))
      {
         int index = Random.Range(0, miArreglo.Count);
         miArreglo[index]= Random.Range(1,100);
      }
      
      if (Input.GetKeyDown(KeyCode.O))
      {
         int index = Random.Range(0, miArreglo.Count);
         miArreglo.RemoveAt(index);
      }
      
      if (Input.GetKeyDown(KeyCode.C))
      {
         miArreglo.Clear();
      }
      
   }


   void OnMiArregloChange(SyncListOperation op, int index, int before, int next, bool asServer)
   {
      
      Debug.Log($"lista actualizada: {op} - index: {index} - before: {before} - next: {next}");
      switch (op)
      {
         case SyncListOperation.Add:
            break;
         case SyncListOperation.Insert:
            break;
         case SyncListOperation.Set:
            break;
         case SyncListOperation.RemoveAt:
            break;
         case SyncListOperation.Clear:
            break;
         case SyncListOperation.Complete:
            break;
         
      }
   }
}
