using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEvent : MonoBehaviour
{
    [SerializeField] GameObject useCardContainer;
    [SerializeField] GameObject wordCardContainer;

    [SerializeField] TextMeshProUGUI[] cardTextContainer;

    [SerializeField] GameObject cardPrefab;


    public void EnterShop()
    {
        //기존에 있던 유닛, 마법 카드 오브젝트 삭제.
        foreach (Transform child in useCardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        //유닛, 마법 카드만 골라 표시.
        List<CardContent> cardList = RunManager.Inst.unitDataManager.GetShopUseCardPool();
        for(int i = 0; i < 4; i++)
        {
            int cardIndex = i;
            int newCardCost = UnityEngine.Random.Range(60, 80);

            cardTextContainer[i].text = newCardCost.ToString();

            GameObject cardUI = Instantiate(cardPrefab, useCardContainer.transform);
            CardContent targetCard = cardList[UnityEngine.Random.Range(0, cardList.Count)];

            cardUI.GetComponent<CardRewardCardUI>().Setup(targetCard);
            cardUI.GetComponent<Button>().onClick.AddListener(() => BuyCard(newCardCost, targetCard, cardUI.GetComponent<CanvasGroup>(), cardIndex));
        }
        
        //기존에 있던 단어 카드 오브젝트 삭제.
        foreach (Transform child in wordCardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        //유닛, 마법 카드만 골라 표시.
        cardList = RunManager.Inst.unitDataManager.GetShopWordCardPool();
        for(int i = 4; i < 6; i++)
        {
            int cardIndex = i;
            int newCardCost = UnityEngine.Random.Range(100, 120);

            cardTextContainer[i].text = newCardCost.ToString();

            GameObject cardUI = Instantiate(cardPrefab, wordCardContainer.transform);
            CardContent targetCard = cardList[UnityEngine.Random.Range(0, cardList.Count)];
            
            cardUI.GetComponent<CardRewardCardUI>().Setup(targetCard);
            cardUI.GetComponent<Button>().onClick.AddListener(() => BuyCard(newCardCost, targetCard, cardUI.GetComponent<CanvasGroup>(), cardIndex));
        }

        gameObject.SetActive(true);
    }

    void BuyCard(int cost, CardContent cardData, CanvasGroup cardObject, int index)
    {
        if(RunManager.Inst.resourceManager.SpendGold(cost))
        {
            RunManager.Inst.deckManager.AddCardToDeck(cardData);

            cardObject.interactable = false;
            cardObject.alpha = 0;
            cardObject.blocksRaycasts = false;

            cardTextContainer[index].text = "";
        }
    }
}
