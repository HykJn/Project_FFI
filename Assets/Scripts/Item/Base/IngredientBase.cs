using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient Data", menuName = "Scriptable Object/ItemBase/Ingredient")]
public class IngredientBase : ItemBase
{
    #region ==========Fields==========
    public int Price => price;
    [SerializeField] private int price;
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========

    #endregion
}
