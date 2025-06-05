using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour, IPanel
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

    private Player player = null;

    [Header("Volume")]
    [SerializeField] private Slider mainVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Text mainVolumeText;
    [SerializeField] private Text bgmVolumeText;
    [SerializeField] private Text sfxVolumeText;
    [Header("Child Panel")]
    [SerializeField] private GameObject[] childPanels;
    #endregion

    #region ==========Unity==========
    private void Update()
    {
        UpdateVolume();
    }
    #endregion

    #region ==========Methods==========
    public void UpdateVolume()
    {
        mainVolumeText.text = (mainVolume.value).ToString();
        bgmVolumeText.text = (bgmVolume.value).ToString();
        sfxVolumeText.text = (sfxVolume.value).ToString();

        SoundManager.Instance.MainVolume = mainVolume.value;
        SoundManager.Instance.BGMVolume = bgmVolume.value;
        SoundManager.Instance.SFXVolume = sfxVolume.value;
    }

    public void OpenPanel()
    {
        if (Enabled) return;
        this.gameObject.SetActive(true);

        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();

        player.IsMovable = false;
        player.IsInteractable = false;

        UIManager.Instance.ActivatedPanels.Add(this);
    }

    public void ClosePanel()
    {
        if (!Enabled) return;
        foreach (GameObject panel in childPanels)
        {
            IPanel child = panel.GetComponent<IPanel>();
            child.ClosePanel();
        }
        this.gameObject.SetActive(false);

        player.IsMovable = true;
        player.IsInteractable = true;

        UIManager.Instance.ActivatedPanels.Remove(this);
    }
    #endregion
}
