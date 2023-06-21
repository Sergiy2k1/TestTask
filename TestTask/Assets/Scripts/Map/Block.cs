using UnityEngine;

public class Block : MonoBehaviour, IDamagable
{
    public virtual void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }
}