using UnityEngine;

public class UsableItem : MonoBehaviour, IItem
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
            gameObject.SetActive(false);
        }
    }

    public virtual void Use(GameObject target) {}

}
