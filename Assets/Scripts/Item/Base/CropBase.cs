using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Scriptable Object/ItemBase/Crop")]
public class CropBase : ItemBase
{
    #region Properties
    public int HealthAmount => healthAmount;
    public int StaminaAmount => staminaAmount;
    public int Price => price;
    #endregion

    #region PrivateFieldsss
    [SerializeField] private int healthAmount;
    [SerializeField] private int staminaAmount;
    [SerializeField] private int price;
    #endregion

    #region Unity
    #endregion

    #region Methods
    #endregion
}