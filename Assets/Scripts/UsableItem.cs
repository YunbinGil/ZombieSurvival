using UnityEngine;
using UnityEngine.Events;

public class UsableItem : MonoBehaviour, IItem
{
    public UnityEvent OnMeet;
    public AudioSource audioPlayer;
    public AudioClip pickupClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioPlayer.PlayOneShot(pickupClip);
            Use(other.gameObject);
            gameObject.SetActive(false);
            OnMeet.Invoke();
        }
    }

    public virtual void Use(GameObject target) {}

}
