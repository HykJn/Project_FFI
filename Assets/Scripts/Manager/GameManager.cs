using System;
using System.Threading.Tasks;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //
    //Event for end day
    public event Action OnEndDay = null;
    #region Properties
    //Singleton instance
    public static GameManager Instance => instance;

    //Time System
    public float CurTime => curTime;
    public GameState CurrentState => gameState;
    public Timezone CurrentTimezone => timezone;

    //Playing flag
    public bool IsPlaying => isPlaying;

    //Golds for result
    public int CurDay => curDay;
    public int FarmingResult { get; set; } = 0;
    public int AnimalResult { get; set; } = 0;
    public int ExpenseResult { get; set; } = 0;
    public int MiscResult { get; set; } = 0;
    public int TotalResult
    {
        get
        {
            return FarmingResult + AnimalResult + ExpenseResult;
        }
    }
    #endregion

    #region Fields
    //Singleton instance
    private static GameManager instance = null;

    //For control game sequance
    private GameState gameState;
    private Timezone timezone;
    private bool isPlaying = false;
    //Time System
    [Header("Time")]
    public const int MINUITES_OF_DAY = 1080; //Day + Noon + Night (each 360min)
    private float minuitesPerDay = 5;
    private bool timerPause = false;
    private float curTime = 0;
    private int curDay = 0;
    [SerializeField] private float timerTick = 0;

    //Light
    [Header("Light")]
    [SerializeField] private Light2D globalLight;
    private readonly Color DAY_COLOR = Color.white;
    private readonly Color NOON_COLOR = new Color(255, 140, 75, 255) / 255f;
    private readonly Color NIGHT_COLOR = new Color(45, 45, 45, 255) / 255f;

    //Loading
    [Header("Loading")]
    [SerializeField] private LoadingUI loadingUI;

    [Header("New Game")]
    private int startGolds = 500;
    #endregion

    #region Unity
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        GameStateBehaviour();
    }
    #endregion

    #region Methods
    void Init()
    {

    }

    //SceneManagement
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = false;

        while (!loadScene.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadSceneAsync(int buildIndex, Action afterWork)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(buildIndex);
        loadingUI.gameObject.SetActive(true);
        while (!loadScene.isDone)
        {
            loadingUI.SetLoadingProgress(loadScene.progress);
            yield return null;
        }

        loadingUI.SetLoadingProgress(1f);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        afterWork();
        loadingUI.gameObject.SetActive(false);
    }

    //Game State
    public void ToTitle()
    {
        SoundManager.Instance.BGM_Play(BGMID.Title);
    }

    public void ToPlayingNewFile()
    {
        StartCoroutine(LoadSceneAsync(1, PlayNewFileCallback));
    }

    public void ToPlayingLoad(GameObject failToLoadPanel)
    {
        if (!DataManager.CheckDataExists())
        {
            failToLoadPanel.SetActive(true);
            return;
        }
        StartCoroutine(LoadSceneAsync(1, PlayLoadCallback));
    }

    public void PlayNewFileCallback()
    {
        DataManager.DeleteData();
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.Golds = startGolds;
        player.IsMovable = false;
        player.IsInteractable = false;
        UIManager.Instance.TutoCanvas.gameObject.SetActive(true);
        
        WhenPlayingSceneDone();
    }

    public void PlayLoadCallback()
    {
        LoadData();
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.IsMovable = true;
        player.IsInteractable = true;
        UIManager.Instance.TutoCanvas.gameObject.SetActive(false);
        WhenPlayingSceneDone();
    }

    public void WhenPlayingSceneDone()
    {
        if (globalLight == null)
        {
            GameObject lightObj = GameObject.FindWithTag("Light2D");
            if (lightObj != null)
            {
                globalLight = lightObj.GetComponent<Light2D>();
            }
        }

        gameState = GameState.Playing;
        isPlaying = true;

        SoundManager.Instance.BGM_Play(BGMID.Day0);
        SoundManager.Instance.BGMAutoPlay = true;
    }

    public void ToPause()
    {
        Time.timeScale = 0;
        isPlaying = false;
        timerPause = true;
    }

    public void ToExit()
    {
        //Do something
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }

    void OnTitle()
    {

    }

    void OnPlaying()
    {
        GameSequance();
    }

    void OnPause()
    {
    }

    void OnExit()
    {
        //Do something
    }

    void GameStateBehaviour()
    {
        switch (gameState)
        {
            case GameState.Title:
                OnTitle();
                break;
            case GameState.Playing:
                OnPlaying();
                break;
            case GameState.Pause:
                OnPause();
                break;
            case GameState.Exit:
                OnExit();
                break;
        }
    }


    //Game Sequance
    void GameSequance()
    {
        if (!timerPause)
        {
            timerTick += Time.deltaTime;
        }
        if (timerTick >= 30f)
        {
            timerTick = 0;
            curTime += MINUITES_OF_DAY / minuitesPerDay;
            if (UIManager.Instance != null) UIManager.Instance.UpdateTime((int)curTime, MINUITES_OF_DAY);
        }


        if (curTime >= 1080)
        {
            EndDay();
        }
        else if (curTime >= 720)
        {
            Night();
        }
        else if (curTime >= 360)
        {
            Noon();
        }
        else if (curTime >= 0)
        {
            Morning();
        }
    }

    public void StartDay()
    {
        if (curTime != -1) return;
        //Init day
        timerPause = false;
        timerTick = 0;
        curTime = 0;

        FarmingResult = 0;
        AnimalResult = 0;
        ExpenseResult = 0;
        MiscResult = 0;

        TileManager.Instance.Init();
        UIManager.Instance.UpdateTime((int)curTime, MINUITES_OF_DAY);
    }

    void Morning()
    {
        //On Morning
        timezone = Timezone.Morning;
        globalLight.color = DAY_COLOR;
    }

    void Noon()
    {
        //On Noon
        timezone = Timezone.Noon;
        globalLight.color = Color.Lerp(DAY_COLOR, NOON_COLOR, (curTime - 360) / 360f);
    }

    void Night()
    {
        //On Night
        timezone = Timezone.Night;
        globalLight.color = Color.Lerp(NOON_COLOR, NIGHT_COLOR, (curTime - 720) / 360f);
    }

    void EndDay()
    {
        //On Sleep or Slept
        timerPause = true;
        curTime = -1;
        curDay += 1;
        OnEndDay?.Invoke();
        UIManager.Instance.DailyResult();
    }

    //Others
    public void FastFoward()
    {
        if (!timerPause)
            timerTick += 60;
    }

    //Settings
    public void SetMinuitePerDay(float minuitePerDay) => this.minuitesPerDay = minuitePerDay;

    public void SaveData()
    {
        //Set player data
        PlayerData pData = new(GameObject.FindWithTag("Player").GetComponent<Player>());

        //Set scene data
        SceneData scData = new(GameObject.FindGameObjectsWithTag("Item"));

        //Set system data
        SystemData stData = new(curDay);

        SaveData data = new(pData, scData, stData);
        DataManager.SaveData(data);
    }

    public void LoadData()
    {
        SaveData data = DataManager.LoadData();

        //Set player data
        PlayerData pData = data.playerData;
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.transform.position = pData.position;
        for (int i = 0; i < 9; i++)
        {
            GameObject item = ObjectManager.Instance.GetInstance(pData.quickslot[i].itemID, false);
            if (item == null)
            {
                player.Quickslot[i] = null;
            }
            else
            {
                player.Quickslot[i] = item.GetComponent<Item>();
                player.Quickslot[i].Data = pData.quickslot[i];
            }
        }
        for (int i = 0; i < 36; i++)
        {
            GameObject item = ObjectManager.Instance.GetInstance(pData.inventory[i].itemID, false);
            if (item == null)
            {
                player.Inventory[i / 9, i % 9] = null;
            }
            else
            {
                player.Inventory[i / 9, i % 9] = item.GetComponent<Item>();
                player.Inventory[i / 9, i % 9].Data = pData.inventory[i];
            }
        }
        player.Health = pData.health;
        player.Stamina = pData.stamina;
        player.Golds = pData.golds;

        //Set scene data
        SceneData scData = data.sceneData;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (obj.TryGetComponent<Item>(out Item item))
            {
                obj.SetActive(false);
            }
        }
        for (int i = 0; i < scData.objects.Length; i++)
        {
            ItemPositionData ipData = data.sceneData.objects[i];
            GameObject obj = ObjectManager.Instance.GetInstance(ipData.itemData.itemID, true);
            obj.transform.position = ipData.position;
            if (obj.TryGetComponent<Item>(out Item item))
            {
                item.Data = ipData.itemData;
            }
        }

        SystemData sData = data.systemData;
        curDay = sData.curDay;

        //Update UI
        UIManager.Instance.UpdateQuickslot();
        UIManager.Instance.UpdateTime((int)curTime, MINUITES_OF_DAY);
        UIManager.Instance.UpdateGolds(player.Golds);
    }
    #endregion
}
