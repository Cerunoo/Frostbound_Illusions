using UnityEngine;
using UnityEngine.UI;

public class TurnPaperAssist : MonoBehaviour
{
    [SerializeField] private Sprite page1;
    [SerializeField] private Sprite page2;

    [Space(5)]
    [SerializeField] private Animator hintTextAnim;

    private bool turned;

    public void TurnSpriteInKeyAnim()
    {
        GetComponent<Image>().sprite = turned ? page1 : page2;
        turned = !turned;
    }

    public void HintTextDes()
    {
        hintTextAnim.SetTrigger("Des");
    }
}
