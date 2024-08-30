using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] private DialogueManager dm;
    [SerializeField] private HintControl hint;
    [SerializeField] private Dialogue dialogue;

    void OnEnable()
    {
        hint.btnPressed += TriggerDialogue;
        hint.triggerExit += dm.EndDialogue;
    }

    void OnDisable()
    {
        hint.btnPressed -= TriggerDialogue;
        hint.triggerExit -= dm.EndDialogue;
    }

    void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
    }
}