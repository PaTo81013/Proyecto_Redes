using UnityEngine;
using UnityEngine.Events;
public class LocalEventos : MonoBehaviour
{
    public UnityEvent<Transform> OnLocalPlayerSpawn;

    public void EjecutarOnLocalPlayerSpawn(Transform playerTransform)
    {
        Debug.Log($"El jugador spawneo");
        OnLocalPlayerSpawn.Invoke(playerTransform);
    }
}
