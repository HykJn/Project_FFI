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
                Debug.Log("�ش� ��ġ�� ������ �� �����ϴ�.");
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
                Debug.Log("�ش� ��ġ�� �۹��� ��Ȯ�� �� �����ϴ�.");
            }
        }

        if (this.Durability <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().RemoveItem(this);
        }
    }
    #endregion
}
