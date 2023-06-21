using UnityEngine;
using System;

public class Base : MonoBehaviour, IDamagable
{
    public event Action OnEndGame;

    public void TakeDamage(int damage)
    {
        OnEndGame?.Invoke();
    }
}