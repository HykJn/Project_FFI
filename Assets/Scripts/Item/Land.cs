using UnityEngine;

public class Land : Item, IAmount, IUsableItem
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
    public RuleTile Tile => Base.Land;
    public int Amount { get; set; }

    private new LandBase Base => baseData as LandBase;
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

    public void UseItem(Vector2Int pos, KeyCode key)
    {
        if (Amount <= 0) return;
        if (key == KeyCode.Mouse0)
        {
            if (TileManager.Instance.CreateLand(pos, Tile))
            {
                Amount--;
            }
            else
            {
                Debug.Log("�ش� ��ġ�� ����� ��ġ�� �� �����ϴ�.");
                return;
            }
        }
        else if (key == KeyCode.Mouse1)
        {
            if (TileManager.Instance.RemoveLand(pos))
            {
                Amount--;
            }
            else
            {
                Debug.Log("�ش� ��ġ�� ����� �����ϴ� �� �����߽��ϴ�.");
                return;
            }
        }

        if (Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }
    #endregion
}
