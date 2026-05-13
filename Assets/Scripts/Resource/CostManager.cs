using UnityEngine;

public class CostManager : MonoBehaviour
{
    [SerializeField]CostBackground costBackground;
    public int currentCost;

    public float time = 0f;
    float currentRegenTime;

    public void Init()
    {
        currentCost = 0;
        time = 0f;

        int deckSize = DeckManager.Inst.GetDeckdata().Count;
        currentRegenTime = GameRule.GetCostRegenTime(deckSize);

        int tier = GameRule.GetCostRegenTier(deckSize);
        Debug.Log($"[CostManager] 덱 {deckSize}장, 단계 {tier}, 재생 {currentRegenTime}초");
    }

    void Update()
    {
        if (currentCost < GameRule.MAX_COST)
        {
            time += Time.deltaTime;

            if (time > currentRegenTime)
            {
                AddCost(1);
                time = 0f;
            }
        }
        else
        {
            time = 0f;
        }
    }

    public bool CheckUseCostAvailable(int amount)
    {
        return currentCost >= amount;
    }

    public void AddCost(int amount)
    {
        costBackground.OnCostChanged();
        currentCost = Mathf.Min(GameRule.MAX_COST, currentCost + amount);
    }

    public void UseCost(int amount)
    {
        currentCost = Mathf.Max(0, currentCost - amount);
    }
    public int GetCurrentCost()
    {
        return currentCost;
    }

}
