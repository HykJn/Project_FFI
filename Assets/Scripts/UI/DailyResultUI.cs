using UnityEngine;

public class DailyResultUI : MonoBehaviour, IPanel
{
    #region ==========Fields==========
    public bool Enabled
    {
        get => this.gameObject.activeSelf;
        set
        {
            if (value) OpenPanel();
            else ClosePanel();
        }
    }
    #endregion

    #region ==========Unity Events==========

    #endregion

    #region ==========Methods==========
    public void Continue()
    {
        GameManager.Instance.StartDay();
    }

    public void ExitGame()
    {
        GameManager.Instance.ToExit();
    }

    public void OpenPanel()
    {
        if (Enabled) return; 

        this.gameObject.SetActive(true);

        UIManager.Instance.ActivatedPanels.Add(this);
    }

    public void ClosePanel()
    {
        if (!Enabled) return;

        this.gameObject.SetActive(false);

        UIManager.Instance.ActivatedPanels.Remove(this);
    }
    #endregion
}
