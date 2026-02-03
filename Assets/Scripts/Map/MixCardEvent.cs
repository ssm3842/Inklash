using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixCardEvent : MonoBehaviour
{
    [SerializeField] EventManager eventManager;
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    Dictionary<string, int> cardCountDict;

    public void FilterDeckCard()
    {
        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        
        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();

        //덱에 있는 카드들의 매수를 구함.
        cardCountDict = new Dictionary<string, int>();
        foreach(CardContent card in deck)
        {
            //아이디로 카드를 검사. 딕셔너리에 있을 경우 숫자만 증가
            if(cardCountDict.ContainsKey(card.id)) cardCountDict[card.id]++;
            else cardCountDict.Add(card.id, 1);
        }
        
        foreach(CardContent card in deck)
        {
            //카드가 2장 이상 있을 경우에만 카드UI를 생성.
            if(cardCountDict[card.id] >= 2)
            {
                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                // cardUI.GetComponent<Button>().onClick.AddListener(() => UpgradeCard(card, maptype));
            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 5) + 1) * 470 + 50);
    }
}
