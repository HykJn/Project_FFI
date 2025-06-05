using UnityEngine;

public class WateringCan : Tool
{
    #region Fields
    public override int Price => Base.Price;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Methods
    public override void UseItem(Vector2Int position, KeyCode key)
    {
        if (Durability <= 0) return;

        if (key == KeyCode.Mouse0)
        {
            if (TileManager.Instance.Watering(position))
            {
                this.Durability--;
            }
            else
            {
                Debug.Log("�ش� ��ġ�� ���� �� �� �����ϴ�.");
            }
        }
        else if (key == KeyCode.Mouse1)
        {
            if (TileManager.Instance.FillWater(position))
            {
                this.Durability = MaxDurability;
            }
            else
            {
                Debug.Log("�ش� ��ġ���� ���� ä�� �� �����ϴ�.");
            }
        }
    }
    #endregion
}
