using UnityEngine;

public class Hoe : Tool
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
        if (Durability <= 0) return;

        SoundManager.Instance.SFX_PlayOneShot(SFXID.Hoe);
        if (key == KeyCode.Mouse0)
        {
            if(TileManager.Instance.Plow(position))
            {
                this.Durability--;
            }
            else
            {
                Debug.Log("해당 위치를 경작할 수 없습니다.");
            }
        }
        else if (key == KeyCode.Mouse1)
        {
            if (TileManager.Instance.Harvest(position))
            {
                this.Durability--;
            }
            else
            {
                Debug.Log("해당 위치의 작물을 수확할 수 없습니다.");
            }
        }

        if (this.Durability <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }
    #endregion
}
