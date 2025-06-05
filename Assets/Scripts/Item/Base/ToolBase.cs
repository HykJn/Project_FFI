using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Data", menuName = "Scriptable Object/ItemBase/Tool")]
public class ToolBase : ItemBase
{
    #region Properties
    public ToolType Type => type;
    public int MaxDurability => maxDurability;
    public int Price => price;
    #endregion

    #region PrivateFields
    [SerializeField] private ToolType type;
    [SerializeField] private int maxDurability;
    [SerializeField] private int price;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}