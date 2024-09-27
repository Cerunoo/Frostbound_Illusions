using UnityEngine;

public class StaminaSliderCrystal : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private bool state;
    public bool State
    {
        get => state;
        set
        {
            state = value;
            anim.SetBool("Filled", value);
        }
    }
}
