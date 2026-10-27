using System.ComponentModel.Composition.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public ZombieSpawner zombieSpawner;
    public ItemSpawner itemSpawner;
    private int score = 0;
    public bool IsGameover{get; private set;}
    private void Start()
    {
        score = 0;
        zombieSpawner.enabled = true;
        itemSpawner.enabled = true;
    }

    public void AddScore(int amount) => SetScore(score + amount);
    private void SetScore(int amount)
    {
        if(IsGameover) return;
        score = amount;
        uiManager.SetScoreText(score);
    }
    public void EndGame()
    {
        IsGameover = true;
        uiManager.SetActiveGameoverUI(true);

        zombieSpawner.OnEndGame();
        zombieSpawner.enabled = false;
        itemSpawner.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
