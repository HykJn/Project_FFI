using System;
using UnityEngine;

public class Crop : Item, IUsableItem, IEquatable<Crop>, IAmount, ISellable, IGrade
{
    #region Properties
    public override ItemData Data
    {
        get => new(ID, Amount, Grade);
        set
        {
            if (value.itemID != ID) return;
            if (value.value1 < 0) value.value1 = 0;
            value.value2 = Mathf.Clamp(value.value2, 0, 3);
            Amount = value.value1;
            grade = value.value2;
        }
    }
    public int HealthAmount => Base.HealthAmount;
    public int StaminaAmount => Base.StaminaAmount;
    public int Amount { get; set; }
    public int Grade => grade;
    public int Price => (int)(Base.Price * (1 + Grade * 0.5f));
    private new CropBase Base => baseData as CropBase;
    #endregion

    #region PrivateFields
    [SerializeField] private int grade = 0;
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

    public void UseItem(Vector2Int position, KeyCode key)
    {
        if (key != KeyCode.Mouse0) return;

        if (Amount <= 0) return;
        //TODO: Log to UI
        Debug.Log(ItemName + "을/를 먹었습니다!");
        Debug.Log($"체력을 {HealthAmount * (1 + (grade * 0.5f))}만큼 회복했습니다!");
        Debug.Log($"기력을 {StaminaAmount * (1 + (grade * 0.5f))}만큼 회복했습니다!");

        this.Amount--;

        if(this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }

    public bool Equals(Crop other)
    {
        return other.Base == this.Base && other.Grade == this.Grade;
    }

    public override bool Equals(Item other) => this.Equals(other as Crop);

    public void Sell()
    {
        if (this.Amount <= 0) return;
        

        Debug.Log(ItemName + "을(를) 판매합니다.");
        Debug.Log($"판매 가격: {Price}원");
        GameObject.FindWithTag("Player").GetComponent<Player>().Golds += Price;
        GameManager.Instance.FarmingResult += Price;
        this.Amount--;
        if(this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
        UIManager.Instance.UpdateQuickslot();
    }
    #endregion
}
