using UnityEngine;

public class FastTank : Enemy
{
    [SerializeField] private ShootingBooster _shootingBooster;

    private void Start()
    {
        _speed += _speed;
        _scoreForKilled = 100;
        _booster = _shootingBooster;
    }
}
