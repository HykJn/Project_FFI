using UnityEngine;

[CreateAssetMenu(fileName = "New Tree Data", menuName = "Scriptable Object/ItemBase/Tree")]
public class TreeBase : ItemBase
{
    #region ==========Fields==========
    public ItemID[] DropItems => dropItemID;
    [SerializeField] ItemID[] dropItemID;
    #endregion

    #region ==========Unity Methods==========
    
    #endregion

    #region ==========Methods==========
    
    #endregion
}
