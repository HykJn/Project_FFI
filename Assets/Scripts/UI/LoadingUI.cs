using UnityEngine;
using UnityEngine.UI;   

public class LoadingUI : MonoBehaviour
{
    #region ==========Fields==========
    //[SerializeField] private Animator anim;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Text tipsText;
    [SerializeField] private Text nowLoadingText;
    [SerializeField] private Text pressAnyKeyText;
    [SerializeField, TextArea] private string[] tips;

    [SerializeField] private Image[] images;
    private Text[] texts;

    float tipChangeTick = 0, tipTextTick = 0, nowLoadingTick = 0;
    int tipIdx = 0, tipStringIdx = 0, dotsCount = 0;

    #endregion

    #region ==========Unity Methods==========
    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    private void Update()
    {
        TipsOnLoading();
    }
    #endregion

    #region ==========Methods==========
    public void SetLoadingProgress(float progress)
    {
        loadingBar.fillAmount = progress;
        if (progress >= 1f)
        {
            pressAnyKeyText.gameObject.SetActive(true);
        }
    }

    public void TipsOnLoading()
    {
        tipChangeTick += Time.deltaTime;
        nowLoadingTick += Time.deltaTime;

        if (nowLoadingTick >= 0.3f)
        {
            nowLoadingTick = 0;
            nowLoadingText.text = "Now Loading" + new string('.', 1 + (dotsCount++ % 3));
        }

        if (tipChangeTick <= 5f)
        {
            tipTextTick += Time.deltaTime;
            if (tipTextTick >= 0.1f)
            {
                if (tipStringIdx >= tips[tipIdx].Length) return;
                tipsText.text += tips[tipIdx][tipStringIdx++];
                tipTextTick = 0;
            }
        }
        else
        {
            tipIdx = (tipIdx + 1) % tips.Length;
            tipStringIdx = 0;
            tipsText.text = "";
            tipChangeTick = 0;
            tipTextTick = 0;
        }
    }

    
    #endregion
}
