using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Data", menuName = "Scriptable Object/ItemBase/Plant")]
public class PlantBase : ItemBase
{
    #region Properties
    //public GameObject Crop => prefab_Crop;
    public ItemID CropID => cropID;
    public Sprite[] SpritesEachSteps => spritesEachSteps;
    public int MaxDropAmount => maxDropAmount;
    public int MinDropAmount => minDropAmount;
    #endregion

    #region PrivateFields
    //[SerializeField] private GameObject prefab_Crop;
    [SerializeField] private ItemID cropID;
    [SerializeField] private Sprite[] spritesEachSteps;
    [SerializeField] private int maxDropAmount;
    [SerializeField] private int minDropAmount;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}