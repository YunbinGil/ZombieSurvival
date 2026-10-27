using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour, IItem
{
    int score = 100;
    public UnityEvent OnUse { get; set; } = new UnityEvent();


    public void Use(GameObject target)
    {
        Debug.Log("Coin");
        gameObject.SetActive(false);
        OnUse.Invoke();
    }
}