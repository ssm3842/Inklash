using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixCardEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]GameObject firstCardSlot;
    [SerializeField]GameObject secondCardSlot;
    [SerializeField]GameObject lastCardSlot;

    [SerializeField]TextMeshProUGUI atkText;
    [SerializeField]TextMeshProUGUI hpText;
    [SerializeField]TextMeshProUGUI costText;

    [SerializeField]Image eventStone;
    [SerializeField]Sprite eventStoneDeactivated;
    [SerializeField]Sprite eventStoneActivated;

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
        eventStone.sprite = eventStoneDeactivated;
        SetPreview();

        List<CardContent> deck = DeckManager.Inst.GetDeckdata();

        //덱에 있는 카드들의 매수를 구함.
        cardCountDict = new Dictionary<string, int>();
        foreach(CardContent card in deck)
        {
            //아이디로 카드를 검사. 딕셔너리에 있을 경우 숫자만 증가
            if(cardCountDict.ContainsKey(card.id)) cardCountDict[card.id]++;
            else cardCountDict.Add(card.id, 1);
        }
        
        int spawnedCount = 0;
        foreach(CardContent card in deck)
        {
            //카드가 2장 이상 있을 경우에만 카드UI를 생성.
            if(cardCountDict[card.id] >= 2)
            {
                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CanvasGroup>(), cardUI.GetComponent<CardRewardCardUI>()));
                spawnedCount++;

            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((spawnedCount - 1) / 4) + 1) * 270);
    }

    void FilterDeckCard(CardContent selectCard)
    {
        int activatedCount = 0;
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
                activatedCount++;
            }
        }
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((activatedCount - 1) / 4) + 1) * 270);
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
        //두개 초과로 선택하려 하는 경우
        else if(firstCard != null && secondCard != null)
        {
            return;
        }
        //두번째 카드를 선택 하는 경우
        else
        {
            secondCard = targetCard;
            cardCanvasgroup.alpha = 0.3f; 

            if (RunManager.Inst.resourceManager.currentGold >= 50)confirmButton.interactable = true;
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
            firstCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
        else
        {
            firstCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            firstCardSlot.GetComponent<CardRewardCardUI>().Setup(firstCard.cardContent);
        }

        if(secondCard == null)
        {
            secondCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
        else
        {
            secondCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            secondCardSlot.GetComponent<CardRewardCardUI>().Setup(secondCard.cardContent);
        }

        if(firstCard != null && secondCard != null)
        {
            // lastEmptySlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            lastCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            lastCardSlot.GetComponent<CardRewardCardUI>().Setup(firstCard.cardContent);

            atkText.color = Color.red;
            atkText.text = (firstCard.cardContent.stats.baseATK + Mathf.FloorToInt(secondCard.cardContent.stats.baseATK * 0.5f)).ToString();
            hpText.color = Color.red;
            hpText.text = (firstCard.cardContent.stats.baseMaxHp + Mathf.FloorToInt(secondCard.cardContent.stats.baseMaxHp * 0.5f)).ToString();
            // costText.color = Color.red;
            costText.text = Mathf.Min(firstCard.cardContent.cost, secondCard.cardContent.cost).ToString();
            eventStone.sprite = eventStoneActivated;
        }
        else
        {
            eventStone.sprite = eventStoneDeactivated;

            lastCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        }
    }

    public void MixCard()
    {
        firstCard.cardContent.cost = Mathf.Min(firstCard.cardContent.cost, secondCard.cardContent.cost);
        firstCard.cardContent.stats.baseATK += Mathf.FloorToInt(secondCard.cardContent.stats.baseATK * 0.5f);
        firstCard.cardContent.stats.baseMaxHp += Mathf.FloorToInt(secondCard.cardContent.stats.baseMaxHp * 0.5f);

        //두번째 카드는 삭제하고 첫번째 카드의 스탯을 조정.
        DeckManager.Inst.RemoveCardToDeck(secondCard.cardContent);
        RunManager.Inst.resourceManager.SpendGold(50);

        eventManager._OnEventEnd();
    }
}
