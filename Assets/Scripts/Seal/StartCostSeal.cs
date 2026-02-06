using UnityEngine;

public class StartManaSeal : UnitSeal
{
    private void Start()
    {
        // 소환 직후 마나 2 증가
        CostManager cm = FindFirstObjectByType<CostManager>();
        if (cm != null)
        {
            cm.AddCost(2);
        }
    }
}