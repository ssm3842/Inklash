using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MakeFlagEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]Transform useCardcontainer;
    [SerializeField]GameObject useCardScrollView;
    [SerializeField]Transform wordCardcontainer;
    [SerializeField]GameObject wordCardScrollView;
    [SerializeField]GameObject cardPrefab;
    [SerializeField]TextMeshProUGUI text;

    CardContent useCard = null;

    public void SetEvent()
    {
        SetUseCardContent();
    }

    void SetUseCardContent()
    {
        useCardScrollView.SetActive(true);
        wordCardScrollView.SetActive(false);

        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();

        //유닛, 마법 카드를 표시할 캔버스를 먼저 초기화
        foreach (Transform child in useCardcontainer)
        {
            Destroy(child.gameObject);
        }
        //카드 타입이 유닛, 마법인 카드만 골라 표시.
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Unit || card.cardType == CardType.Spell)
            {
                GameObject cardUI = Instantiate(cardPrefab, useCardcontainer);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectUseCard(card));
            }
        }
        useCard = null;
        //카드 수에 따라 스크롤 뷰 높이를 변경.
        useCardcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((useCardcontainer.childCount - 1) / 5) + 1) * 470 + 50);
    }
    void SetWordCardContent()
    {
        useCardScrollView.SetActive(false);
        wordCardScrollView.SetActive(true);

        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();

        //단어카드 캔버드를 초기화
        foreach (Transform child in wordCardcontainer)
        {
            Destroy(child.gameObject);
        }

        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Word)
            {
                GameObject cardUI = Instantiate(cardPrefab, wordCardcontainer);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => UpgradeCard(useCard, card));
            }
        }
        //카드 수에 따라 스크롤 뷰 높이를 변경.
        wordCardcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((wordCardcontainer.childCount - 1) / 5) + 1) * 470 + 50);
    }
    void SelectUseCard(CardContent card)
    {
        useCard = card;

        SetWordCardContent();
    }

    void UpgradeCard(CardContent targetUseCard, CardContent targetWordCard)
    {
        Debug.Log(targetUseCard.name + "에게 " + targetWordCard.name + " 효과를 부여함");
        RunManager.Inst.deckManager.RemoveCardToDeck(targetWordCard);

        eventManager._OnEventEnd();
    }
}
