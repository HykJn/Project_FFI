using UnityEngine;

public class Prop : Item, IUsableItem, IAmount
{
    #region Fields
    public override ItemData Data
    {
        get => new(ID, Amount);
        set
        {
            if (value.itemID != ID) return;
            Amount = value.value1;
        }
    }
    public ItemID PropID => Base.PropID;
    public int Amount { get; set; }
    private new PropBase Base => baseData as PropBase;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Methods
    public void Init(int amount)
    {
        this.Amount = amount;
    }
    public override void Init() => Init(1);

    public void UseItem(Vector2Int position, KeyCode key)
    {
        if (this.Amount <= 0) return;

        if (key == KeyCode.Mouse0)
        {
            if (TileManager.Instance.PlaceProp(position, ObjectManager.Instance.GetPrefab(PropID)))
            {
                this.Amount--;
            }
        }

        if(this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }
    #endregion
}
