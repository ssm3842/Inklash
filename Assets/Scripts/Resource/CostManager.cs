using UnityEngine;

public class CostManager : MonoBehaviour
{
    public int currentCost;

    public float time = 0f;

    public void Init()
    {
        currentCost = 0;
    }

    void Update()
    {
        if (currentCost < GameRule.MAX_COST)
        {
            time += Time.deltaTime;

            if (time > GameRule.COST_GENERATE_SECOND)
            {
                AddCost();
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

    void AddCost()
    {
        currentCost = Mathf.Min(GameRule.MAX_COST, currentCost+1);
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
