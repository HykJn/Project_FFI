using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    #region Properties
    public ItemID ID => id;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Sprite => itemSprite;
    #endregion

    #region PrivateFields
    [SerializeField] private ItemID id;
    [SerializeField] private string itemName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite itemSprite;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}