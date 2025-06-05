using System;
using UnityEngine;

public class Seed : Item, IUsableItem, IAmount
{
    #region Properties
    public override ItemData Data
    {
        get => new(ID, Amount);
        set
        {
            if (value.itemID != ID) return;
            Amount = value.value1;
        }
    }
    
    //public GameObject Plant => Base.Plant;
    public ItemID PlantID => Base.PlantID;
    public int Amount { get; set; }

    private new SeedBase Base => baseData as SeedBase;
    #endregion

    #region PrivateFields
    #endregion

    #region Unity
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Methods
    public void Init(int amount)
    {
        this.Amount = amount;
    }

    public override void Init() => Init(1);

    public void UseItem(Vector2Int position, KeyCode key)
    {
        if (key != KeyCode.Mouse0 || Amount <= 0) return;

        if (TileManager.Instance.Plant(position, ObjectManager.Instance.GetPrefab(PlantID)))
        {
            this.Amount--;
            ActionID actionID = PlantID switch
            {
                ItemID.PLANT_CARROT => ActionID.PlantCarrot,
                ItemID.PLANT_EGGPLANT => ActionID.PlantEggplant,
            };
            QuestManager.OnQuestProceed?.Invoke(actionID, 1);
        }
        else
        {
            Debug.Log("해당 위치에 심을 수 없습니다.");
        }

        if(this.Amount <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }


    #endregion
}
