using UnityEngine;

public class Ingredient : Item, IAmount, ISellable
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
    public int Price => Base.Price;

    private new IngredientBase Base => baseData as IngredientBase;
    #endregion

    #region ==========Unity Methods==========
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region ==========Methods==========
    public void Init(int amount) => this.Amount = amount;
    public override void Init() => Init(1);

    public void Sell()
    {
        if (this.Amount <= 0) return;


        Debug.Log(ItemName + "을(를) 판매합니다.");
        Debug.Log($"판매 가격: {Price}원");
        GameObject.FindWithTag("Player").GetComponent<Player>().Golds += Price;
        GameManager.Instance.FarmingResult += Price;
        this.Amount--;
        if (this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
        UIManager.Instance.UpdateQuickslot();
    }
    #endregion
}
