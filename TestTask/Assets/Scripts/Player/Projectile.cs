using UnityEngine;
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private float _speed = 150f;
    private Vector2 _direction;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetupDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rigidBody.velocity = new Vector2(_direction.x * _speed * Time.fixedDeltaTime, _direction.y * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(1);
            Destroy(gameObject);
        }
        if (collision.TryGetComponent(out SteelBlock steelBlock))
        {
            Destroy(gameObject);
        }

    }

    public void IncreaseSpeed()
    {
        _speed += _speed;
    }
}
