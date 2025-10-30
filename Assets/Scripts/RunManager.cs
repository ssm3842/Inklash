using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Inst { get; private set; }
    void Awake()
    {
        Inst = this;
        // DontDestroyOnLoad(this);
    }
    [SerializeField] GameObject battle;
    [SerializeField] GameObject battleUICanvas;

    [SerializeField] MapManager mapManager;
    [SerializeField] MapDataGenerator mapDataGenerator;

    [SerializeField] BattleController battleController;
    void Start()
    {
        battle.SetActive(false);
        battleUICanvas.SetActive(false);

        mapManager.gameObject.SetActive(true);
    }

    public void SetupBattle() //TODO: 방정보 받기
    {
        mapManager.gameObject.SetActive(false);
        
        battle.SetActive(true);
        battleUICanvas.SetActive(true);

        battleController.StartBattle();
    }
}
