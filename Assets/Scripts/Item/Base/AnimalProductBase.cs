using UnityEngine;

[CreateAssetMenu(fileName = "New Animal Product Data", menuName = "Scriptable Object/ItemBase/Animal Product")]
public class AnimalProductBase : ItemBase
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