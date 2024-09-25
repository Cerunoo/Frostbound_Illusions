using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] private DialogueManager dm;
    [SerializeField] private InteractionButton button;
    [SerializeField] private Dialogue dialogue;

    private void OnEnable()
    {
        button.btnPress += TriggerDialogue;
        button.btnExit += dm.EndDialogue;
    }

    private void OnDisable()
    {
        button.btnPress -= TriggerDialogue;
        button.btnExit -= dm.EndDialogue;
    }

    private void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
    }
}