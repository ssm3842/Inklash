using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Inst { get; private set; }
    void Awake()
    {
        Inst = this;
    }
    [SerializeField] GameObject battleUICanvas;

    [SerializeField] DeckSO startingDeckSO;
    
    // 체력, 돈 확인용 
    [SerializeField] private ResourceManager resourceManager;


    public MapManager mapManager;
    public DeckManager deckManager;
    public BattleManager battleManager;
    public UnitDataManager unitDataManager;

    int runGold;


    void Start()
    {
        battleManager.gameObject.SetActive(false);
        battleUICanvas.SetActive(false);

        mapManager.gameObject.SetActive(true);
    
        InitRun();

        // 체력, 돈 확인용 
        resourceManager.Init();
    }

    public void InitRun() //런 시작 시 게임 초기화.
    {
        mapManager.InitMapdata(); //맵 정보를 생성
        deckManager.InitDeck(startingDeckSO.startingDeck); //덱 정보를 시작 덱으로 초기화.
        unitDataManager.LoadCsvData(); //유닛 데이터를 csv에서 가져와 초기화.
    }
}
