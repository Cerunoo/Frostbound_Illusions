using UnityEngine;
using UnityEngine.Events;

public class TriggerActions : MonoBehaviour
{
    [SerializeField, Space(3)] private UnityEvent m_ExecuteEvents = new UnityEvent();
    private bool triggered;

    private void OnTriggerEnter2D()
    {
        if (triggered) return;
        triggered = true;

        m_ExecuteEvents?.Invoke();
    }
}