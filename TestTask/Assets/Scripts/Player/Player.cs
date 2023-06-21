using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IDamagable
{
    public event Action<int> OnChangeHitpoints;
    public event Action OnEndGame;
    public event Action OnShooted;

    [SerializeField] private Transform _transform;
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioClip _takeDamageSound;
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _speed;
    [SerializeField] private float _shootingReloadTime;
    private int direction = 0;

    private AudioSource _audioSource;
    private Vector3 rotation;
    private float _angleRotationZ;
    private bool _isCanFire = true, _isIncreasedProjectileSpeed, _isUnbreakable;
    private int _hitPoints = MAX_HITPOINTS;
    private const int MAX_HITPOINTS = 3;

    private void Start()
    {

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MovementInput();
        ShootingInput();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = 1;
            rotation = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = 1;
            rotation = new Vector3(0, 0, 180);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = 1;
            rotation = new Vector3(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = 1;
            rotation = new Vector3(0, 0, -90);
        }
        else
        {
            direction = 0;
        }

        _transform.localRotation = Quaternion.Euler(rotation);
    }

    private void MovementLogic()
    {
        Vector3 movement = new Vector3(0, direction, 0f) * _speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void ShootingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isCanFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(_angleRotationZ, transform.forward);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.SetupDirection(transform.up);

        if (_isIncreasedProjectileSpeed)
        {
            projectileComponent.IncreaseSpeed();
        }

        _audioSource.PlayOneShot(_shootingSound);
        _isCanFire = false;
        OnShooted?.Invoke();
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_shootingReloadTime);
        _isCanFire = true;
    }

    public void TakeDamage(int damage)
    {
        if (_isUnbreakable)
        {
            return;
        }

        _hitPoints -= damage;
        _audioSource.PlayOneShot(_takeDamageSound);
        OnChangeHitpoints?.Invoke(_hitPoints);

        if (_hitPoints <= 0)
        {
            _hitPoints = 0;
            OnEndGame?.Invoke();
            Destroy(gameObject);
        }
    }

    public void TakeHealth()
    {
        _hitPoints++;

        if (_hitPoints >= MAX_HITPOINTS)
        {
            _hitPoints = MAX_HITPOINTS;
        }

        OnChangeHitpoints?.Invoke(_hitPoints);
    }

    public void StartIncreasingProjectileSpeed()
    {
        StartCoroutine(IncreaseProjectileSpeed());
    }

    private IEnumerator IncreaseProjectileSpeed()
    {
        _isIncreasedProjectileSpeed = true;
        float boostedTime = 10f;

        yield return new WaitForSeconds(boostedTime);
        _isIncreasedProjectileSpeed = false;
    }

    public void MakePlayerUnbreakable()
    {
        StartCoroutine(SetArmor());
    }

    private IEnumerator SetArmor()
    {
        _isUnbreakable = true;
        float boostedTime = 10f;

        yield return new WaitForSeconds(boostedTime);
        _isUnbreakable = false;
    }
}
