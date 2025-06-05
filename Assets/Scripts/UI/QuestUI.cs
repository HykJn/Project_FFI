using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IPanel
{
    #region ==========Fields==========
    public bool Enabled
    {
        get => questPanel.activeSelf;
        set
        {
            if (value) OpenPanel();
            else ClosePanel();
        }
    }

    [SerializeField] private GameObject questPrefab;
    [Header("Quest Main Panel")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Transform questList;
    [SerializeField] private Text questDescription;
    [SerializeField] private Button rewardButton;
    [Header("Mini Quest Panel")]
    [SerializeField] private Transform miniQuestList;
    private Player player;
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========
    public void AddQuest(Quest quest)
    {
        if(quest == null) return;
        QuestUIElement inPanel = Instantiate(questPrefab, questList).GetComponent<QuestUIElement>();
        inPanel.Init(quest, 35);
        inPanel.OnClick.AddListener(() => questDescription.text = quest.Description);
        inPanel.OnClick.AddListener(quest.Read);
        inPanel.OnClick.AddListener(() => SetRewardButton(quest));

        QuestUIElement inMini = Instantiate(questPrefab, miniQuestList).GetComponent<QuestUIElement>();
        inMini.Init(quest, 25);
        inMini.OnClick.AddListener(OpenPanel);
        inMini.OnClick.AddListener(inPanel.OnClick.Invoke);

        QuestManager.AddQuest(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        if (quest == null) return;
        QuestUIElement[] quests = questList.GetComponentsInChildren<QuestUIElement>();
        foreach (QuestUIElement q in quests)
        {
            if (q.Quest == quest)
            {
                Destroy(q.gameObject);
                break;
            }
        }
        quests = miniQuestList.GetComponentsInChildren<QuestUIElement>();
        foreach (QuestUIElement q in quests)
        {
            if (q.Quest == quest)
            {
                Destroy(q.gameObject);
                break;
            }
        }
        QuestManager.RemoveQuest(quest);
    }

    public void UpdateAll()
    {
        QuestUIElement[] questElements = questList.GetComponentsInChildren<QuestUIElement>();
        foreach(QuestUIElement quest in questElements)
        {
            quest.UpdateUI();
        }
        questElements = miniQuestList.GetComponentsInChildren<QuestUIElement>();
        foreach(QuestUIElement quest in questElements)
        {
            quest.UpdateUI();
        }
    }

    public void UpdateUI(Quest quest)
    {
        QuestUIElement[] quests = questList.GetComponentsInChildren<QuestUIElement>();
        foreach(QuestUIElement q in quests)
        {
            if (q.Quest == quest)
            {
                q.UpdateUI();
                break;
            }
        }
        quests = miniQuestList.GetComponentsInChildren<QuestUIElement>();
        foreach (QuestUIElement q in quests)
        {
            if (q.Quest == quest)
            {
                q.UpdateUI();
                break;
            }
        }
    }

    void SetRewardButton(Quest quest)
    {
        rewardButton.interactable = quest.State == QuestState.Clear;

        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
        {
            player.Golds += quest.Reward;
            RemoveQuest(quest);
        });
        rewardButton.GetComponentInChildren<Text>().text = quest.Reward.ToString();
    }

    public void OpenPanel()
    {
        if (Enabled) return;

        questPanel.SetActive(true);

        if(player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.IsMovable = false;
        player.IsInteractable = false;

        UIManager.Instance.ActivatedPanels.Add(this);
    }

    public void ClosePanel()
    {
        if (!Enabled) return;

        questPanel.SetActive(false);

        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.IsMovable = true;
        player.IsInteractable = true;

        UIManager.Instance.ActivatedPanels.Remove(this);
    }
    #endregion
}
