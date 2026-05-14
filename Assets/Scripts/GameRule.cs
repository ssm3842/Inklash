using UnityEngine;

public class GameRule : MonoBehaviour
{
    public static int MAX_HAND_CARD_NUM = 5;

    public static float COST_GENERATE_SECOND = 1.5f;
    public const float COST_REGEN_TIER_STEP = 0.2f;
    public static readonly int[] COST_REGEN_TIER_THRESHOLDS = {10, 12, 15, 19};
    public static int MAX_COST = 10;
    public static float REROLL_WAIT_SECOND = 15f;

    public static float GetCostRegenTime(int deckSize)
    {
        float regen = COST_GENERATE_SECOND;
        foreach (int threshold in COST_REGEN_TIER_THRESHOLDS)
        {
            if (deckSize >= threshold) regen -= COST_REGEN_TIER_STEP;
        }
        return regen;
    }

    public static int GetCostRegenTier(int deckSize)
    {
        int tier = 0;
        foreach (int threshold in COST_REGEN_TIER_THRESHOLDS)
        {
            if (deckSize >= threshold) tier++;
        }
        return tier;
    }
}
