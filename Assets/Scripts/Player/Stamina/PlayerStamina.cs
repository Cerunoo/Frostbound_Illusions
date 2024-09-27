using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private StaminaSlider slider;
    [SerializeField] private ScriptableStats _stats;

    [Header("Debug")]
    [SerializeField] private int value;
    [SerializeField] private bool infinityStamina;

    private void Start()
    {
        int readValue = 5;  // Read saved value
        value = readValue;
        slider.Value = value;
    }

    public bool TrySpendStamina()
    {
        if (value <= 0 && !infinityStamina) return false;

        value--;
        slider.Value = value;
        return true;
    }
}
