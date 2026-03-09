using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCardEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]Transform cardRewardContainer;

    [SerializeField]Button confirmButton;

    CardRewardCardUI rewardCard;
    public void SetEvent()
    {
        rewardCard = null;

        confirmButton.interactable = false;

        List<CardDataSO> cardRewardList = new List<CardDataSO>();
        List<CardDataSO> allCardRewardPool = RunManager.Inst.unitDataManager.GetCardRewardPool();
        for(int i=0; i<3; i++)
        {
            int randomI = Random.Range(0, allCardRewardPool.Count);

            CardDataSO currentCardData = allCardRewardPool[randomI];
            cardRewardList.Add(currentCardData);
        }

        //유닛, 마법 카드를 표시할 캔버스를 먼저 초기화
        foreach (Transform child in cardRewardContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(CardDataSO cardDataSO in cardRewardList)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardRewardContainer);
            cardUI.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cardUI.GetComponent<CardRewardCardUI>().Setup(cardDataSO.card);
            cardUI.GetComponent<Button>().onClick.AddListener(() => selectCard(cardUI.GetComponent<CardRewardCardUI>()));
        }
    }

    void selectCard(CardRewardCardUI card)
    {
        foreach(Transform child in cardRewardContainer)
        {
            child.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
        card.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        rewardCard = card;
        confirmButton.interactable = true;
    }

    public void GetCard()
    {
        DeckManager.Inst.AddCardToDeck(rewardCard.cardContent);

        eventManager._OnEventEnd();
    }
}
