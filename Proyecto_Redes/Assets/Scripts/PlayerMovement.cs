using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public Rigidbody rb;
    public float jumpForce = 3f;
    public float maxHealth = 100f;

    [SyncVar(OnChange = nameof(VidaActualizada))]
    private float _currentHealth;

    public GameObject healthBarFill;
    public GameObject victoryCanvas; // Canvas de victoria
    public GameObject defeatCanvas; // Canvas de derrota
    public float attackRange = 2f; // Rango del ataque
    public float attackDamage = 10f; // Daño del ataque

    private void Start()
    {
        if (IsClient)
        {
            _currentHealth = maxHealth;
            UpdateHealthBar();
        }

        // Asegúrate de que los Canvas estén desactivados al inicio
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("VictoryCanvas is not assigned in the inspector.");
        }

        if (defeatCanvas != null)
        {
            defeatCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("DefeatCanvas is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 newPosition = rb.position + new Vector3(moveHorizontal, 0, moveVertical);
        rb.MovePosition(newPosition);

        bool isMoving = moveHorizontal != 0f || moveVertical != 0f;
        animator.SetBool("Walk", isMoving);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            animator.SetBool("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Box", true);
            ServerAttack();
        }
        else
        {
            animator.SetBool("Box", false);
        }
    }

    private bool IsGrounded()
    {
        return Mathf.Abs(rb.velocity.y) < 0.01f;
    }

    [ServerRpc]
    private void ServerAttack()
    {
        Debug.Log("Attack initiated by: " + Owner.ClientId);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player") && hitCollider.gameObject != this.gameObject)
            {
                PlayerMovement targetPlayer = hitCollider.gameObject.GetComponent<PlayerMovement>();
                if (targetPlayer != null)
                {
                    targetPlayer.TakeDamage(attackDamage);
                }
            }
        }
    }

    [Server]
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        Debug.Log($"Player {Owner.ClientId} took {damage} damage, current health: {_currentHealth}");

        // Sincronizar la vida con todos los clientes
        UpdateHealthBarClientRpc(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    [ObserversRpc]
    private void UpdateHealthBarClientRpc(float health)
    {
        _currentHealth = health;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercentage = _currentHealth / maxHealth;
            Vector3 newScale = healthBarFill.transform.localScale;
            newScale.x = healthPercentage;
            healthBarFill.transform.localScale = newScale;
            Debug.Log($"Updated health bar: {healthPercentage * 100}%");
        }
    }

    private void Die()
    {
        Debug.Log($"Player {Owner.ClientId} Died");
        ActivateDefeatCanvasClientRpc();
    }

    private void Win()
    {
        Debug.Log($"Player {Owner.ClientId} Won");
        ActivateVictoryCanvasClientRpc();
    }

    [ObserversRpc]
    private void ActivateDefeatCanvasClientRpc()
    {
        if (defeatCanvas != null)
        {
            Debug.Log("Activating defeat canvas");
            defeatCanvas.SetActive(true); // Activar Canvas de derrota
        }
        else
        {
            Debug.LogError("DefeatCanvas is not assigned in the inspector.");
        }
    }

    [ObserversRpc]
    private void ActivateVictoryCanvasClientRpc()
    {
        if (victoryCanvas != null)
        {
            Debug.Log("Activating victory canvas");
            victoryCanvas.SetActive(true); // Activar Canvas de victoria
        }
        else
        {
            Debug.LogError("VictoryCanvas is not assigned in the inspector.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject != this.gameObject)
        {
            if (IsServer)
            {
                PlayerMovement targetPlayer = collision.gameObject.GetComponent<PlayerMovement>();
                if (targetPlayer != null)
                {
                    targetPlayer.TakeDamage(10f);
                }
            }
        }
    }

    private void VidaActualizada(float prev, float vidaActual, bool asServer)
    {
        // Aquí actualiza la vida
        Debug.Log($"Vida actualizada de {prev} a {vidaActual}");
        UpdateHealthBar();
    }

    // Lógica para detectar la victoria (depende de tu juego)
    private void CheckForVictory()
    {
        // Aquí debes agregar la lógica para detectar cuando el jugador ha ganado.
        // Por ejemplo, si el jugador recoge todos los objetos, elimina todos los enemigos, etc.
        bool hasWon = false; // Cambia esto según tu lógica

        if (hasWon)
        {
            Win();
        }
    }
}
