using System;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public static event Action<Quest> QuestPassedEvent;
    public static event Action<Quest> QuestGotEvent;

    public string QuestName { get; protected set; }
    public string QuestDescription { get; protected set; }

    public virtual void Pass() => QuestPassedEvent?.Invoke(this);
    public virtual void Got() => QuestGotEvent?.Invoke(this);
    public virtual void Start() { }
    public virtual void Destroy() { }

    public Quest(string parametrs)
    {
        // List<string> parList;
        string json = @"{
            ""name"": ""QuestName"",
            ""description"": ""QuestDesc""
        }";
        QuestList questList = JsonUtility.FromJson<QuestList>(json);
        Debug.Log(JsonUtility.FromJson<QuestList>(json));
        Debug.Log(questList.name);
        Debug.Log(questList.description);

        // foreach (Quest quest in questList.Quests)
        // List<string> parList = parametrs.GetWords();
        // List<string> parList = 
        // parametrs = JsonUtility.FromJson<Quest>(parametrs);

        // var nameIndex = parList.FindIndex(s => s == "name");
        // var descIndex = parList.FindIndex(s => s == "description");

        // QuestName = "";
        // for (int i = nameIndex + 1; i < descIndex; i++)
        //     QuestName += parList[i] + " ";

        // QuestDescription = "";
        // for (int i = descIndex + 1; i < parList.Count; i++)
        //     QuestDescription += parList[i] + " ";
    }
}

[Serializable]
public class QuestList
{
    public string name;
    public string description;
}