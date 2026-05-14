using TMPro;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Inst { get; private set; }
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }
    [SerializeField] GameObject battleUICanvas;

    [SerializeField]GameObject bgmManager;
    [SerializeField]AudioClip battleBGM;

    [SerializeField]GameObject settingCavnasPrefab;

    [SerializeField] DeckSO debugDeckSO;

    public MapManager mapManager;
    public BattleManager battleManager;
    public UnitDataManager unitDataManager;
    public ResourceManager resourceManager;

    public GameObject cardRewardCanvas;
    public EventManager eventCanvas;
    public ShopEvent shopCanvas;

    void Start()
    {
        battleManager.gameObject.SetActive(false); //전투 비활성화

        //캔버스들을 미리 비활성화로 돌림.
        battleUICanvas.SetActive(false);
        eventCanvas.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(false);
        
        InitRun();
    }

    public void InitRun() //런 시작 시 게임 초기화.
    {
        if(DeckManager.Inst == null)
        {
            GameObject deckManagerOBJ = new GameObject();
            DeckManager deckManager = deckManagerOBJ.AddComponent<DeckManager>();
            deckManager.SetStartDeck(debugDeckSO);
            
            DontDestroyOnLoad(deckManagerOBJ);
        }

        if(SettingManger.Inst == null)
        {
            Instantiate(settingCavnasPrefab);
        }
        SettingManger.Inst.LoadSetting();
        SettingManger.Inst.CloseSetting();

        if(BGMManager.Inst == null)
        {
            Instantiate(bgmManager);
            BGMManager.Inst.PlayBGM(battleBGM, 0.1f);
        }
        else
        {
            BGMManager.Inst.ChangeBGM(battleBGM, 0.1f);
        }

        mapManager.InitMapdata(); //맵 정보를 생성
        unitDataManager.LoadCsvData(); //유닛 데이터를 csv에서 가져와 초기화.
        DeckManager.Inst.InitDeck(); //덱 정보를 시작 덱으로 초기화.
        resourceManager.Init(); // 체력, 돈 확인용
        mapManager.SetVisible();
    }
}
