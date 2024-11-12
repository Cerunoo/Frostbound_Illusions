using UnityEngine;
using UnityEngine.Events;

public class TriggerActions : MonoBehaviour
{
    [SerializeField, Space(3)] private UnityEvent m_ExecuteEvents = new UnityEvent();

    private void OnTriggerEnter2D()
    {
        m_ExecuteEvents?.Invoke();
    }
}