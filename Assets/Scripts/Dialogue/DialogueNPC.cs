using System;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueManager dm;
    public InteractionButton button;
    [SerializeField] private Dialogue dialogue;

    public void TriggerDialogueOver()
    {
        OnDisable();
        DialogueOver?.Invoke();
    }
    public event Action DialogueOver;

    private void OnEnable()
    {
        button.btnPress += TriggerDialogue;
    }

    private void OnDisable()
    {
        button.btnPress -= TriggerDialogue;
    }

    private void TriggerDialogue() => dm.StartDialogue(dialogue, this);
}