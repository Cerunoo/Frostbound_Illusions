public class DeliveryQuest : Quest
{
    private int fromID;
    private int toID;

    // private StartDialogComponent sender;
    // private StartDialogComponent target;

    public DeliveryQuest(string parametrs) : base(parametrs)
    {
        // ParsingUtility utility = new ParsingUtility(parametrs);

        // fromID = utility.GetValue<int>("from");
        // toID = utility.GetValue<int>("to");

        // var dialogIDs = utility.GetValues<string>("dialogs", 2);


        // sender = NPCManagement.NPCManager.GetNPC(fromID).gameObject.AddComponent<StartDialogComponent>();
        // sender.SetDialogID(dialogIDs[0]);

        // target = NPCManagement.NPCManager.GetNPC(toID).gameObject.AddComponent<StartDialogComponent>();
        // target.SetDialogID(dialogIDs[1]);

        // DialogSystem.DialogText.DialogActionEvent += GotQuest;
    }

    private void GotQuest(string id, string action)
    {
        if (action == "GotQuest")
            Got();
    }

    private void PassQuest(string id, string action)
    {
        if (action != "PassQuest")
            return;
        Pass();
    }

    public override void Destroy()
    {
        // DialogSystem.DialogText.DialogActionEvent -= GotQuest;
    }

    public override void Got()
    {
        base.Got();
        // GameObject.Destroy(sender);
        // DialogSystem.DialogText.DialogActionEvent -= GotQuest;
        // DialogSystem.DialogText.DialogActionEvent += PassQuest;
    }

    public override void Pass()
    {
        base.Pass();
        // GameObject.Destroy(target);
    }
}