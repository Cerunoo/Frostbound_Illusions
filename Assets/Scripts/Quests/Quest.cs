using System;
using System.Linq;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] protected DisableWallNPC disableWallNPC;
    protected enum DisableWallNPC { None, AfterGot, AfterPass }

    [SerializeField] protected GameObject[] includeObjects;
    [SerializeField] protected IncludeObjectsAfter includeObjectAfter;
    protected enum IncludeObjectsAfter { None, AfterGot, AfterPass }

    [SerializeField] protected Dialogue passDialogue;
    [SerializeField] protected Dialogue failDialogue;

    protected DialogueNPC npc;
    protected bool questGot;

    public event Action<Quest> QuestGotEvent;
    public event Action<Quest> QuestPassedEvent;

    protected virtual void Got()
    {
        if (questGot) return;
        questGot = true;
        QuestGotEvent?.Invoke(this);
        if (disableWallNPC == DisableWallNPC.AfterGot) GetComponent<Collider2D>().enabled = false;
        if (includeObjectAfter == IncludeObjectsAfter.AfterGot) foreach (GameObject item in includeObjects) item.SetActive(true);
        npc.button.btnPress += TriggerDialogue;
    }
    protected virtual void Pass()
    {
        QuestPassedEvent?.Invoke(this);
        if (disableWallNPC == DisableWallNPC.AfterPass) GetComponent<Collider2D>().enabled = false;
        if (includeObjectAfter == IncludeObjectsAfter.AfterPass) foreach (GameObject item in includeObjects) item.SetActive(true);
        npc.button.btnPress -= TriggerDialogue;
    }
    protected virtual void TriggerDialogue()
    {
        bool passQuest = CheckQuest();
        npc.dm.StartDialogue(passQuest ? passDialogue : failDialogue, npc);
        if (passQuest) Pass();
    }
    protected virtual bool CheckQuest() => true;

    protected virtual void OnEnable()
    {
        npc = GetComponent<DialogueNPC>();
        npc.DialogueOver += Got;

        foreach (GameObject item in includeObjects) item.SetActive(false);
    }
    protected virtual void OnDisable()
    {
        npc.DialogueOver -= Got;
        if (npc.button._btnPress != null && npc.button._btnPress.GetInvocationList().Any(d => d.Method.Name == "TriggerDialogue")) // Пздц
            npc.button.btnPress -= TriggerDialogue;
    }
}