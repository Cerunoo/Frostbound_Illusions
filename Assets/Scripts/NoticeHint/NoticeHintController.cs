using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoticeHintController : MonoBehaviour
{
    public static NoticeHintController Instance { get; private set; }

    [SerializeField] private Text contentText;

    [Space(3)]
    [SerializeField] private float showTime;
    private bool isShow;

    private Animator anim;

    private void Awake()
    {
        Instance = this;

        anim = GetComponent<Animator>();
    }

    public void ShowMessage(string content)
    {
        if (isShow) return;

        StartCoroutine(TypeContent(content));
        anim.SetBool("Show", true);
        isShow = true;

        StartCoroutine(HideMessage());
    }

    private IEnumerator TypeContent(string sentence)
    {
        contentText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            contentText.text += letter;
            yield return null;
        }
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(showTime);
        anim.SetBool("Show", false);
        StartCoroutine(WaitDisableIsShow());
    }

    private IEnumerator WaitDisableIsShow()
    {
        yield return new WaitForSeconds(0.5f);
        isShow = false;
    }
}
