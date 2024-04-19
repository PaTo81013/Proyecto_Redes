using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public EventReference sfxEventReference; // Asigna el evento de SFX en el Inspector de Unity

    private FMOD.Studio.EventInstance sfxEventInstance;
    private Transform listenerPosition; // La posición del oyente, usualmente la cámara o el jugador

    void Start()
    {
        // Crear una instancia del evento de SFX
        sfxEventInstance = RuntimeManager.CreateInstance(sfxEventReference);

        // Suponiendo que la cámara es el oyente
        listenerPosition = Camera.main.transform;

        // Establecer los atributos 3D del evento
        RuntimeManager.AttachInstanceToGameObject(sfxEventInstance, listenerPosition, GetComponent<Rigidbody>());

        sfxEventInstance.start(); // Iniciar el evento si no está configurado para iniciarse automáticamente
    }

    // Llamar a esta función desde tu slider de SFX
    public void SetSFXVolume(float volume)
    {
        Debug.Log("Ajustando el volumen del SFX a: " + volume);
        sfxEventInstance.setParameterByName("Volume", volume);
    }


    void OnDestroy()
    {
        // Asegúrate de detener y liberar la instancia del evento al destruir
        sfxEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        sfxEventInstance.release();
    }
}