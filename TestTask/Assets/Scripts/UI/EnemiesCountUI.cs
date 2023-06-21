using UnityEngine;

public class EnemiesCountUI : MonoBehaviour
{
    [SerializeField] GameObject[] _activeEnemies;

    private int _countActiveEnemies;

    private void Start()
    {
        _countActiveEnemies = 0;
        ShowCurrentActiveEnemies();
    }

    public void SetActiveEnemy()
    {
        _countActiveEnemies++;
        ShowCurrentActiveEnemies();
    }

    public void DeleteActiveEnemy()
    {
        _countActiveEnemies--;
        ShowCurrentActiveEnemies();
    }

    private void ShowCurrentActiveEnemies()
    {
        for (int i = 0; i < _activeEnemies.Length; i++)
        {
            if (i >= _countActiveEnemies)
            {
                _activeEnemies[i].gameObject.SetActive(false);
            }
            else
            {
                _activeEnemies[i].gameObject.SetActive(true);
            }
        }
    }
}
