using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour, IItem
{
    int score = 100;
    public UnityEvent OnUse { get; set; } = new UnityEvent();
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }


    public void Use(GameObject target)
    {
        gameManager.AddScore(score);
        gameObject.SetActive(false);
        OnUse.Invoke();
    }
}