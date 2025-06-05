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
        Debug.Log(ItemName + "��/�� �Ծ����ϴ�!");
        Debug.Log($"ü���� {HealthAmount * (1 + (grade * 0.5f))}��ŭ ȸ���߽��ϴ�!");
        Debug.Log($"����� {StaminaAmount * (1 + (grade * 0.5f))}��ŭ ȸ���߽��ϴ�!");

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
        

        Debug.Log(ItemName + "��(��) �Ǹ��մϴ�.");
        Debug.Log($"�Ǹ� ����: {Price}��");
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
