using UnityEngine;

[CreateAssetMenu(fileName = "New Chicken Egg Data", menuName = "Scriptable Object/ItemBase/Chicken Egg")]
public class ChickenEggBase : ItemBase
{
    #region ==========Fields==========
    public ItemID AnimalID => animalID;
    [SerializeField] private ItemID animalID;
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========

    #endregion
}
