using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class HidingPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject particleSystemGameObject;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float health = 100f;
    
    
    private Transform _groundCheck;
    
    public float runSpeed = 6f;

    private GameObject _newModel;
    private Vector3 _playerVelocity;
    private float _jumpHeight = 3.0f;
    private float _gravityValue = -9.81f;
    private bool _isGrounded;
    private bool _isParticlePlay = false;
    private bool _isTimerWorking = false;
    private Transform _cam;
    
    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private  Vector3 _moveDirection = Vector3.zero;
    private Vector3 _direction;
    
    private float _skill_1_Time = 0f;
    private float _skill_2_Time = 0f;
    private float _timeLeft = 10f;

    public void Awake()
    {
        this.tag = "HidingPlayer";
    }

    public void Start()
    {
        _cam = Camera.main.transform;
        GetComponent(playerModel);
    }

    private void GetComponent(GameObject obj)
    {
        controller.height = obj.GetComponent<ObjectScript>().Settings.height;
        controller.radius = obj.GetComponent<ObjectScript>().Settings.radius;
        _groundCheck = obj.transform.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        //Проверяем, можем ли мы прыгнуть
        _isGrounded = Physics.CheckSphere(_groundCheck.position, groundDistance, groundLayer);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        Skill_1();
        Skill_2();
        ReturnToPlayerModel();
        
        if (_isTimerWorking)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                _isTimerWorking = false;
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Если мы не двигаемся
        if (_direction.sqrMagnitude == 0)
        {
            _isParticlePlay = false;
            particleSystem.Stop();
        }

        //Если мы двигаемся
        if (_direction.magnitude >= 0.1f)
        {
            if (!_isTimerWorking && !_isParticlePlay)
            {
                particleSystem.Play();
                _isParticlePlay = true;
            }
            
            Move();
        }
        
        Jump();
    }

    private void Move()
    {
        float targetAngel = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
        float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angel, 0f);
        
        _moveDirection = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;
            
        controller.Move(_moveDirection.normalized * runSpeed * Time.deltaTime);
    }
    
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
        }
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void Skill_1()
    {
        if (!Input.GetKey(KeyCode.E)) return;
        if (!(Time.time > _skill_1_Time))
        {
            Debug.Log("Skill is not ready!");
            return;
        }
        particleSystem.Stop();
        _isTimerWorking = true;
        _isParticlePlay = false;
        _skill_1_Time = Time.time + 20f;
    }

    private void Skill_2()
    {
        if (!(Time.time > _skill_2_Time))
        {
            return;
        }
        if (!Input.GetKey(KeyCode.F)) return;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray " + hit.transform.name);
            if (hit.transform.CompareTag("Object"))
                ChangePlayerModel(hit.transform);
        }
    }

    private void ChangePlayerModel(Transform hit)
    {
        _skill_2_Time = Time.time + 20f;
        playerModel.SetActive(false);
        var name = Regex.Replace(hit.name, @"(?<![a-zA-Z])[^a-zA-Z]|[^a-zA-Z](?![a-zA-Z])", String.Empty);
        _newModel = (GameObject) Instantiate(Resources.Load("Models/" + name), 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            transform.rotation, transform);
        _newModel.GetComponent<Rigidbody>().detectCollisions = false;
        _newModel.GetComponent<Rigidbody>().useGravity = false;
        
        GetComponent(_newModel);
    }

    private void ReturnToPlayerModel()
    {
        if(!Input.GetKey(KeyCode.Q))
            return;
        if (playerModel.activeSelf)
            return;
        GetComponent(playerModel);
        playerModel.SetActive(true);
        Destroy(_newModel);
    }

    public void GetDamaged(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowParticleEffect()
    {
        particleSystemGameObject.GetComponent<ParticleSystemRenderer>().enabled = true;
    }

    public void HideParticleEffect()
    {
        particleSystemGameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
    }
}
