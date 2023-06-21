using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;
    public int ScoreCount { get; private set; }

    private void Start()
    {
        SetScore(ScoreCount);
    }

    public void SetScore(int score)
    {
        ScoreCount += score;
        _score.text = ScoreCount.ToString();
    }
}
