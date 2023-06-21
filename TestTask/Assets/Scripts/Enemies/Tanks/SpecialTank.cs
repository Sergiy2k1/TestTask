using Unity.VisualScripting;
using UnityEngine;

public class SpecialTank : Enemy
{
    [SerializeField] private Heart _heart;

    private void Start()
    {
        _scoreForKilled = 300;
        _booster = _heart;
    }
}