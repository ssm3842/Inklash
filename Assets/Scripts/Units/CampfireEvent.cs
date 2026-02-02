using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampfireEvent : MonoBehaviour
{
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;
    [SerializeField]TextMeshProUGUI text;
    public void FilterDeckCard(StatType maptype, CardType cardType)
    {
        if(maptype == StatType.MAX_HP)
        {
            text.text = "체력강화";
        }
        else if(maptype == StatType.ATK)
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
            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 5) + 1) * 470 + 50);
    }
}
