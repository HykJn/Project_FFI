using UnityEngine;

public class Tree : Item
{
    #region ==========Fields==========
    public override ItemData Data { get; set; }

    private ItemID[] DropItems => Base.DropItems;
    private int chopCount;
    private float resetTick = 0;
    private new TreeBase Base => baseData as TreeBase;
    #endregion

    #region ==========Unity Methods==========
    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (chopCount < 3)
        {
            resetTick += Time.deltaTime;
            if (resetTick >= 5f) // Reset after 5 seconds
            {
                chopCount = 3; // Reset chop count
                resetTick = 0;
            }
        }
    }
    #endregion

    #region ==========Methods==========
    public override void Init()
    {
        chopCount = 3;
        Vector3 origin = this.transform.position;
        this.transform.position = new Vector3(origin.x, origin.y, origin.y / 100f);
    }

    public void Chop()
    {
        this.chopCount--;
        resetTick = 0;
        this.GetComponent<Animator>().SetTrigger("Hit");
        if (chopCount <= 0)
        {
            for (int i = 0; i < DropItems.Length; i++)
            {
                int dropAmount = Random.Range(1, 4);
                for (int j = 0; j < dropAmount; j++)
                {
                    GameObject obj = ObjectManager.Instance.GetInstance(DropItems[i]);
                    obj.transform.position = this.transform.position;

                    Vector2 randPos = TileManager.Instance.GetRandomPos(Vector2Int.RoundToInt(this.transform.position));
                    obj.GetComponent<MonoBehaviour>().StartCoroutine(TileManager.Instance.DropItemSpread(obj, randPos));
                }
            }
            TileManager.Instance.TreeCount--;
            TileManager.Instance.RemoveProp(Vector2Int.RoundToInt(this.transform.position), 0);
        }
    }
    #endregion
}
