using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

//TODO: Рефактор, много обязанностей в классе
public class Enemy : MonoBehaviour, IDamagable
{
    public event Action<int> OnTakeDamage;
    public event Action<GameObject> OnDestroyed;
    public event Action<int> OnScoreChanged;

    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioClip _takeDamageSound;
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] protected float _speed;

    private AudioSource _audioSource;
    private ScoreUI _score;
    protected Booster _booster;
    protected GameObject _target;
    protected GameObject _base;
    protected GameObject _player;
    protected Vector3[] _previousPositions = new Vector3[MOVEMENT_FRAMES];
    protected int _hitPoints = 1;
    private const int MOVEMENT_FRAMES = 3;
    private const float MINIMUM_STANDING_DISTANCE = 1f, RELOAD_TIME = 1f, SHOOTING_RATE = 3f, DELAY_BETWEEN_FINDING_PLAYER = 1f;
    private const float CORRECTION_ANGLE = 90f;
    private bool _isCanMove = true, _isCanFire = true;
    protected int _scoreForKilled;


    private void OnEnable()
    {
        SetStartPositions();

        _score = FindObjectOfType<ScoreUI>();
        _audioSource = GetComponent<AudioSource>();

        OnScoreChanged += _score.SetScore;
        OnDestroyed += DropBooster;

        StartCoroutine(FindingPlayerTank());
        StartCoroutine(ObstaclesAvailabilityController());
        StartCoroutine(Shooting());
    }

    private void OnDisable()
    {
        OnScoreChanged -= _score.SetScore;
        OnDestroyed -= DropBooster;
    }

    private void FixedUpdate()
    {
        if (_isCanMove && _target != null)
            Movement();
    }

    private void SetStartPositions()
    {
        for (int i = 0; i < _previousPositions.Length; i++)
        {
            _previousPositions[i] = Vector3.zero;
        }
    }

    public void SetBase(GameObject basePosition)
    {
        _base = basePosition;
        SetCurrentTarget(_base);
    }

    private void SetCurrentTarget(GameObject target)
    {
        _target = target;
    }

    private void Movement()
    {
        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - CORRECTION_ANGLE, Vector3.forward);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.fixedDeltaTime);
    }

    private IEnumerator ObstaclesAvailabilityController()
    {
        yield return new WaitForSeconds(3);

        _isCanMove = true;

        yield return new WaitForSeconds(1);

        SetPreviouslyPositions();
        CheckIsCanMoving();

        StartCoroutine(ObstaclesAvailabilityController());
    }

    private void SetPreviouslyPositions()
    {
        for (int i = 0; i < _previousPositions.Length - 1; i++)
        {
            _previousPositions[i] = _previousPositions[i + 1];
        }
        _previousPositions[_previousPositions.Length - 1] = transform.position;
    }

    private void CheckIsCanMoving()
    {
        for (int i = 0; i < _previousPositions.Length - 1; i++)
        {
            if (Vector3.Distance(_previousPositions[i], _previousPositions[i + 1]) >= MINIMUM_STANDING_DISTANCE)
            {
                _isCanMove = true;
                Shoot();
                break;
            }
            else
            {
                _isCanMove = false;
            }
        }
    }

    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(SHOOTING_RATE);

        Shoot();

        StartCoroutine(Shooting());
    }

    private void Shoot()
    {
        if (!_isCanFire)
        {
            return;
        }

        GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(transform.rotation.z, transform.forward);
        projectile.GetComponent<Projectile>().SetupDirection(transform.up);
        _audioSource.PlayOneShot(_shootingSound);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(RELOAD_TIME);
        _isCanFire = true;
    }

    public void TakeDamage(int damage)
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0)
        {
            _hitPoints = 0;
            _audioSource.PlayOneShot(_takeDamageSound);
            OnScoreChanged?.Invoke(_scoreForKilled);
            OnDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator FindingPlayerTank()
    {
        yield return new WaitForSeconds(DELAY_BETWEEN_FINDING_PLAYER);

        bool isFindedPlayer = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        foreach (var item in colliders)
        {
            if (item.TryGetComponent(out Player player))
            {
                isFindedPlayer = true;
                _player = player.gameObject;
                SetCurrentTarget(_player);
            }
        }

        if (!isFindedPlayer)
        {
            _target = _base;
        }

        StartCoroutine(FindingPlayerTank());
    }

    private void DropBooster(GameObject gameObject)
    {
        float random = Random.Range(0f, 1f);

        if (random >= 0.1f)
        {
            Instantiate(_booster.gameObject, transform.position, Quaternion.identity);
        }
    }
}
