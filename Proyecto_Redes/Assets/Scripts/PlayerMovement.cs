using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public Animator _animator;
    public Rigidbody _Rigidbody;
    public float _jumpForce = 3f;
    void Update()
    {
        if (!base.IsOwner)
            return;

        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        
        //transform.Translate(move, 0, 0);
       
        _Rigidbody.AddForce(Vector3.right * move ,ForceMode.Impulse);
        _animator.SetBool("Walk",true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("Jump", true);
            _Rigidbody.AddForce(Vector3.up * _jumpForce ,ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("Box", true);
        }
    }
    
}