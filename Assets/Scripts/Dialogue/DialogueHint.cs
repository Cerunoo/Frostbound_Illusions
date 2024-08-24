using UnityEngine;

public class DialogueHint : MonoBehaviour
{
    public DialogueManager dm;
    public Dialogue dialogue;

    void OnEnable()
    {
        gameObject.GetComponent<HintControl>().keyPressed += TriggerDialogue;
        gameObject.GetComponent<HintControl>().triggerExit += dm.EndDialogue;
    }

    void OnDisable()
    {
        gameObject.GetComponent<HintControl>().keyPressed -= TriggerDialogue;
        gameObject.GetComponent<HintControl>().triggerExit -= dm.EndDialogue;
    }

    void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
    }
}