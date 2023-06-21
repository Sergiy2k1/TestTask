using UnityEngine;

public class FatTank : Enemy
{
    [SerializeField] private Shield _shield;

    private void Start()
    {
        _hitPoints += _hitPoints;
        _scoreForKilled = 200;
        _booster = _shield;
    }
}
