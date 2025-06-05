
using UnityEngine;

public class Quest
{
    #region ==========Fields==========
    public ActionID Action { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public QuestState State { get; private set; }
    public int Current { get; private set; }
    public int Goal { get; private set; }
    public int Reward { get; private set; }
    #endregion

    #region ==========Methods==========
    void QuestProceed(ActionID action, int value)
    {
        if (IsSameAction(action))
        {
            Current += value;
            if (Current >= Goal)
            {
                State = QuestState.Clear;
                Current = Goal;
            }
            UIManager.Instance.QuestPanel.UpdateUI(this);
        }
    }

    public void Read()
    {
        if (State == QuestState.NotRead)
        {
            State = QuestState.Read;
            UIManager.Instance.QuestPanel.UpdateUI(this);
        }
    }

    public void OnClear()
    {
        if (State != QuestState.Clear) return;
        QuestManager.OnQuestProceed -= QuestProceed;
        if (this == QuestManager.FirstQuest)
        {
            ActionID[] questID = new ActionID[]
            {
                ActionID.Plant,
                ActionID.PlantCarrot,
                ActionID.PlantEggplant,
                ActionID.Harvest,
                ActionID.HarvestCarrot,
                ActionID.HarvestEggplant,
                ActionID.GetAnimalProduct,
                ActionID.GetEgg
            };
            foreach(ActionID id in questID)
            {
                UIManager.Instance.QuestPanel.AddQuest(QuestManager.AutoQuestGenerator(id));
            }
        }
        else
        {
            UIManager.Instance.QuestPanel.AddQuest(QuestManager.AutoQuestGenerator(Action));
        }
    }

    bool IsSameAction(ActionID action)
    {
        string origin = ((int)Action).ToString("X4");
        string target = ((int)action).ToString("X4");
        string result = string.Empty;
        for (int i = 0; i < 4; i++)
        {
            if(origin[i] == target[i])
            {
                result += origin[i];
            }
            else
            {
                result += '0';
            }
        }
        return origin.Equals(result);
    }

    public Quest(ActionID action, string name, string description, int goal, int reward)
    {
        Action = action;
        Name = name;
        Description = description;
        State = QuestState.NotRead;
        Current = 0;
        Goal = goal;
        Reward = reward;
        QuestManager.OnQuestProceed += QuestProceed;
    }
    #endregion
}