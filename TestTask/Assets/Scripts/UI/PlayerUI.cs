using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;

    private Player _player;
    private int _currentHitpoints;
    private const int MAX_HITPOINTS = 3;

    private void Start()
    {
        _currentHitpoints = MAX_HITPOINTS;

        ShowCurrentLivesStatus(MAX_HITPOINTS);

        _player = FindObjectOfType<Player>();
        _player.OnChangeHitpoints += ShowCurrentLivesStatus;
    }

    private void OnDisable()
    {
        _player.OnChangeHitpoints -= ShowCurrentLivesStatus;
    }

    private void ShowCurrentLivesStatus(int hitpoints)
    {
        _currentHitpoints = hitpoints;

        if (_currentHitpoints <= 0)
        {
            _currentHitpoints = 0;
        }

        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < _currentHitpoints)
            {
                _hearts[i].gameObject.SetActive(true);
            }
            else
            {
                _hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
