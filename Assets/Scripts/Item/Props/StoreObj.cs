using UnityEngine;

public class StoreObj : PropObj, IIneteractable, IRemovable
{
    private Animator anim;
    [SerializeField] private int removeCnt = 3;
    private float recoverTick = 0f;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (removeCnt < 3)
        {
            recoverTick += Time.deltaTime;
            
            if (recoverTick >= 1f)
            {
                recoverTick = 0f;
                removeCnt = 3;
            }
        }
    }

    private void OnEnable()
    {
        this.enabled = true;
    }

    public void Interact()
    {
        UIManager.Instance.SwitchInGamePanel(InGamePanelType.Store);
    }

    public void Remove()
    {
        if(removeCnt > 1)
        {
            removeCnt--;
            anim.SetTrigger("Hit");
            recoverTick = 0;
            return;
        }
        GameObject temp = ObjectManager.Instance.GetInstance(ItemID.PROP_STORE_ITEM, true);
        temp.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}