using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Egg Data", menuName = "Scriptable Object/ItemBase/Spawn Egg")]
public class SpawnEggBase : ItemBase
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
