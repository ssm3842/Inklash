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
    [SerializeField]GameObject selectUseCardSlot;
    [SerializeField]GameObject selectWordCardSlot;

    [SerializeField]Image unitEventStone;
    [SerializeField]Sprite unitEventStoneDeactivated;
    [SerializeField]Sprite unitEventStoneActivated;
    [SerializeField]Image wordEventStone;
    [SerializeField]Sprite wordEventStoneDeactivated;
    [SerializeField]Sprite wordEventStoneActivated;

    [SerializeField]Button confirmButton;
    [SerializeField]TextMeshProUGUI eventCostText;
    int eventRepeat;


    CardRewardCardUI selectUseCard, selectWordCard;

    public void SetEvent()
    {
        selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);

        confirmButton.interactable = false;
        eventRepeat = 0;
        CheckEventAvailable();

        unitEventStone.sprite = unitEventStoneDeactivated;
        wordEventStone.sprite = wordEventStoneDeactivated;

        selectUseCard = null;
        selectWordCard = null;

        List<CardContent> deck = DeckManager.Inst.GetDeckdata();
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

        foreach (Transform child in useCardcontainer)
        {
            child.gameObject.SetActive(true);
            CardContent card = child.gameObject.GetComponent<CardRewardCardUI>().cardContent;
            if(card.seals.Count >= 3) child.gameObject.SetActive(false); //인장이 3개인 카드는 스킵.
            if(selectWordCard != null && SealManager.IsHaveSomeSeal(card, selectWordCard.cardContent.seals[0])) child.gameObject.SetActive(false); //이미 선택한 인장을 가지고 있는 카드는 스킵.
        }
    }
    public void SetWordCardContent()
    {
        useCardScrollView.SetActive(false);
        wordCardScrollView.SetActive(true);

        foreach (Transform child in wordCardcontainer)
        {
            child.gameObject.SetActive(true);
            CardContent card = child.gameObject.GetComponent<CardRewardCardUI>().cardContent;
            if(selectUseCard != null && SealManager.IsHaveSomeSeal(selectUseCard.cardContent, card.seals[0])) child.gameObject.SetActive(false); //이미 선택한 인장을 가지고 있는 카드는 스킵.
        }
    }
    void SelectUseCard(CanvasGroup cardCanvasgroup, CardRewardCardUI targetCard)
    {
        if(selectUseCard == targetCard)
        {
            selectUseCard = null;

            selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
            unitEventStone.sprite = unitEventStoneDeactivated;

            cardCanvasgroup.alpha = 1f;
        }
        else
        {
            //이미 선택한 카드를 빼야 다른 카드 선택 가능.
            if(selectUseCard != null) selectUseCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

            selectUseCard = targetCard;

            selectUseCardSlot.GetComponent<CardRewardCardUI>().Setup(selectUseCard.cardContent);
            selectUseCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);

            unitEventStone.sprite = unitEventStoneActivated;
            cardCanvasgroup.alpha = 0.3f;

            SetWordCardContent();
        }

        if(selectUseCard != null && selectWordCard != null && CheckEventAvailable()) confirmButton.interactable = true;
        else confirmButton.interactable = false;
    }
    void SelectWordCard(CanvasGroup cardCanvasgroup, CardRewardCardUI targetCard)
    {
        if(selectWordCard == targetCard)
        {
            selectWordCard = null;

            selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
            wordEventStone.sprite = wordEventStoneDeactivated;
            
            cardCanvasgroup.alpha = 1f;
        }
        else
        {
            //이미 선택한 카드를 빼야 다른 카드 선택 가능.
            if(selectWordCard != null) selectWordCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;;

            selectWordCard = targetCard;

            selectWordCardSlot.GetComponent<CardRewardCardUI>().Setup(selectWordCard.cardContent);
            selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(false);

            wordEventStone.sprite = wordEventStoneActivated;

            cardCanvasgroup.alpha = 0.3f;
        }

        if(selectUseCard != null && selectWordCard != null && RunManager.Inst.resourceManager.currentGold >= 50) confirmButton.interactable = true;
        else confirmButton.interactable = false;
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
            return false;
        }
    }

    void UpdateCards()
    {
        foreach (Transform child in useCardcontainer)
        {
            child.gameObject.GetComponent<CardRewardCardUI>().Setup(child.gameObject.GetComponent<CardRewardCardUI>().cardContent);
        }
    }

    public void UpgradeCard()
    {
        SealManager.AddSealToCard(selectUseCard.cardContent, selectWordCard.cardContent.seals[0]);
        DeckManager.Inst.RemoveCardToDeck(selectWordCard.cardContent);

        selectUseCardSlot.GetComponent<CardRewardCardUI>().Setup(selectUseCard.cardContent);
        selectWordCardSlot.GetComponent<CardRewardCardUI>().SetTransparent(true);
        wordEventStone.sprite = wordEventStoneDeactivated;

        Destroy(selectWordCard.gameObject);
        selectWordCard = null;

        UpdateCards();

        RunManager.Inst.resourceManager.SpendGold(50 * (eventRepeat + 1));
        eventRepeat++;
        CheckEventAvailable();

        // eventManager._OnEventEnd();
    }
}
