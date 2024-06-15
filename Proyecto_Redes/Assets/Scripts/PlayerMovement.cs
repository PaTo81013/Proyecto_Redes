using FishNet.Object;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public Animator _animator;
    public Rigidbody _Rigidbody;
    public float _jumpForce = 3f;
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarFill; // Referencia al GameObject del sprite de la barra de salud

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (!base.IsOwner)
            return;

        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        _Rigidbody.AddForce(Vector3.right * move, ForceMode.Impulse);
        bool _isMoving = move != 0f;
        _animator.SetBool("Walk", _isMoving);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("Jump", true);
            _Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("Box", true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Actualiza la escala de la barra de salud según la vida actual
            float healthPercentage = currentHealth / maxHealth;
            Vector3 newScale = healthBarFill.transform.localScale;
            newScale.x = healthPercentage;
            healthBarFill.transform.localScale = newScale;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        // Agregar más funcionalidad para la muerte aquí, como reaparecer o desactivar controles
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject != this.gameObject)
        {
            TakeDamage(10f); // Ajusta el valor de daño según sea necesario
        }
    }
}
