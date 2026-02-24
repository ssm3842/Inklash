using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampfireEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;

    [SerializeField]GameObject emptySlot;
    [SerializeField]GameObject selectSlot;

    [SerializeField]CardRewardCardUI cardPreviewBefore;
    [SerializeField]CardRewardCardUI cardPreviewAfter;
    [SerializeField]TextMeshProUGUI atkText;
    [SerializeField]TextMeshProUGUI hpText;

    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;
    [SerializeField]TextMeshProUGUI text;

    StatType mapType;
    CardRewardCardUI selectCard;

    public void FilterDeckCard(StatType newMaptype, CardType cardType)
    {   
        emptySlot.SetActive(true);
        selectSlot.SetActive(false);

        mapType = newMaptype;
        selectCard = null;
        
        if(newMaptype == StatType.MAX_HP)
        {
            text.text = "체력강화";
        }
        else if(newMaptype == StatType.ATK)
        {
            text.text = "공격력강화";
        }

        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        //카드 타입이 같은 카드만 골라 표시.
        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();
        foreach(CardContent card in deck)
        {
            if(card.cardType == cardType)
            {
                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>(), newMaptype));
            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 5) + 1) * 470 + 50);
    }

    void SelectCard(CardRewardCardUI targetCard, StatType targetStat)
    {
        emptySlot.SetActive(false);
        selectSlot.SetActive(true);

        if(selectCard != null) selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        selectCard = targetCard;
        selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 0.3f;

        hpText.color = Color.black;
        atkText.color = Color.black;

        cardPreviewBefore.Setup(targetCard.cardContent);
        cardPreviewAfter.Setup(targetCard.cardContent);
        if(targetStat == StatType.MAX_HP)
        {
            hpText.text = (targetCard.cardContent.stats.baseMaxHp + 10).ToString();
            hpText.color = Color.red;
        }
        else if(targetStat == StatType.ATK)
        {
            atkText.text = (targetCard.cardContent.stats.baseATK + 5).ToString();
            atkText.color = Color.red;
        }
    }

    public void ConfirmUpgradeCard()
    {
        if(mapType == StatType.MAX_HP) selectCard.cardContent.stats.baseMaxHp += 10;
        else if(mapType == StatType.ATK) selectCard.cardContent.stats.baseATK += 5;

        eventManager._OnEventEnd();
    }
}
