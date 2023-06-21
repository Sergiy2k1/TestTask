using UnityEngine;

public class Borders : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile))
        {
            Destroy(projectile.gameObject);
        }
    }
}
