using UnityEngine;

public class Mole : MonoBehaviour
{
    #region ==========Fields==========

    private Animator anim;
    private float hideTick = 0f;
    #endregion

    #region ==========Unity Events==========
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        hideTick += Time.deltaTime;
        if (hideTick >= 15f)
        {
            hideTick = 0f;
            Hide();
        }
    }

    private void OnEnable()
    {
        if (!anim) anim = GetComponent<Animator>();
        anim.Play("Show");
    }

    #endregion

    #region ==========Methods==========
    public void Hide()
    {
        anim.Play("Hide");
        TileManager.Instance.MoleSpawned = false;
    }
    #endregion
}
