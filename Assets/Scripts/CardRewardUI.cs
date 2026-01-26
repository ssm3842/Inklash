using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRewardUI : MonoBehaviour
{
    [SerializeField]CardRewardCardUI[] cardRewardCardUIs;
    [SerializeField]BattleRewardController battleRewardController;

    public void ShowCardReward(List<CardDataSO> cardRewardList)
    {
        for(int i=0; i<3; i++)
        {
            cardRewardCardUIs[i].Setup(cardRewardList[i].card);
        }

        gameObject.SetActive(true);
    }
    void OnCardRewardSelected(CardRewardCardUI cardUI)
    {
        RunManager.Inst.deckManager.AddCardToDeck(cardUI.cardContent);

        battleRewardController.CardRewardAccepted();

        gameObject.SetActive(false);
    }
}
