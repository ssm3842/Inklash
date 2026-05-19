using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddCardEvent : MonoBehaviour
{
    [SerializeField]GameObject cardPrefab;

    [SerializeField]Transform cardRewardContainer;

    [SerializeField]Button rerollButton;
    [SerializeField]Button getCardButton;
    [SerializeField]TextMeshProUGUI eventCostText;
    int eventRepeat;

    CardRewardCardUI rewardCard;
    public void SetEvent()
    {
        rewardCard = null;

        eventRepeat = 0;
        rerollButton.interactable = CheckEventAvailable();
        getCardButton.interactable = false;
    }

    public void GetNewRandomCards()
    {
        cardRewardContainer.gameObject.SetActive(true);

        List<CardDataSO> cardRewardList = new List<CardDataSO>();
        List<CardDataSO> allCardRewardPool = new List<CardDataSO>(RunManager.Inst.unitDataManager.GetCardRewardPool());
        for(int i=0; i<3; i++)
        {
            int randomI = UnityEngine.Random.Range(0, allCardRewardPool.Count);

            CardDataSO originalCardData = allCardRewardPool[randomI];
            CardDataSO instantiatedCardData = Instantiate(originalCardData);

            cardRewardList.Add(instantiatedCardData);
            allCardRewardPool.Remove(originalCardData); //선택지에 같은 카드가 나오는 것 방지.
        }

        //5층 이상부터는 강화 카드가 보상으로 나올 수 있음.
        if(RunManager.Inst.mapManager.floorClimbed >= 5)
        {
            foreach(CardDataSO card in cardRewardList)
            {
                //단어카드는 강화 없음
                if(card.card.cardType == CardType.Word) continue;

                //강화 카드 등장 확률 기본 10% + 클리어한 층마다 5% 추가.
                if(UnityEngine.Random.Range(0, 100) < (10 + RunManager.Inst.mapManager.floorClimbed * 5))
                {
                    //강화가 여러번 될 확률.
                    int enchantedCount = 1;
                    do
                    {
                        int enchantType = UnityEngine.Random.Range(0, 4);

                        //마법카드는 체력 강화가 나올 수 없음.
                        while(card.card.cardType == CardType.Spell && enchantType == 1)
                        {
                            enchantType = UnityEngine.Random.Range(0, 4);
                        }

                        //강화 실행
                        switch(enchantType)
                        {
                            //공격력 강화
                            case 0:
                                card.card.stats.baseATK += 5;
                                break;
                            //체력 강화
                            case 1:
                                card.card.stats.baseMaxHp += 10;
                                break;
                            //코스트 강화
                            case 2:
                                card.card.cost = Mathf.Max(0, card.card.cost - 1);
                                break;
                            //랜덤 인장 부여
                            case 3:
                                //이미 인장이 3개면 다시 강화 시도
                                if(card.card.seals.Count >= 3) continue;

                                Array sealValues = Enum.GetValues(typeof(SealType));
                                int randomIndex = UnityEngine.Random.Range(1, sealValues.Length);

                                //인장 중복을 방지
                                while(card.card.seals.Contains((SealType)sealValues.GetValue(randomIndex)))
                                {
                                    randomIndex = UnityEngine.Random.Range(1, sealValues.Length);
                                }

                                card.card.seals.Add((SealType)sealValues.GetValue(randomIndex));
                                break;
                            default:
                                break;
                        }
                        enchantedCount++;
                        
                    } while (UnityEngine.Random.Range(0, 100) < 100 * Mathf.Pow(0.5f, enchantedCount + 1));
                }
            }
        }

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

        RunManager.Inst.resourceManager.SpendGold(50 * (eventRepeat + 1));
        eventRepeat++;
        CheckEventAvailable();
    }

    void selectCard(CardRewardCardUI card)
    {
        foreach(Transform child in cardRewardContainer)
        {
            child.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
        card.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        rewardCard = card;
        getCardButton.interactable = true;
    }

    bool CheckEventAvailable()
    {
        eventCostText.text = (50 * (eventRepeat + 1)).ToString();

        //충분한 골드를 소지하고 있을 경우
        if(RunManager.Inst.resourceManager.CheckEnoughGold(50 * (eventRepeat + 1)))
        {
            eventCostText.color = Color.white;
            return true;
        }
        else
        {
            eventCostText.color = Color.red;
            rerollButton.interactable = false;
            return false;
        }
    }

    public void GetCard()
    {
        DeckManager.Inst.AddCardToDeck(rewardCard.cardContent);
        getCardButton.interactable = false;
        cardRewardContainer.gameObject.SetActive(false);

        // eventManager._OnEventEnd();
    }
}
