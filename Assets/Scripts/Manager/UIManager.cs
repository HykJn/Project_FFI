using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class UIManager : MonoBehaviour
{

    #region ==========Fields==========
    public static UIManager Instance => instance;
    public List<IPanel> ActivatedPanels => activatedPanels;
    public Canvas MainCanvas => mainCanvas;
    public Canvas TutoCanvas => tutoCanvas;
    public QuestUI QuestPanel => questPanel;

    //Singleton instance
    private static UIManager instance;

    //UI Objects
    [Header("UI Objects")]
    //Quickslot
    [SerializeField] private ItemSlot[] quickslot;
    [SerializeField] private GameObject selector;
    [SerializeField] private GameObject tooltip;
    [Header("Canvas")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas tutoCanvas;

    [Header("Panels")]
    [SerializeField] private GameObject[] panels;
    [SerializeField] private QuestUI questPanel;
    private List<IPanel> activatedPanels;
    [Header("Daily Result")]
    [SerializeField] private Text dayCount;
    [SerializeField] private Text farmingResult;            
    [SerializeField] private Text animalResult;
    [SerializeField] private Text expenseResult;
    [SerializeField] private Text totalResult;
    [Header("Infomation")]
    [SerializeField] private Image weather;
    [SerializeField] private Sprite[] weathers;
    [SerializeField] private RectTransform timerArrow;
    [SerializeField] private Text timer;
    [SerializeField] private Text golds;

    //Player
    private GameObject playerObj;
    private Player playerScript;
    #endregion

    #region ==========Unity Events==========
    void Awake()
    {
        Init();
    }

    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();

        UpdateQuickslot();
        UpdateTime((int)GameManager.Instance.CurTime, GameManager.MINUITES_OF_DAY);
        UpdateGolds(playerScript.Golds);
    }
    #endregion

    #region ==========Methods==========
    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        activatedPanels = new List<IPanel>();
    }

    public void UpdateQuickslot()
    {
        for (int i = 0; i < 9; i++)
        {
            Item playerItem = playerScript.Quickslot[i];
            ItemSlot slot = quickslot[i];
            if (playerItem != null)
            {
                slot.Icon.enabled = true;
                slot.Icon.sprite = playerItem.Sprite;
                switch (playerItem)
                {
                    case IDurability iDurability:
                        slot.AmountObj.SetActive(false);
                        slot.DurabilityObj.SetActive(true);
                        slot.Durability = iDurability.Durability / (float)iDurability.MaxDurability;
                        break;
                    case IAmount iAmount:
                        slot.AmountObj.SetActive(true);
                        slot.DurabilityObj.SetActive(false);
                        slot.Amount = iAmount.Amount;
                        break;
                }
            }
            else
            {
                slot.Icon.enabled = false;
                slot.AmountObj.SetActive(false);
                slot.DurabilityObj.SetActive(false);
            }
        }
    }

    public void SetSelectorPos(int idx)
    {
        selector.transform.localPosition = new Vector2(-4.5f + idx, 1);
        selector.GetComponent<Animation>().Play();
    }

    public void SwitchInGamePanel(InGamePanelType panelType)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            IPanel panel = panels[i].GetComponent<IPanel>();
            panel.Enabled = i == (int)panelType && !panel.Enabled;
        }
    }

    public void ClosePanel()
    {
        if (activatedPanels.Count <= 0) return;

        IPanel panel = ActivatedPanels[^1];
        panel.ClosePanel();
    }

    public void UpdateTime(int curTime, int dayLength)
    {
        timer.text = (6 + curTime / 60).ToString("00") + ":" + (curTime % 60).ToString("00");
        timerArrow.rotation = Quaternion.Euler(0, 0, 90 - curTime * 180 / dayLength);
    }

    public void UpdateWeather(Weather _weather)
    {
        weather.sprite = weathers[(int)_weather];
    }

    public void UpdateGolds(int golds)
    {
        if (golds >= 1000000)
        {
            float _golds = golds / 1000000f;
            this.golds.text = _golds.ToString("0.00") + "M";
        }
        else
        {
            this.golds.text = golds.ToString();
        }
    }

    public void DailyResult()
    {
        SwitchInGamePanel(InGamePanelType.DailyResult);

        dayCount.text = "Day " + GameManager.Instance.CurDay.ToString();
        farmingResult.text = $"Farming: {GameManager.Instance.FarmingResult}";
        animalResult.text = $"Animal: {GameManager.Instance.AnimalResult}";
        expenseResult.text = $"Expense: {GameManager.Instance.ExpenseResult}";
        bool isPosi = GameManager.Instance.TotalResult >= 0;
        totalResult.text = $"Total: {playerScript.Golds} {(isPosi ? "+" : "-")} {(isPosi ? GameManager.Instance.TotalResult : -GameManager.Instance.TotalResult)} = {playerScript.Golds + GameManager.Instance.TotalResult}";
    }

    public void ExitGame()
    {
        GameManager.Instance.ToExit();
    }

    public void FastFoward()
    {
        GameManager.Instance.FastFoward();
    }

    public bool MouseOverUI()
    {
        // Check if the mouse is over any UI element
        return EventSystem.current.IsPointerOverGameObject();
    }

    public IEnumerator AppearTooltip(string text)
    {
        StopCoroutine(DisappearTooltip());
        tooltip.SetActive(true);
        tooltip.GetComponent<RectTransform>().position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Text description = tooltip.GetComponentInChildren<Text>();
        description.text = text;
        Image tooltipBG = tooltip.GetComponent<Image>();
        while(tooltipBG.color.a < 1f)
        {
            Color temp = tooltipBG.color;
            temp.a += Time.deltaTime * 3f;
            tooltipBG.color = temp;

            temp = description.color;
            temp.a += Time.deltaTime * 3f;
            description.color = temp;
            yield return null;
        }
    }

    public IEnumerator DisappearTooltip()
    {
        StopCoroutine(AppearTooltip(string.Empty));
        Image tooltipBG = tooltip.GetComponent<Image>();
        Text description = tooltip.GetComponentInChildren<Text>();
        while (tooltipBG.color.a > 0f)
        {
            Color temp = tooltipBG.color;
            temp.a -= Time.deltaTime * 3f;
            tooltipBG.color = temp;

            temp = description.color;
            temp.a -= Time.deltaTime * 3f;
            description.color = temp;
            yield return null;
        }
        description.text = string.Empty;
        tooltip.SetActive(false);
    }

    public void SaveData() => GameManager.Instance.SaveData();
    public void LoadData() => GameManager.Instance.LoadData();

    public void ButtonClickSound() => SoundManager.Instance.SFX_Play(SFXID.ButtonClick);
    public void SliderMoveSound() => SoundManager.Instance.SFX_PlayOneShotWithCheck(SFXID.OnSliderMove);
    #endregion
}
