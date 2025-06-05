using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    //TODO: Implement the tutorial UI logic here.
    #region ==========Fields==========
    [SerializeField] private GameObject[] tutorialPanels;
    Player player;
    int idx = 0;
    #endregion

    #region ==========Unity Methods==========
    private void Start()
    {
        tutorialPanels[idx].SetActive(true);
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        player.IsMovable = false;
        player.IsInteractable = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(idx == 11)
            {
                Text text = tutorialPanels[11].GetComponentInChildren<Text>();
                text.text = "열심히 해서 자신만의 농장을\n가꾸어봐요!";
                idx++;
                return;
            }
            if (idx == 12)
            {
                player.IsMovable = true;
                player.IsInteractable = true;
                UIManager.Instance.QuestPanel.AddQuest(QuestManager.FirstQuest);
                this.enabled = false;
                Destroy(this.gameObject);
                return;
            }
            tutorialPanels[idx].SetActive(false);
            tutorialPanels[++idx].SetActive(true);
            if (idx == 2 || idx == 4) UIManager.Instance.SwitchInGamePanel(InGamePanelType.Option);
            else if (idx == 6 || idx == 7) UIManager.Instance.SwitchInGamePanel(InGamePanelType.Inventory);
            else if (idx == 9 || idx == 11) UIManager.Instance.SwitchInGamePanel(InGamePanelType.Store);
        }
    }
    #endregion

    #region ==========Methods==========
    public void AddStartQuest()
    {
        if (QuestManager.Quests.Length > 0) return;
        UIManager.Instance.QuestPanel.AddQuest(QuestManager.FirstQuest);
    }
    #endregion
}
