using UnityEngine;

public class Coin : UsableItem
{
    private GameManager gameManager;
    public int score = 10;
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }
    public override void Use(GameObject target)
    {
        gameManager.AddScore(score);
    }

}
