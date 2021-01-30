using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    
    public float runSpeed = 6f;
    
    private Vector3 _playerVelocity;
    private float _jumpHeight = 3.0f;
    private float _gravityValue = -9.81f;
    private bool _isGrounded;
    
    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private  Vector3 _moveDirection = Vector3.zero;
    
    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angel, 0f);
        
            _moveDirection = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;
            
            controller.Move(_moveDirection.normalized * runSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
        }
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }
}
