using UnityEngine;
using UnityEngine.UI;

public class QuestUIElement : MonoBehaviour
{
    #region ==========Fields==========
    public Button.ButtonClickedEvent OnClick => button.onClick;
    public Quest Quest => quest;

    private Quest quest;
    private Text questName;
    private Button button;
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========
    public void Init(Quest quest, int fontSize)
    {
        this.quest = quest;

        questName = this.GetComponent<Text>();
        button = this.GetComponent<Button>();

        questName.text = $"[!] {quest.Name} [{quest.Current}/{quest.Goal}]";
        questName.color = QuestManager.Color_NotRead;
        questName.fontSize = fontSize;
    }

    public void UpdateUI()
    {
        switch (quest.State)
        {
            case QuestState.NotRead:
                questName.text = $"[!] {quest.Name} [{quest.Current}/{quest.Goal}]";
                questName.color = QuestManager.Color_NotRead;
                break;
            case QuestState.Read:
                questName.text = $"[#] {quest.Name} [{quest.Current}/{quest.Goal}]";
                questName.color = QuestManager.Color_Read;
                break;
            case QuestState.Clear:
                questName.text = $"[\u2713] {quest.Name} [{quest.Current}/{quest.Goal}]";
                questName.color = QuestManager.Color_Clear;
                break;
        }
    }
    #endregion
}
