using UnityEngine;

public class Heart : Booster
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _pickUp.Play();
            Invoke("DestroyObject", _pickUp.clip.length);
            player.TakeHealth();
        }
    }
}
