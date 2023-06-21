using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        DestroyAllBlocksInsideSpawnZone();

        Invoke("DestroyAllBlocksInsideSpawnZone", 0.01f);
    }

    private void DestroyAllBlocksInsideSpawnZone()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, _sprite.bounds.size, 0f);

        foreach (var item in colliders)
        {
            if (item.TryGetComponent(out Block block))
            {
                Destroy(item.gameObject);
            }
        }
    }
}
