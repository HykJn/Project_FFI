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
                Debug.Log("해당 위치에 물을 줄 수 없습니다.");
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
                Debug.Log("해당 위치에서 물을 채울 수 없습니다.");
            }
        }
    }
    #endregion
}
