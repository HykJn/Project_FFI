using UnityEngine;

[CreateAssetMenu(fileName = "New Land Data", menuName = "Scriptable Object/ItemBase/Land")]
public class LandBase : ItemBase
{
    #region Fields
    public RuleTile Land => rTile_Land;
    #endregion

    #region Unity Methods
    [SerializeField] RuleTile rTile_Land;
    #endregion

    #region Methods
    
    #endregion
}
