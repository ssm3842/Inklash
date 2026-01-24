using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRewardUI : MonoBehaviour
{
    bool isRewardSetted = false;

    [SerializeField]GameObject cardRewardUIPrefab;
    [SerializeField]GameObject cardRewardUIContainer;
    [SerializeField]CardDataLinkSO cardLinkSo;

    List<CardDataSO> rewards;
    List<CardDataSO> cardData;
    public void ShowCardReward()
    {
        //보상이 이미 설정된 상태라면 보여주기.
        if(isRewardSetted) gameObject.SetActive(true);
        else SetReward();
    }

    void SetReward()
    {   
        rewards = new List<CardDataSO>();
        cardData = new List<CardDataSO>();

        //보상 풀 설정
        foreach(CardLink data in cardLinkSo.playerUnits)
        {
            cardData.Add(data.cardContents);
        }
        foreach(CardLink data in cardLinkSo.playerSpells)
        {
            cardData.Add(data.cardContents);
        }
        foreach(CardLink data in cardLinkSo.playerWords)
        {
            cardData.Add(data.cardContents);
        }

        isRewardSetted = true;

        for(int i=0; i<3; i++)
        {
            int randomI = Random.Range(0, cardData.Count);

            GameObject newCardReward = Instantiate(cardRewardUIPrefab, cardRewardUIContainer.transform);

            CardDataSO currentCardData = cardData[randomI];
            rewards.Add(currentCardData);

            newCardReward.GetComponent<CardRewardCardUI>().Setup(currentCardData.card);

            newCardReward.GetComponent<Button>().onClick.AddListener(() => OnCardRewardSelected(currentCardData));

            gameObject.SetActive(true);
        }
    }

    void OnCardRewardSelected(CardDataSO cardData)
    {
        RunManager.Inst.deckManager.AddCardToDeck(cardData.card);
        isRewardSetted = false;
        gameObject.SetActive(false);
    }
}
