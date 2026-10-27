using UnityEngine;

public class UsableItem : MonoBehaviour, IItem
{
    public Collider collider;
    
    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        collider.enabled = true;
    }
    private void OnDisable()
    {
        collider.enabled = false;
    }

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
