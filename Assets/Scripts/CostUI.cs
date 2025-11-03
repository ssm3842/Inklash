using TMPro;
using UnityEngine;

public class CostUI : MonoBehaviour
{
    [SerializeField] CostManager costManager;

    TextMeshProUGUI costText;

    void Start()
    {
        costText = GetComponent<TextMeshProUGUI>();
        costText.text = "Cost: 0 / 0";
    }

    void Update()
    {
        costText.text = "Cost: " + costManager.GetCurrentCost() + " / " + GameRule.MAX_COST;
    }
}
