using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixCardEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]GameObject firstEmptySlot;
    [SerializeField]GameObject secondEmptySlot;
    [SerializeField]GameObject lastEmptySlot;
    [SerializeField]GameObject firstCardSlot;
    [SerializeField]GameObject secondCardSlot;
    [SerializeField]GameObject lastCardSlot;

    [SerializeField]Button confirmButton;

    Dictionary<string, int> cardCountDict;

    CardRewardCardUI firstCard, secondCard;

    public void FilterDeckCard()
    {
        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        
        firstCard = null;
        secondCard = null;
        confirmButton.interactable = false;
        SetPreview();

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
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CanvasGroup>(), cardUI.GetComponent<CardRewardCardUI>()));

            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 5) + 1) * 470 + 50);
    }

    void FilterDeckCard(CardContent selectCard)
    {
        foreach (Transform child in content)
        {
            //다른 카드들은 비활성화.
            if(child.gameObject.GetComponent<CardRewardCardUI>().cardContent.id != selectCard.id)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void SelectCard(CanvasGroup cardCanvasgroup, CardRewardCardUI targetCard)
    {
        //이미 선택된 카드를 눌렀는지 검사.
        if(IsSelectedSameCard(targetCard.cardContent)) return;

        //카드를 하나도 고르지 않았을 때.
        if(firstCard == null && secondCard == null)
        {
            firstCard = targetCard;
            cardCanvasgroup.alpha = 0.3f;
            FilterDeckCard(firstCard.cardContent);
        }
        //두개 이상 선택하려 하는 경우
        else if(firstCard != null && secondCard != null)
        {
            return;
        }
        //두번째 카드를 선택 하는 경우
        else
        {
            secondCard = targetCard;
            cardCanvasgroup.alpha = 0.3f; 

            confirmButton.interactable = true;
        }
        SetPreview();
    }
    //이미 선택된 카드를 고른 경우
    bool IsSelectedSameCard(CardContent targetCard)
    {
        //첫번째 카드가 중복 선택일 때
        if(firstCard != null && firstCard.cardContent == targetCard)
        {
            //한장만 있는데 선택취소하면 초기화면으로
            if(secondCard == null)
            {
                FilterDeckCard();
            }
            else
            {
                firstCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                firstCard = secondCard;
                secondCard = null;
                SetPreview();
            }
            return true;
        }
        else if(secondCard != null && secondCard.cardContent == targetCard)
        {
            secondCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            secondCard = null;
            SetPreview();
            return true;
        }
        return false;
    }

    void SetPreview()
    {
        if(firstCard == null)
        {
            firstEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            firstCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
        else
        {
            firstEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            firstCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            firstCardSlot.GetComponent<CardRewardCardUI>().Setup(firstCard.cardContent);
        }

        if(secondCard == null)
        {
            secondEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            secondCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
        else
        {
            secondEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            secondCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            secondCardSlot.GetComponent<CardRewardCardUI>().Setup(secondCard.cardContent);
        }

        if(firstCard != null && secondCard != null)
        {
            lastEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            lastCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            lastCardSlot.GetComponent<CardRewardCardUI>().Setup(firstCard.cardContent);
        }
        else
        {
            lastEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            lastCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
    }

    public void MixCard()
    {
        firstCard.cardContent.stats.baseATK += secondCard.cardContent.stats.baseATK;
        firstCard.cardContent.stats.baseMaxHp += secondCard.cardContent.stats.baseMaxHp;

        //두번째 카드는 삭제하고 첫번째 카드의 스탯을 조정.
        RunManager.Inst.deckManager.RemoveCardToDeck(secondCard.cardContent);

        eventManager._OnEventEnd();
    }
}
