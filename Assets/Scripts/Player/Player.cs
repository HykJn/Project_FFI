using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    //General
    public int Health { get; set; } = 100;
    public int Stamina { get; set; } = 100;
    public int Golds
    {
        get => golds;
        set
        {
            golds = value;
            UIManager.Instance.UpdateGolds(golds);
        }
    }

    //Items
    public Item[,] Inventory => inventory;
    public Item[] Quickslot => quickslot;
    public Item CurItem => curItem; 

    //Buffs
    public int FoodBonus { get; set; } = 1;
    public int GoldBonus { get; set; } = 1;

    //States
    public bool IsMovable { get; set; } = true;
    public bool IsInteractable { get; set; } = true;

    #endregion

    #region PrivateFields
    private int golds = 0;

    //Move
    [Header("Movement")]
    [SerializeField] private float speed;

    //Item
    [Header("Item")]
    private Item curItem;
    private List<Ingredient> ingredients = new List<Ingredient>();
    //Quickslot
    [SerializeField] private Item[] quickslot;
    private int quickslot_Idx = 0;
    //Inventory
    private Item[,] inventory;
    //ActionBox
    [SerializeField] private GameObject actionBox;

    //Camera
    [Header("Camera")]
    private Camera cam;
    [SerializeField] private float offset_x;
    [SerializeField] private float offset_y;

    //Misc
    Vector2 mousePos;
    #endregion

    #region Components
    Animator anim;
    #endregion

    #region Unity
    void Start()
    {
        Init();
    }

    void Update()
    {
        PlayerControl();
    }

    private void FixedUpdate()
    {
        CameraFollowing();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            GetItem(collision.GetComponent<Item>());
            SelectItem();
            UIManager.Instance.UpdateQuickslot();
            SoundManager.Instance.SFX_PlayOneShot(SFXID.GetItem);
        }
    }
    #endregion

    #region Methods
    void Init()
    {
        inventory = new Item[4, 9];
        quickslot = new Item[9];
        curItem = null;
        cam = Camera.main;
        anim = this.GetComponentInChildren<Animator>();
    }

    void PlayerControl()
    {
        if (!GameManager.Instance.IsPlaying) return;
        //variables
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //IsMovable = !UIManager.Instance.AnyPanelActivated;
        //IsInteractable = !UIManager.Instance.AnyPanelActivated;

        //Move
        if (input_x != 0 || input_y != 0 )
        {
            Move(new Vector2(input_x, input_y), Input.GetKey(KeyCode.LeftShift));
        }
        //Idle
        else
        {
            //Set Animator
            anim.SetBool("IsMove", false);
        }

        //SelectItem
        //By Wheelscroll
        if (Input.mouseScrollDelta.y != 0)
        {
            SelectItem();
        }
        //By Keyboard**********
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)(i + 49)))
            {
                SelectItem(i);
                break;
            }
        }

        //ActionBox
        ActionBoxActive();

        //UseItem
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (UIManager.Instance.MouseOverUI()) return;
            if (TileManager.Instance.TryGetPropObj(GetInteractPos(), out PropObj prop))
            {
                if (prop is IIneteractable temp && IsInteractable)
                {
                    if (CurItem == null || !(CurItem.ID >= ItemID.TOOL_NORMAL_AXE && CurItem.ID < ItemID.TOOL_NORMAL_HOE))
                    {
                        temp.Interact();
                        return;
                    }
                }
            }
            UseItem(CurItem, GetInteractPos(), KeyCode.Mouse0);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UseItem(CurItem, GetInteractPos(), KeyCode.Mouse1);
        }

        //UI
        //Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.SwitchInGamePanel(InGamePanelType.Inventory);
        }
        //Quest
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            UIManager.Instance.SwitchInGamePanel(InGamePanelType.Quest);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.ActivatedPanels.Count > 0)
            {
                UIManager.Instance.ClosePanel();
            }
            else
            {
                UIManager.Instance.SwitchInGamePanel(InGamePanelType.Option);
            }
        }
    }

    void CameraFollowing()
    {
        float camX = cam.transform.position.x, camY = cam.transform.position.y;

        if (this.transform.position.x - camX > offset_x) camX = this.transform.position.x - offset_x;
        else if (this.transform.position.x - camX < -offset_x) camX = this.transform.position.x + offset_x;

        if (this.transform.position.y - camY > offset_y) camY = this.transform.position.y - offset_y;
        else if (this.transform.position.y - camY < -offset_y) camY = this.transform.position.y + offset_y;

        cam.transform.position = new Vector3(camX, camY, -1);
    }

    //Methods for move
    void Move(Vector2 dir, bool sprint)
    {
        if (!IsMovable) return;
        //Move by transform
        this.transform.position += Time.deltaTime * speed * (sprint ? 1.5f : 1f) * (Vector3)dir.normalized;

        Vector3 origin = this.transform.position;
        this.transform.position = new Vector3(origin.x, origin.y, origin.y / 100f);

        //Set Animator
        anim.SetBool("IsMove", true);
        anim.SetFloat("IsRun", sprint ? 1 : 0);
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
    }

    //Methods for SelectItem
    void SelectItem()
    {
        if (!IsMovable) return;
        if (Input.mouseScrollDelta.y > 0) quickslot_Idx = (quickslot_Idx + 8) % 9;
        else if (Input.mouseScrollDelta.y < 0) quickslot_Idx = (quickslot_Idx + 1) % 9;
        curItem = quickslot[quickslot_Idx];
        UIManager.Instance.SetSelectorPos(quickslot_Idx);
        UIManager.Instance.UpdateQuickslot();
    }

    void SelectItem(int idx)
    {
        if(!IsMovable) return;
        quickslot_Idx = idx;
        curItem = quickslot[idx];
        UIManager.Instance.SetSelectorPos(quickslot_Idx);
        UIManager.Instance.UpdateQuickslot();
    }

    void SelectItem(Item item)
    {
        curItem = item;
    }

    void UseItem(Item item, Vector2Int pos, KeyCode key)
    {
        if (!IsMovable) return;

        //If Usable item
        if (item is IUsableItem usable)
        {
            //Use Item
            usable.UseItem(pos, key);

            //If Item is Tool
            if (item is Tool tool)
            {
                //Set Animator
                Vector2 dir = pos - (Vector2)this.transform.position;
                anim.SetTrigger("Action");
                anim.SetFloat("ActionID", (int)tool.Type);
                anim.SetFloat("X", dir.x);
                anim.SetFloat("Y", dir.y);
            }
        }

        UIManager.Instance.UpdateQuickslot();
    }

    //Methods for GetItem
    public void GetItem(Item item)
    {
        if (item == null) return;

        int emptySlot = -1; //index for quickslot

        //Search quickslot
        for (int i = 0; i < 9; i++)
        {
            //If current slot is empty
            if (quickslot[i] == null)
            {
                //IDurability => non-stackable
                if(item is IDurability)
                {
                    quickslot[i] = item;
                    item.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    //Set smallest index of empty slot
                    if (emptySlot == -1)
                    {
                        emptySlot = i;
                    }
                    continue;
                }
            }
            //If current slot has item
            else
            {
                //If both are same type and IAmount
                if (quickslot[i].GetType() == item.GetType() && quickslot[i] is IAmount iAmount)
                {
                    if (quickslot[i].Equals(item))
                    {
                        iAmount.Amount++;
                        ObjectManager.Instance.ReturnHierarchy(item.gameObject);
                        OnGetItem(item);
                        if (item is Ingredient) ingredients.Add(item as Ingredient);
                        return;
                    }
                }
            }
        }

        //If quickslot hasn't item but empty slot
        if (emptySlot != -1)
        {
            //Get item in empty slot
            quickslot[emptySlot] = item;
            item.gameObject.SetActive(false);
            OnGetItem(item);
            if (item is Ingredient) ingredients.Add(item as Ingredient);
            return;
        }
        //Quickslot hasn't empty slot and item
        else
        {
            //Search inventory
            for (int i = 0; i < 36; i++)
            {
                //If current slot is empty
                if (inventory[i / 9, i % 9] == null)
                {
                    //IDurability => non-stackable
                    if (item is IDurability)
                    {
                        inventory[i / 9, i % 9] = item;
                        item.gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        //Set smallest index of empty slot
                        if (emptySlot == -1)
                        {
                            emptySlot = i;
                        }
                        continue;
                    }
                }
                //If current slot has item
                else
                {
                    //If both are same type and IAmount
                    if (inventory[i / 9, i % 9].GetType() == item.GetType() && inventory[i / 9, i % 9] is IAmount iAmount)
                    {
                        if (inventory[i / 9, i % 9].Equals(item))
                        {
                            iAmount.Amount++;
                            ObjectManager.Instance.ReturnHierarchy(item.gameObject);
                            OnGetItem(item);
                            if (item is Ingredient) ingredients.Add(item as Ingredient);
                            return;
                        }
                    }
                }
            }
        }

        //If inventory hasn't item but empty slot
        if (emptySlot != -1)
        {
            //Get item in empty slot
            inventory[emptySlot / 9, emptySlot % 9] = item;
            item.gameObject.SetActive(false);
            OnGetItem(item);
            if (item is Ingredient) ingredients.Add(item as Ingredient);
            return;
        }
    }

    void OnGetItem(Item item)
    {
        if(item.ID == ItemID.EGG_NORMAL)
            QuestManager.OnQuestProceed?.Invoke(ActionID.GetEgg, 1);
    }

    //Methods for RemoveItem
    public void RemoveItem(Item item)
    {
        if (curItem == item)
        {
            curItem = null;
        }
        for (int i = 0; i < 9; i++)
        {
            if (quickslot[i] == item)
            {
                quickslot[i] = null;
                break;
            }
        }

        for (int r = 0; r < 4; r++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (inventory[r, j] == item)
                {
                    inventory[r, j] = null;
                    break;
                }
            }
        }
        UIManager.Instance.UpdateQuickslot();
        ObjectManager.Instance.ReturnHierarchy(item.gameObject);
    }

    //Methods for ActionBox
    Vector2Int GetInteractPos()
    {
        //Init
        Vector2Int interactPos;
        int x = 0, y = 0;

        //Check mouse position
        if (mousePos.x - this.transform.position.x > 0.5f) x = 1;
        else if (mousePos.x - this.transform.position.x < -0.5f) x = -1;

        if (mousePos.y - this.transform.position.y > 0.5f) y = 1;
        else if (mousePos.y - this.transform.position.y < -0.5f) y = -1;

        //If in center
        if (x == 0 && y == 0)
        {
            interactPos = Vector2Int.RoundToInt(this.transform.position + ((Vector3)mousePos - this.transform.position).normalized);
        }
        else
        {
            interactPos = Vector2Int.RoundToInt(this.transform.position) + new Vector2Int(x, y);
        }

        return interactPos;
    }

    void ActionBoxActive()
    {
        actionBox.SetActive(CurItem is IUsableItem && CurItem is not Crop);

        if (actionBox.activeSelf)
        {
            actionBox.transform.position = (Vector2)GetInteractPos();
        }
    }
    #endregion
}
