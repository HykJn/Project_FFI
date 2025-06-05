using UnityEngine;

public abstract class Tool : Item, IUsableItem, IDurability, ISellable
{
    #region Properties
    public override ItemData Data
    {
        get => new(ID, Durability);
        set
        {
            if (value.itemID != ID) return;
            this.Durability = value.value1;
        }
    }
    public ToolType Type => Base.Type;
    public int MaxDurability => Base.MaxDurability;
    public int Durability { get; protected set; }
    public virtual int Price => (int)(Base.Price * (Durability / (float)MaxDurability));

    protected new ToolBase Base => baseData as ToolBase;
    #endregion

    #region PrivateFields
    #endregion

    #region Unity
    #endregion

    #region Methods
    public void Init(int durability)
    {
        this.Durability = durability;
    }

    public override void Init() => Init(MaxDurability);

    public abstract void UseItem(Vector2Int position, KeyCode key);

    public void Sell()
    {
        if (this.Durability <= 0) return;
        Debug.Log(ItemName + "��(��) �Ǹ��մϴ�.");
        Debug.Log($"�Ǹ� ����: {Price}��");

        GameObject.FindWithTag("Player").gameObject.GetComponent<Player>().Golds += Price;
        GameManager.Instance.MiscResult += Price;
        GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        UIManager.Instance.UpdateQuickslot();
    }
    #endregion
}
