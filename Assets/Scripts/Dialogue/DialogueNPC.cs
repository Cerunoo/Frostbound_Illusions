using System;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueManager dm;
    public InteractionButton button;
    [SerializeField] private TextAsset dialog;

    public event Action DialogueOver;

    public void TriggerDialogueOver()
    {
        OnDisable();
        DialogueOver?.Invoke();
    }

    private void OnEnable()
    {
        button.btnPress += TriggerDialogue;
    }

    private void OnDisable()
    {
        button.btnPress -= TriggerDialogue;
    }

    private void TriggerDialogue() => dm.StartDialogue(dialog, this);
}