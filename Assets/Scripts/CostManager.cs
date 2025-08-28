using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CostManager : MonoBehaviour
{
    public static CostManager Inst;

    //Cost 관련 변수
    [SerializeField] int finalMaxCost;
    private int baseMaxCost = 10;
    private int CostModifier = 0;
    public int currentCost { get; private set; }

    public float time = 0f;
    public TextMeshProUGUI GameCostText;
    void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        currentCost = 0;
        UpdateMaxCost();
    }


    void Update()
    {
        GetCost();
    }

    // 3초마다 Cost 1 증가
    public void GetCost()
    {
        if (currentCost < finalMaxCost)
        {
            time += Time.deltaTime;

            if (time > GameRule.COST_GENERATE_SECOND)
            {
                currentCost++;
                time = 0f;
                IndicateCost();
            }
        }
        else
        {
            time = 0f;
        }
    }
    //space바 사용시 cost 1 감소
    public bool UseCost(int amount)
    {
        if (amount > currentCost) return false;
        else
        {
            currentCost -= amount;
            IndicateCost();
            return true;
        }
    }

    /*
    //카드에 적힌 cost 비용만큼 감소, 사용 불가시 false 반환
    public bool TryUseCard(Card card)
    {
        int cardCost = card.cardContent.cost;
        if (currentCost >= cardCost)
        {
            currentCost -= cardCost;
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    //max Cost 값 추가 및 감소
     public void AddMaxCostModifier(int amount)
    {
        CostModifier += amount;
        UpdateMaxCost();
    }

    public void RemoveMaxCostModifier(int amount)
    {
        CostModifier -= amount;
        UpdateMaxCost();
    }
    //Max Cost 값 적용
    private void UpdateMaxCost()
    {
        finalMaxCost = Mathf.Max(0,baseMaxCost + CostModifier);

        if (currentCost > finalMaxCost) //current와 Max값 비교 후 Max값으로
        {
            currentCost = finalMaxCost;
        }
        IndicateCost();
    }
    //Cost 값 출력 
    public void IndicateCost()
    {
        GameCostText.text = $"Cost : {currentCost} / {finalMaxCost}";
    }
}
