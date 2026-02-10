using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MakeFlagEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]GameObject containerGroup;
    [SerializeField]Transform useCardcontainer;
    [SerializeField]Transform wordCardcontainer;
    [SerializeField]GameObject cardPrefab;
    [SerializeField]TextMeshProUGUI text;

    public void FilterDeckCard()
    {
        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in useCardcontainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in wordCardcontainer)
        {
            Destroy(child.gameObject);
        }

        //카드 타입이 유닛, 마법인 카드만 골라 표시.
        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Unit || card.cardType == CardType.Spell)
            {
                GameObject cardUI = Instantiate(cardPrefab, useCardcontainer);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                // cardUI.GetComponent<Button>().onClick.AddListener(() => UpgradeCard(card, maptype));
            }
        }
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Word)
            {
                GameObject cardUI = Instantiate(cardPrefab, wordCardcontainer);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                // cardUI.GetComponent<Button>().onClick.AddListener(() => UpgradeCard(card, maptype));
            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        int manyCard = Mathf.Max(useCardcontainer.transform.childCount, wordCardcontainer.transform.childCount);
        containerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((manyCard - 1) / 5) + 1) * 470 + 50);
    }

    void UpgradeCard(CardContent targetCard, StatType targetStat)
    {
        if(targetStat == StatType.MAX_HP) targetCard.stats.baseMaxHp += 10;
        else if(targetStat == StatType.ATK) targetCard.stats.baseATK += 5;

        eventManager._OnEventEnd();
    }
}
