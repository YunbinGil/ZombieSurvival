using System.ComponentModel.Composition.Hosting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score {get; private set;} = 0;
    public UIManager uiManager;
    private void Start()
    {
        Score = 0;
    }

    public void AddScore(int amount) => SetScore(Score + amount);
    private void SetScore(int amount)
    {
        Score = amount;
        uiManager.SetScoreText(Score);
    }
}
