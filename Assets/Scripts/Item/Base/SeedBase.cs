using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Data", menuName = "Scriptable Object/ItemBase/Seed")]
public class SeedBase : ItemBase
{
    #region Properties
    //public GameObject Plant => prefab_Plant;
    public ItemID PlantID => plantID;
    #endregion

    #region PrivateFields
    //[SerializeField] private GameObject prefab_Plant;
    [SerializeField] private ItemID plantID;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}
