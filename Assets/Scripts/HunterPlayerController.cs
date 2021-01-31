using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HunterPlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask groundLayer;


    public float runSpeed = 6f;
    
    private Vector3 _playerVelocity;
    private float _jumpHeight = 3.0f;
    private float _gravityValue = -9.81f;
    private bool _isGrounded;

    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private  Vector3 _moveDirection = Vector3.zero;
    private Vector3 _direction;
    private Transform _cam;

    private GameObject[] _hidingPlayers;
    private GameObject _ray;
    
    
    private float _skill_1_Time = 0f;
    private float _shootTimer = 0f;
    
    public void Awake()
    {
        this.tag = "HunterPlayer";
    }


    public void Start()
    {
        _cam = Camera.main.transform;
    }


    private void Update()
    {
        Skill_1();
        Shoot();

        //Проверяем, можем ли мы прыгнуть
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (_direction.sqrMagnitude == 0)
        {
            _anim.SetInteger("AnimState",0);
        }

        //Если мы двигаемся
        if (_direction.magnitude >= 0.1f)
        {
            _anim.SetInteger("AnimState",2);
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
        if(!Input.GetKey(KeyCode.F))
            return;
        if (!(Time.time > _skill_1_Time))
            return;

        _hidingPlayers = GameObject.FindGameObjectsWithTag("HidingPlayer");
        foreach (var pl in _hidingPlayers)
        {
           pl.GetComponent<HidingPlayerController>().ShowParticleEffect();
           StartCoroutine(HideParticleEffect());
        }
        _skill_1_Time = Time.time + 20f;
    }

    IEnumerator HideParticleEffect()
    {
        yield return new WaitForSeconds(10f);
        foreach (var pl in _hidingPlayers)
        {
            pl.GetComponent<HidingPlayerController>().HideParticleEffect();
        }
    }

    private void Shoot()
    {
        if (!(Time.time >= _shootTimer)) return;
        if (!Input.GetButtonDown("Fire1")) return;

        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        
        if (Physics.Raycast(ray, out var hit, 100f))
        {
            _anim.SetTrigger("Attack");
            StartCoroutine(WhaitForShoot(hit));

        }
        _shootTimer = Time.time + 0.5f;
    }

    IEnumerator WhaitForShoot(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.35f);
        _ray =
            Instantiate(bullet, startPoint.position, Quaternion.identity);
        
        StartCoroutine(Destroy());
        LineRenderer lineRenderer = _ray.GetComponent<LineRenderer>();
        lineRenderer.SetPosition (0, startPoint.transform.position);
        lineRenderer.SetPosition (1, hit.point);
            
        if(hit.transform.GetComponent<HidingPlayerController>() != null)
            hit.transform.GetComponent<HidingPlayerController>().GetDamaged(damage);
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(_ray);
    }
}
