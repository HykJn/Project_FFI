using UnityEngine;

[CreateAssetMenu(fileName = "New Egg Data", menuName = "Scriptable Object/ItemBase/Egg")]
public class EggBase : ItemBase
{
    #region Properties
    public int MaxStackAmount => maxStackAmount;
    public int Price => price;
    #endregion

    #region PrivateFields
    [SerializeField] private int maxStackAmount;
    [SerializeField] private int price;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}