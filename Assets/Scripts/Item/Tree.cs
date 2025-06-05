using UnityEngine;

public class Tree : Item
{
    #region ==========Fields==========
    public override ItemData Data { get; set; }

    private int chopCount;
    private float resetTick = 0;
    private new ItemBase_Default Base => baseData as ItemBase_Default;
    #endregion

    #region ==========Unity Methods==========
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
    }

    public void Chop()
    {
        this.chopCount--;
        resetTick = 0;
        if (chopCount <= 0)
        {
            int rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                GameObject obj = ObjectManager.Instance.GetInstance(ItemID.WOOD_BRANCH);
                obj.transform.position = this.transform.position;
            }
            rand = Random.Range(2, 4);
            for (int i = 0; i < rand; i++)
            {
                GameObject obj = ObjectManager.Instance.GetInstance(ItemID.WOOD_LOG);
                obj.transform.position = this.transform.position;
            }
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
