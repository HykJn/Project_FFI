using System;
using UnityEngine;

public abstract class Item : MonoBehaviour, IEquatable<Item>
{
    #region Properties
    public abstract ItemData Data { get; set; }
    public ItemID ID => Base.ID;
    public string ItemName => Base.ItemName;
    public string Description => Base.Description;
    public Sprite Sprite => Base.Sprite;

    protected ItemBase Base => baseData;
    #endregion

    #region PrivateFields
    [SerializeField] private protected ItemBase baseData;
    #endregion

    #region Unity
    #endregion

    #region Methods
    public abstract void Init();

    public virtual bool Equals(Item other)
    {
        if (other == null) return false;
        if (other.Base != this.Base) return false;
        if (other.GetType() != this.GetType()) return false;
        return true;
    }
    #endregion
}
