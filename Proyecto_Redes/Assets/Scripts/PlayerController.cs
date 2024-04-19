using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]  float velocidad;
    Renderer _renderer;
    
    [SyncVar(OnChange = nameof(MicolorActualizado))]
    private Color _miColor;

    [FormerlySerializedAs("_proyectilPrefab")]
    [Header("Proyectil")] 
    [SerializeField] private GameObject proyectilPrefab;
    [FormerlySerializedAs("_proyectilVelocidad")] [SerializeField] private Vector3 proyectilVelocidad;
    [FormerlySerializedAs("_proyectilSpawnPoint")] [SerializeField] private Transform proyectilSpawnPoint;

    [Header("Vida")]
    [SerializeField]  int vidaMaxima;
    [SyncVar(OnChange = nameof(VidaActualizada))]
    public int vida = 100;
    [SerializeField] private Transform BarraVerde;

    void VidaActualizada(int prev, int vidaActual, bool asServer)
    {
        Debug.Log($"{name}sufrio daño y su nueva vida es: {vidaActual}");
        BarraVerde.localScale = new Vector3(vidaActual / (float) vidaMaxima, 1f, 1f);
    }
    void MicolorActualizado(Color colorviejo, Color nuevoColor, bool asServer)
    {
        _renderer.material.color = nuevoColor;
    }
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.Owner.IsLocalClient)
        {
            _renderer.material.color = Color.green;
            name += "-local " ;
            GameObject.Find("LocalEventos").GetComponent<LocalEventos>().EjecutarOnLocalPlayerSpawn(transform);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (base.IsOwner == false)
            return;
        if (Input.GetKeyDown(KeyCode.C))
        {
            Color newColor = Random.ColorHSV();
            CambiarColorServidorRPC(newColor);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DispararServerRPC();
        }
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.y = Input.GetAxis("Vertical");
        transform.Translate(inputDirection * velocidad * Time.deltaTime);
    }

    [ServerRpc]
    void DispararServerRPC()
    {
        GameObject nuevoProyectil = Instantiate(proyectilPrefab, proyectilSpawnPoint.position,proyectilSpawnPoint.rotation);
        nuevoProyectil.GetComponent<Rigidbody>().velocity = proyectilSpawnPoint.forward = proyectilVelocidad;
            
        FishNet.InstanceFinder.ServerManager.Spawn(nuevoProyectil);
    }

    [ServerRpc(RequireOwnership = false)]
    void CambiarColorServidorRPC(Color color)
    {
        _miColor = color;
        //CambiarColorRPC(_color);
    }

    [ObserversRpc]
    void CambiarColorRPC (Color color )
    {
        _renderer.material.color = color;
    }
    
}
