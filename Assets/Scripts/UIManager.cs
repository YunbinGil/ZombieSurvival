using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text waveInfoText;
    public Text scoreText;
    public GameObject gameOverUI;
    private void OnEnable()
    {
        SetActiveGameoverUI(false);
    }
    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text= $"{magAmmo} / {remainAmmo}";
    }

    public void SetWaveInfoText(int wave, int enemyLeft)
    {
        waveInfoText.text= $"WAVE : {wave}\nENEMY LEFT : {enemyLeft}";
    }
    public void SetScoreText(int score)
    {
        scoreText.text = $"SCORE : {score}";
    } 
    public void SetActiveGameoverUI(bool active) => gameOverUI.SetActive(active);
  
}
