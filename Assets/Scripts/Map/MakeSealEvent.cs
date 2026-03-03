using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MakeSealEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]Transform useCardcontainer;
    [SerializeField]GameObject useCardScrollView;
    [SerializeField]Transform wordCardcontainer;
    [SerializeField]GameObject wordCardScrollView;

    [SerializeField]GameObject emptyUseCardSlot;
    [SerializeField]GameObject emptyWordCardSlot;
    [SerializeField]GameObject selectUseCardSlot;
    [SerializeField]GameObject selectWordCardSlot;

    [SerializeField]Button confirmButton;


    CardRewardCardUI selectUseCard, selectWordCard;

    public void SetEvent()
    {
        emptyUseCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        emptyWordCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);

        confirmButton.interactable = false;

        selectUseCard = null;
        selectWordCard = null;

        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();
        //유닛, 마법 카드를 표시할 캔버스를 먼저 초기화
        foreach (Transform child in useCardcontainer)
        {
            Destroy(child.gameObject);
        }

        int spawnedCount = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Unit || card.cardType == CardType.Spell)
            {
                GameObject cardUI = Instantiate(cardPrefab, useCardcontainer);
                cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectUseCard(cardUI.GetComponent<CanvasGroup>(), cardUI.GetComponent<CardRewardCardUI>()));
                spawnedCount++;
            }
        }
        //카드 수에 따라 스크롤 뷰 높이를 변경.
        useCardcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((spawnedCount - 1) / 4) + 1) * 270);

        //단어카드 캔버스를 초기화
        foreach (Transform child in wordCardcontainer)
        {
            Destroy(child.gameObject);
        }

        spawnedCount = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Word)
            {
                GameObject cardUI = Instantiate(cardPrefab, wordCardcontainer);
                cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectWordCard(cardUI.GetComponent<CanvasGroup>(), cardUI.GetComponent<CardRewardCardUI>()));
                spawnedCount++;
            }
        }
        //카드 수에 따라 스크롤 뷰 높이를 변경.
        wordCardcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((spawnedCount - 1) / 4) + 1) * 270);

        SetUseCardContent();
    }

    public void SetUseCardContent()
    {
        useCardScrollView.SetActive(true);
        wordCardScrollView.SetActive(false);
    }
    public void SetWordCardContent()
    {
        useCardScrollView.SetActive(false);
        wordCardScrollView.SetActive(true);
    }
    void SelectUseCard(CanvasGroup cardCanvasgroup, CardRewardCardUI targetCard)
    {
        if(selectUseCard == targetCard)
        {
            selectUseCard = null;

            emptyUseCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);

            cardCanvasgroup.alpha = 1f;
        }
        else
        {
            //이미 선택한 카드를 빼야 다른 카드 선택 가능.
            if(selectUseCard != null) selectUseCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

            selectUseCard = targetCard;

            selectUseCardSlot.GetComponent<CardRewardCardUI>().Setup(selectUseCard.cardContent);
            selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            emptyUseCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            cardCanvasgroup.alpha = 0.3f;

            SetWordCardContent();
        }

        if(selectUseCard != null && selectWordCard != null) confirmButton.interactable = true;
        else confirmButton.interactable = false;
    }
    void SelectWordCard(CanvasGroup cardCanvasgroup, CardRewardCardUI targetCard)
    {
        if(selectWordCard == targetCard)
        {
            selectWordCard = null;

            emptyWordCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
            
            cardCanvasgroup.alpha = 1f;
        }
        else
        {
            //이미 선택한 카드를 빼야 다른 카드 선택 가능.
            if(selectWordCard != null) selectWordCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;;

            selectWordCard = targetCard;

            selectWordCardSlot.GetComponent<CardRewardCardUI>().Setup(selectWordCard.cardContent);
            selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);
            emptyWordCardSlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            cardCanvasgroup.alpha = 0.3f;
        }

        if(selectUseCard != null && selectWordCard != null) confirmButton.interactable = true;
        else confirmButton.interactable = false;
    }

    public void UpgradeCard()
    {
        SealManager.AddSealToCard(selectUseCard.cardContent, selectWordCard.cardContent.seals);
        RunManager.Inst.deckManager.RemoveCardToDeck(selectWordCard.cardContent);

        eventManager._OnEventEnd();
    }
}
