public class ChatQuest : Quest
{
    private int npcID;
    // private StartDialogComponent dialogComponent;
    private string dialogID;

    public ChatQuest(string parametr) : base(parametr)
    {

        // ParsingUtility utility = new ParsingUtility(parametr);

        // npcID = utility.GetValue<int>("id");
        // bool autoStart = utility.GetValue<bool>("autoStart");
        // dialogID = utility.GetValue<string>("dialog");

        // if (autoStart)
        //     Got();
        // else
            // DialogSystem.DialogText.DialogActionEvent += GotQuest;


    }

    private void PassQuest(string id, string action)
    {
        if (action == "PassQuest")
            Pass();
    }

    private void GotQuest(string id, string action)
    {
        if (action != "GotQuest")
            return;

        Got();
    }

    public override void Destroy()
    {
        // DialogSystem.DialogText.DialogActionEvent -= PassQuest;
    }

    public override void Got()
    {
        base.Got();
        // dialogComponent = NPCManagement.NPCManager.GetNPC(npcID).gameObject.AddComponent<StartDialogComponent>();
        // dialogComponent.SetDialogID(dialogID);

        // DialogSystem.DialogText.DialogActionEvent -= GotQuest;

        // DialogSystem.DialogText.DialogActionEvent += PassQuest;
    }

    public override void Pass()
    {
        // GameObject.Destroy(dialogComponent);
        base.Pass();
    }
}