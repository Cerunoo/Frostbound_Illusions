using UnityEngine;

public class StaminaSlider : MonoBehaviour
{
    [SerializeField] private StaminaSliderCrystal[] crystals;

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            value = (value > crystals.Length) ? crystals.Length : (value < 0) ? 0 : value;  // Изменяем значение если оно не в диапазоне значений
            _value = value;

            for (int i = 0; i < crystals.Length; i++)
            {
                if (i < value) crystals[i].State = true;
                else crystals[i].State = false;
            }
        }
    }
}