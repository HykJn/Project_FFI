using UnityEngine;

[CreateAssetMenu(fileName = "New Prop Data", menuName = "Scriptable Object/ItemBase/Prop")]
public class PropBase : ItemBase
{
    #region Fields
    public ItemID PropID => propID;

    [SerializeField] private ItemID propID;
    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    #endregion
}
