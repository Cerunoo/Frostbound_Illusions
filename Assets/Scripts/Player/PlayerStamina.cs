using UnityEngine;
using UnityEngine.UI;
using FollusionController;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private ScriptableStats _stats;

    [Header("Debug"), SerializeField] private int staminaValue;

    private void Start()
    {
        staminaSlider.maxValue = _stats.MaxStamina;

        staminaValue = _stats.MaxStamina; // Read saved value
        staminaSlider.value = staminaValue;
    }

    public bool TrySpendStamina()
    {
        if (staminaValue == 0) return false;

        staminaValue--;
        staminaSlider.value = staminaValue;
        return true;
    }
}
