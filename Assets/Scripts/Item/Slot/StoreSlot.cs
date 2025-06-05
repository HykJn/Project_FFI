using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : ItemSlot
{
    #region ==========Fields==========
    public ItemID ItemID => itemID;
    public int Price => price;
    public Text PriceText => priceText;

    [SerializeField] private ItemID itemID;
    [SerializeField] private Text priceText;
    [SerializeField] private int price;
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========
    #endregion
}
