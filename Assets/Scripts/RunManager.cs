using TMPro;
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


    public MapManager mapManager;
    public DeckManager deckManager;
    public BattleManager battleManager;
    public UnitDataManager unitDataManager;
    public ResourceManager resourceManager;

    public RandomEventCanvas randomEventCanvas;
    public GameObject cardRewardCanvas;
    public GameObject campfireCanvas;
    public GameObject placeholderCanvas;

    int runGold;


    void Start()
    {
        battleManager.gameObject.SetActive(false); //전투 비활성화

        //캔버스들을 미리 비활성화로 돌림.
        battleUICanvas.SetActive(false);   
        randomEventCanvas.gameObject.SetActive(false);
        campfireCanvas.SetActive(false);
        placeholderCanvas.SetActive(false);
        
        InitRun();
    }

    public void InitRun() //런 시작 시 게임 초기화.
    {
        mapManager.InitMapdata(); //맵 정보를 생성
        unitDataManager.LoadCsvData(); //유닛 데이터를 csv에서 가져와 초기화.
        deckManager.InitDeck(startingDeckSO.startingDeck); //덱 정보를 시작 덱으로 초기화.
        resourceManager.Init(); // 체력, 돈 확인용
        mapManager.SetVisible();
    }
}
