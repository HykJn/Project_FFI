using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    #region Properties
    public abstract int Grown { get; protected set; }
    public ItemID ID => id;
    #endregion

    #region Fields
    [SerializeField] protected ItemID id;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Unity
    protected virtual void Awake()
    {
        anim = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Methods
    protected abstract void Grow();
    #endregion
}