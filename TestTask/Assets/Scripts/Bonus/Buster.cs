using UnityEngine;

public class Booster : MonoBehaviour
{
    protected AudioSource _pickUp;

    protected void OnEnable()
    {
        _pickUp = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }
}