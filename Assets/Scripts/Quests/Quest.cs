using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    [SerializeField] protected TextAsset passDialog;
    [SerializeField] protected TextAsset failDialog;
    [SerializeField] protected TextAsset afterwordDialog;
    [Space(5)]

    [SerializeField] protected DisableWallNPC disableWallNPC;
    protected enum DisableWallNPC { None, AfterGot, AfterPass }
    [Space(5)]

    [SerializeField] private IncludeObjectsController[] includeObjectsControllers;
    [Space(5)]

    [SerializeField] protected ExecuteEventsAfter executeEventsAfter;
    protected enum ExecuteEventsAfter { None, AfterGot, AfterPass }
    [SerializeField, Space(3)] private UnityEvent m_ExecuteEvents = new UnityEvent();

    protected DialogueNPC npc;
    protected bool questGot;
    protected bool questPass;

    public event Action<Quest> QuestGotEvent;
    public event Action<Quest> QuestPassedEvent;

    protected virtual void Got()
    {
        if (questGot) return;
        questGot = true;
        QuestGotEvent?.Invoke(this);
        if (disableWallNPC == DisableWallNPC.AfterGot) GetComponent<Collider2D>().enabled = false;
        foreach (IncludeObjectsController item in includeObjectsControllers) item.TryInclude(IncludeObjectsController.IncludeObjectsAfter.AfterGot);
        if (executeEventsAfter == ExecuteEventsAfter.AfterGot) m_ExecuteEvents?.Invoke();
        npc.button.btnPress += TriggerDialogue;
    }
    protected virtual void Pass()
    {
        if (questPass) return;
        questPass = true;
        QuestPassedEvent?.Invoke(this);
        if (disableWallNPC == DisableWallNPC.AfterPass) GetComponent<Collider2D>().enabled = false;
        foreach (IncludeObjectsController item in includeObjectsControllers) item.TryInclude(IncludeObjectsController.IncludeObjectsAfter.AfterPass);
        if (executeEventsAfter == ExecuteEventsAfter.AfterPass) m_ExecuteEvents?.Invoke();
        if (afterwordDialog == null) npc.button.btnPress -= TriggerDialogue;
    }
    protected virtual void TriggerDialogue()
    {
        if (questPass)
        {
            if (afterwordDialog != null) npc.dm.StartDialogue(afterwordDialog, npc);
            npc.button.btnPress -= TriggerDialogue;
            return;
        }

        bool passQuest = CheckQuest();
        npc.dm.StartDialogue(passQuest ? passDialog : failDialog, npc);
        if (passQuest) Pass();
    }
    protected virtual bool CheckQuest() => true;

    protected virtual void OnEnable()
    {
        npc = GetComponent<DialogueNPC>();
        npc.DialogueOver += Got;

        foreach (IncludeObjectsController item in includeObjectsControllers) item.Exclude();
    }
    protected virtual void OnDisable()
    {
        npc.DialogueOver -= Got;
        if (npc.button._btnPress != null && npc.button._btnPress.GetInvocationList().Any(d => d.Method.Name == "TriggerDialogue")) // Пздц
            npc.button.btnPress -= TriggerDialogue;
    }
}