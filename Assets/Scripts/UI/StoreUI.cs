using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour, IPanel
{
    #region ==========Fields==========
    public bool Enabled
    {
        get => this.gameObject.activeSelf;
        set
        {
            if (value) OpenPanel();

            else ClosePanel();
        }
    }

    private Player player = null;

    [Header("Store Panel")]
    [SerializeField] private Animator frog;
    [SerializeField] private Text bubble;
    [SerializeField, TextArea] private string[] conversations;
    private string word, curWord;
    private bool talkFlag = false;
    private float talkTick = 0f;
    private int talkIdx = 0;
    [SerializeField] private Transform store;
    [SerializeField] private Text gold;

    [Header("Inventory Panel")]
    [SerializeField] private Transform inventory;
    [SerializeField] private Transform quickslot;

    private StoreSlot[] storeSlots;
    private ItemSlot[] inventorySlots;
    private ItemSlot[] quickslotSlots;
    #endregion

    #region ==========Unity==========
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Talk();
    }
    #endregion

    #region ==========Methods==========
    void Init()
    {
        storeSlots = store.GetComponentsInChildren<StoreSlot>();
        inventorySlots = inventory.GetComponentsInChildren<ItemSlot>();
        quickslotSlots = quickslot.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < storeSlots.Length; i++)
        {
            if (storeSlots[i].ItemID != ItemID.ITEM_NULL)
            {
                storeSlots[i].Icon.enabled = true;
                storeSlots[i].Icon.sprite = ObjectManager.Instance.GetPrefab(storeSlots[i].ItemID).GetComponent<Item>().Sprite;
                storeSlots[i].PriceText.enabled = true;
                storeSlots[i].PriceText.text = storeSlots[i].Price.ToString();
                storeSlots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                storeSlots[i].Icon.enabled = false;
                storeSlots[i].Icon.sprite = null;
                storeSlots[i].PriceText.enabled = false;
                storeSlots[i].PriceText.text = "";
                storeSlots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void UpdateInventory()
    {
        //Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        for (int i = 0; i < 36; i++)
        {
            Button btn = inventorySlots[i].GetComponent<Button>();
            ItemSlot slot = inventorySlots[i];
            Text price = slot.transform.GetChild(2).GetComponent<Text>();
            Item playerItem = player.Inventory[i / 9, i % 9];

            btn.onClick.RemoveAllListeners();
            if (playerItem != null)
            {
                slot.Icon.enabled = true;
                slot.Icon.sprite = playerItem.Sprite;
                if (playerItem is IAmount iAmount)
                {
                    slot.AmountObj.SetActive(true);
                    slot.DurabilityObj.SetActive(false);
                    slot.Amount = iAmount.Amount;
                }
                else if (playerItem is IDurability iDurability)
                {
                    slot.AmountObj.SetActive(false);
                    slot.DurabilityObj.SetActive(true);
                    slot.Durability = iDurability.Durability / (float)iDurability.MaxDurability;
                }
                if (playerItem is ISellable iSellable)
                {
                    btn.interactable = true;
                    btn.onClick.AddListener(iSellable.Sell);
                    btn.onClick.AddListener(UpdateInventory);
                    btn.onClick.AddListener(UpdateGold);
                    btn.onClick.AddListener(() => SoundManager.Instance.SFX_Play(SFXID.SellItem));
                    price.text = iSellable.Price.ToString();
                }
                else
                {
                    btn.interactable = false;
                    price.text = "";
                }
            }
            else
            {
                btn.interactable = false;
                slot.Icon.enabled = false;
                slot.AmountObj.SetActive(false);
                slot.DurabilityObj.SetActive(false);
                price.text = "";
            }
        }
    }

    public void UpdateQuickslot()
    {
        //Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        for (int i = 0; i < 9; i++)
        {
            Button btn = quickslotSlots[i].GetComponent<Button>();
            ItemSlot slot = quickslotSlots[i];
            Text price = slot.transform.GetChild(2).GetComponent<Text>();
            Item playerItem = player.Quickslot[i];

            btn.onClick.RemoveAllListeners();
            if (playerItem != null)
            {
                slot.Icon.enabled = true;
                slot.Icon.sprite = playerItem.Sprite;
                if (playerItem is IAmount iAmount)
                {
                    slot.AmountObj.SetActive(true);
                    slot.DurabilityObj.SetActive(false);
                    slot.Amount = iAmount.Amount;
                }
                else if (playerItem is IDurability iDurability)
                {
                    slot.AmountObj.SetActive(false);
                    slot.DurabilityObj.SetActive(true);
                    slot.Durability = iDurability.Durability / (float)iDurability.MaxDurability;
                }
                if (playerItem is ISellable iSellable)
                {
                    btn.interactable = true;
                    btn.onClick.AddListener(iSellable.Sell);
                    btn.onClick.AddListener(UpdateQuickslot);
                    btn.onClick.AddListener(UpdateGold);
                    btn.onClick.AddListener(() => SoundManager.Instance.SFX_Play(SFXID.SellItem));
                    price.text = iSellable.Price.ToString();
                }
                else
                {
                    btn.interactable = false;
                    price.text = "";
                }
            }
            else
            {
                btn.interactable = false;
                slot.Icon.enabled = false;
                slot.AmountObj.SetActive(false);
                slot.DurabilityObj.SetActive(false);
                price.text = "";
            }
        }
    }

    public void UpdateGold()
    {
        int golds = player.Golds;
        if (golds >= 1000000)
        {
            float _golds = golds / 1000000f;
            gold.text = _golds.ToString("0.00") + "M";
        }
        else
        {
            gold.text = golds.ToString();
        }
    }

    public void SetConversation(string conversation)
    {
        word = conversation;
        curWord = "";
        bubble.text = "";
        talkFlag = true;
        talkTick = 0;
        talkIdx = 0;

        frog.SetBool("Talk", true);
    }

    void Talk()
    {
        if (talkFlag)
        {
            talkTick += Time.deltaTime;
            if (talkTick > 0.1f)
            {
                talkTick = 0;
                curWord += word[talkIdx++];
                if (talkIdx >= word.Length)
                {
                    talkFlag = false;
                    frog.SetBool("Talk", false);
                }
            }

            bubble.text = curWord;
        }
    }

    public void OpenPanel()
    {
        if (Enabled) return;
        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();

        this.gameObject.SetActive(true);

        SetConversation(conversations[Random.Range(0,2)]);
        UpdateInventory();
        UpdateQuickslot();
        UpdateGold();

        player.IsMovable = false;
        player.IsInteractable = false;

        UIManager.Instance.ActivatedPanels.Add(this);
    }

    public void ClosePanel()
    {
        if (!Enabled) return;
        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();

        this.gameObject.SetActive(false);

        player.IsMovable = true;
        player.IsInteractable = true;

        UIManager.Instance.ActivatedPanels.Remove(this);
    }

    public void SetStoreItem()
    {
        //TODO: Implement store item setting logic
    }

    public void Sell(StoreSlot slot)
    {
        if(slot.ItemID == ItemID.ITEM_NULL) return;
        if (player.Golds < slot.Price)
        {
            SetConversation("µ·ÀÌ ºÎÁ·ÇØ °³±¼!");
            return;
        }

        player.Golds -= slot.Price;
        GameManager.Instance.ExpenseResult -= slot.Price;
        player.GetItem(ObjectManager.Instance.GetInstance(slot.ItemID, false).GetComponent<Item>());
        SetConversation("°í¸¶¿ö °³±¼!");

        UIManager.Instance.UpdateQuickslot();
        UpdateInventory();
        UpdateQuickslot();
        UpdateGold();
        SoundManager.Instance.SFX_Play(SFXID.Purchase);
    }
    #endregion
}
