using System;
using UnityEngine;

public class Plant : Item
{
    #region Properties
    public override ItemData Data
    {
        get => new(ID, Growth);
        set
        {
            if (value.itemID != ID) return;
            if (value.value1 < 0 || value.value1 > 3) return;
            Growth = value.value1;
        }
    }
    //public GameObject Crop => Base.Crop;
    public ItemID CropID => Base.CropID;
    public Sprite[] SpritesEachSteps => Base.SpritesEachSteps;
    public int MaxDropAmount => Base.MaxDropAmount;
    public int MinDropAmount => Base.MinDropAmount;
    public int Growth { get; set; }
    public int MaxGrowth => Base.MaxGrowth;
    [Obsolete] public bool Witherable { get; set; } = false;
    public int DropAmount { get; set; }

    private new PlantBase Base => baseData as PlantBase;
    #endregion

    #region PrivateFields
    #endregion

    #region Unity
    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEndDay -= Grow;
    }
    #endregion

    #region Methods
    public void Init(int growth)
    {
        this.Growth = growth;
        this.GetComponent<SpriteRenderer>().sprite = SpritesEachSteps[0];
        GameManager.Instance.OnEndDay += Grow;
    }

    public override void Init() => Init(0);

    void Grow()
    {
        if (TileManager.Instance.Growable(Vector2Int.RoundToInt(this.transform.position)))
        {
            Growth = Mathf.Clamp(Growth + 1, 0, 3);
            this.GetComponent<SpriteRenderer>().sprite = SpritesEachSteps[Growth];
        }
        else Wither();
    }

    public void Wither()
    {
        this.gameObject.SetActive(false);
        TileManager.Instance.Wither(Vector2Int.RoundToInt(this.transform.position));
    }

    public void SetGrade(Crop crop)
    {
        //TODO: Implement logic to set the grade of the crop based on Hoe's stat
    }

    public void DropCrops()
    {
        if (Growth < MaxGrowth) return;

        //Set random drop amount
        DropAmount = UnityEngine.Random.Range(MinDropAmount, MaxDropAmount + 1);

        //Drop
        for (int i = 0; i < DropAmount; i++)
        {
            GameObject temp = ObjectManager.Instance.GetInstance(CropID, true);
            temp.transform.position = this.transform.position;

            Vector2 randPos = TileManager.Instance.GetRandomPos(Vector2Int.RoundToInt(this.transform.position));            
            temp.GetComponent<MonoBehaviour>().StartCoroutine(TileManager.Instance.DropItemSpread(temp, randPos));

            SetGrade(temp.GetComponent<Crop>());
        }

        if (ID == ItemID.PLANT_CORN)
        {
            Growth--;
            this.GetComponent<SpriteRenderer>().sprite = SpritesEachSteps[Growth];
        }
        else Wither();
    }
    #endregion
}
