using UnityEngine;
using UnityEngine.Analytics;

public class Coin : MonoBehaviour, IItem
{
    int score = 100;

    public void Use(GameObject target)
    {
        Debug.Log("Coin");
        Destroy(gameObject);
    }
}