using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    #region Properties
    public Item Item { get; set; }
    public Image Slot { get => slot; set => slot = value; }
    public Image Icon { get => icon; set => icon = value; }
    public GameObject AmountObj => amountObj;
    public GameObject DurabilityObj => durabilityObj;
    public int Amount { get => int.Parse(amount.text); set => amount.text = value.ToString(); }
    public float Durability { get => durability.fillAmount; set => durability.fillAmount = value; }
    public int Index { get; set; }
    #endregion

    #region Fields
    [SerializeField] private Image slot;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject amountObj;
    [SerializeField] private GameObject durabilityObj;
    [SerializeField] private Text amount;
    [SerializeField] private Image durability;
    #endregion
}
