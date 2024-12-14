using UnityEngine;
using UnityEngine.Events;

public class TriggerActions : MonoBehaviour
{
    [SerializeField, Space(3)] private UnityEvent m_ExecuteEvents = new UnityEvent();
    private bool triggered;

    [SerializeField] private bool destroyAfterTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (triggered) return;
            triggered = true;

            m_ExecuteEvents?.Invoke();

            if (destroyAfterTrigger) Destroy(gameObject);
        }
    }
}