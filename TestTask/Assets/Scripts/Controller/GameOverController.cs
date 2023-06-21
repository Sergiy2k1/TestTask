using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private Base _base;
    [SerializeField] private ScoreUI _scoreUI;
    [SerializeField] private TextMeshProUGUI _summaryScore;

    private Player _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.OnEndGame += SetActiveGameOverMenu;
        _base.OnEndGame += SetActiveGameOverMenu;
    }

    private void OnDisable()
    {
        _player.OnEndGame -= SetActiveGameOverMenu;
        _base.OnEndGame -= SetActiveGameOverMenu;
    }

    public void SetActiveGameOverMenu()
    {
        Time.timeScale = 0;
        _gameOverMenu.SetActive(true);
        _summaryScore.text = _scoreUI.ScoreCount.ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
