using UnityEngine;

public class Ingredient : Item, IAmount
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
    public int Amount { get; set; }

    private new ItemBase_Default Base => baseData as ItemBase_Default;
    #endregion

    #region ==========Unity Methods==========

    #endregion

    #region ==========Methods==========
    public void Init(int amount) => this.Amount = amount;
    public override void Init() => Init(1);

    #endregion
}
