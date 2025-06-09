using UnityEngine;

public class SpawnEgg : Item, IAmount, IUsableItem
{
    #region ==========Fields==========
    public override ItemData Data
    {
        get => new(ID, Amount);
        set
        {
            if (value.itemID != ID) return;
            Amount = value.value1;
        }
    }
    public ItemID AnimalID => Base.AnimalID;
    public int Amount { get; set; }
    private new SpawnEggBase Base => baseData as SpawnEggBase;
    #endregion

    #region ==========Unity Events==========
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region ==========Methods==========
    public void Init(int amount)
    {
        Amount = amount;
    }

    public override void Init() => Init(1);

    public void UseItem(Vector2Int position, KeyCode key)
    {
        if(key != KeyCode.Mouse0 || Amount <= 0) return;

        if (TileManager.Instance.SpawnAnimal(position, ObjectManager.Instance.GetInstance(AnimalID)))
        {
            this.Amount--;
        }

        if (this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }
    #endregion
}
