using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private int staminaValue = 5;

    private void Start()
    {
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
