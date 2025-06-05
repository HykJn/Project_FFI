using System;
using UnityEngine;

public class Egg : Item, IAmount, ISellable
{
    #region Properties
    public override ItemData Data
    {
        get => new(ID, Amount, Grade);
        set
        {
            if (value.itemID != ID) return;
            Amount = value.value1;
            grade = value.value2;
        }
    }
    public int Price => (int)(Base.Price * (1 + 0.5 * Grade));
    public int Grade => grade;
    public int Amount { get; set; }
    private new EggBase Base => baseData as EggBase;
    #endregion

    #region Fields
    private int grade;
    #endregion

    #region Unity
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Methods
    public void Init(int amount, int grade)
    {
        this.Amount = amount;
        this.grade = grade;
    }

    public override void Init() => Init(1, 0);

    public void Sell()
    {
        if (this.Amount <= 0) return;

        Debug.Log(ItemName + "을(를) 판매합니다.");
        Debug.Log($"판매 가격: {Price}원");
        GameObject.FindWithTag("Player").GetComponent<Player>().Golds += Price;
        GameManager.Instance.AnimalResult += Price;
        this.Amount--;
        if (this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
        UIManager.Instance.UpdateQuickslot();
    }
    #endregion
}