using UnityEngine;

public class Axe : Tool
{
    #region Fields

    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Methods
    public override void UseItem(Vector2Int position, KeyCode key)
    {
        if (this.Durability <= 0) return;

        SoundManager.Instance.SFX_PlayOneShot(SFXID.Hoe);
        if (key == KeyCode.Mouse0)
        {
            if (TileManager.Instance.Axing(position))
            {
                this.Durability--;
            }
            else
            {
                Debug.Log("해당 위치의 나무를 베는 데 실패했습니다.");
            }
        }
        else if (key == KeyCode.Mouse1)
        {
            
        }

        if (this.Durability <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }

    }
    #endregion
}
