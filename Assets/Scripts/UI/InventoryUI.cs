using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InventoryUI : MonoBehaviour, IPanel, IHover
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

    [SerializeField] private Transform inventory;
    [SerializeField] private Transform quickslot;
    private ItemSlot[] inventorySlots;
    private ItemSlot[] quickslotSlots;
    //private ItemSlot beginDragSlot, endDragSlot;
    private (ItemSlot slot, int idx) beginSlot, endSlot;

    [SerializeField] private GraphicRaycaster raycaster;
    private PointerEventData eventData;
    [SerializeField] private GameObject clone;
    [SerializeField] private Scrollbar scroll;

    Player player;
    Item[,] playerInv;
    Item[] playerQuickslot;

    bool tooltipAppeared = false;
    float hoverTick = 0;


    #endregion

    #region ==========Unity==========
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnBeginDrag();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            OnDrag();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnEndDrag();
        }

        OnHover();
    }
    #endregion

    #region ==========Methods==========
    public void Init()
    {
        eventData = new PointerEventData(EventSystem.current);
        if (inventory != null)
        {
            inventorySlots = inventory.GetComponentsInChildren<ItemSlot>();
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].Index = i;
            }
        }
        if (quickslot != null)
        {
            quickslotSlots = quickslot.GetComponentsInChildren<ItemSlot>();
            for (int i = 0; i < quickslotSlots.Length; i++)
            {
                quickslotSlots[i].Index = i;
            }
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < 36; i++)
        {
            ItemSlot slot = inventorySlots[i];
            Item playerItem = playerInv[i / 9, i % 9];

            if (playerItem != null)
            {
                slot.Icon.enabled = true;
                slot.Icon.sprite = playerItem.Sprite;
                slot.AmountObj.SetActive(playerItem is IAmount);
                slot.DurabilityObj.SetActive(playerItem is IDurability);
                if (playerItem is IAmount iAmount) slot.Amount = iAmount.Amount;
                else if (playerItem is IDurability iDurability) slot.Durability = iDurability.Durability / (float)iDurability.MaxDurability;
            }
            else
            {
                slot.Icon.sprite = null;
                slot.Icon.enabled = false;
                slot.AmountObj.SetActive(false);
                slot.DurabilityObj.SetActive(false);
            }
        }
    }

    public void UpdateQuickslot()
    {
        for (int i = 0; i < 9; i++)
        {
            ItemSlot slot = quickslotSlots[i];
            Item playerItem = playerQuickslot[i];

            if (playerItem != null)
            {
                slot.Icon.enabled = true;
                slot.Icon.sprite = playerItem.Sprite;
                slot.Item = playerItem;
                slot.AmountObj.SetActive(playerItem is IAmount);
                slot.DurabilityObj.SetActive(playerItem is IDurability);
                if (playerItem is IAmount iAmount) slot.Amount = iAmount.Amount;
                else if (playerItem is IDurability iDurability) slot.Durability = iDurability.Durability / (float)iDurability.MaxDurability;
            }
            else
            {
                slot.Icon.sprite = null;
                slot.Icon.enabled = false;
                slot.AmountObj.SetActive(false);
                slot.DurabilityObj.SetActive(false);
            }
        }

        UIManager.Instance.UpdateQuickslot();
    }

    public void OpenPanel()
    {
        if (Enabled) return;
        this.gameObject.SetActive(true);

        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player != null)
        {
            if (playerInv == null) playerInv = player.Inventory;
            if (playerQuickslot == null) playerQuickslot = player.Quickslot;
        }

        player.IsMovable = false;
        player.IsInteractable = false;
        UpdateInventory();
        UpdateQuickslot();

        UIManager.Instance.ActivatedPanels.Add(this);
    }

    public void ClosePanel()
    {
        if (!Enabled) return;
        this.gameObject.SetActive(false);

        player.IsMovable = true;
        player.IsInteractable = true;

        UIManager.Instance.ActivatedPanels.Remove(this);
    }

    ItemSlot GetSlot(Vector3 position)
    {
        eventData.position = position;
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
        if (result.Count == 0) return null;
        if (result[0].gameObject.TryGetComponent<ItemSlot>(out ItemSlot slot))
        {
            return slot;
        }
        return null;
    }

    int GetIdx(ItemSlot slot)
    {
        if (slot == null) return -1;

        for (int i = 0; i < 36; i++)
        {
            if (slot == inventorySlots[i]) return i;
        }

        for (int i = 0; i < 9; i++)
        {
            if (slot == quickslotSlots[i]) return i + 36;
        }

        return -1;
    }

    void OnBeginDrag()
    {
        beginSlot.slot = GetSlot(Input.mousePosition);
        beginSlot.idx = GetIdx(beginSlot.slot);
        if (beginSlot.slot != null) { clone.GetComponent<SpriteRenderer>().sprite = beginSlot.slot.Icon.sprite; }
    }

    void OnDrag()
    {
        if (beginSlot.slot != null) clone.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnEndDrag()
    {
        endSlot.slot = GetSlot(Input.mousePosition);
        endSlot.idx = GetIdx(endSlot.slot);
        clone.GetComponent<SpriteRenderer>().sprite = null;
        Swap();
    }

    void Swap()
    {
        if (beginSlot.slot == null || endSlot.slot == null) return;

        (Array arr, int row, int col, Item item) from, to;
        from.col = beginSlot.idx % 9;
        to.col = endSlot.idx % 9;

        if (beginSlot.idx < 36)
        {
            from.arr = playerInv;
            from.row = beginSlot.idx / 9;
            from.item = playerInv[from.row, from.col];
        }
        else
        {
            from.arr = playerQuickslot;
            from.row = -1;
            from.item = playerQuickslot[from.col];
        }

        if (endSlot.idx < 36)
        {
            to.arr = playerInv;
            to.row = endSlot.idx / 9;
            to.item = playerInv[to.row, to.col];
        }
        else
        {
            to.arr = playerQuickslot;
            to.row = -1;
            to.item = playerQuickslot[to.col];
        }

        if (from.row == -1) from.arr.SetValue(to.item, from.col);
        else from.arr.SetValue(to.item, from.row, from.col);

        if (to.row == -1) to.arr.SetValue(from.item, to.col);
        else to.arr.SetValue(from.item, to.row, to.col);

        UpdateInventory();
        UpdateQuickslot();
    }

    public void OnHover()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            hoverTick += Time.deltaTime;
            if (hoverTick >= 0.5f)
            {
                ItemSlot slot = GetSlot(Input.mousePosition);
                if (slot != null && slot.Icon.sprite != null)
                {
                    if (!tooltipAppeared)
                    {
                        StartCoroutine(UIManager.Instance.AppearTooltip(slot.Item.Description));
                        tooltipAppeared = true;
                    }
                }
                hoverTick = 0.5f; // Reset hover tick after showing tooltip
            }
        }
        else
        {
            hoverTick = 0;
            if (tooltipAppeared)
            {
                StartCoroutine(UIManager.Instance.DisappearTooltip());
                tooltipAppeared = false;
            }
        }
    }
    #endregion
}
