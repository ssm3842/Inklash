using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEvent : MonoBehaviour
{
    [SerializeField] GameObject useCardContainer;
    [SerializeField] GameObject wordCardContainer;

    [SerializeField] TextMeshProUGUI[] cardGoldTexts;
    [SerializeField] GameObject[] goldImage;

    [SerializeField] GameObject cardPrefab;

    [SerializeField] GameObject cardDescPanel;
    [SerializeField] TextMeshProUGUI cardDescText;

    [SerializeField] Button deleteButton;
    [SerializeField] TextMeshProUGUI deleteCardCostText;

    [SerializeField] GameObject cardDeleteDeckView;
    [SerializeField] Transform content; 

    int[] cardCostArray;

    public void EnterShop()
    {
        deleteButton.gameObject.SetActive(true);

        cardCostArray = new int[6];

        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in useCardContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in wordCardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        //유닛 카드만 표시.
        List<CardContent> cardList = RunManager.Inst.unitDataManager.GetShopUnitCardPool();
        for(int i = 0; i < 3; i++)
        {
            int cardIndex = i;
            cardCostArray[i] = UnityEngine.Random.Range(60, 80);
            cardGoldTexts[i].text = cardCostArray[i].ToString();

            GameObject cardUI = Instantiate(cardPrefab, useCardContainer.transform);
            cardUI.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            CardContent targetCard = cardList[UnityEngine.Random.Range(0, cardList.Count)];

            cardUI.GetComponent<CardRewardCardUI>().Setup(targetCard);
            cardUI.GetComponent<Button>().onClick.AddListener(() => BuyCard(cardCostArray[cardIndex], targetCard, cardUI.GetComponent<CanvasGroup>(), cardIndex));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverEnter.AddListener(() => UpdateCardDescPanel(targetCard, cardUI.transform));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverExit.AddListener(() => UpdateCardDescPanel());
        }
        
        //기존에 있던 단어 카드 오브젝트 삭제.
        foreach (Transform child in wordCardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        //마법 카드 추가
        cardList = RunManager.Inst.unitDataManager.GetShopSpellCardPool();
        for(int i = 3; i < 4; i++)
        {
            int cardIndex = i;
            cardCostArray[i] = UnityEngine.Random.Range(60, 80);
            cardGoldTexts[i].text = cardCostArray[i].ToString();

            GameObject cardUI = Instantiate(cardPrefab, wordCardContainer.transform);
            cardUI.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            CardContent targetCard = cardList[UnityEngine.Random.Range(0, cardList.Count)];

            cardUI.GetComponent<CardRewardCardUI>().Setup(targetCard);
            cardUI.GetComponent<Button>().onClick.AddListener(() => BuyCard(cardCostArray[cardIndex], targetCard, cardUI.GetComponent<CanvasGroup>(), cardIndex));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverEnter.AddListener(() => UpdateCardDescPanel(targetCard, cardUI.transform));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverExit.AddListener(() => UpdateCardDescPanel());
        }

        //단어 카드 표시.
        cardList = RunManager.Inst.unitDataManager.GetShopWordCardPool();
        for(int i = 4; i < 6; i++)
        {
            int cardIndex = i;
            cardCostArray[i] = UnityEngine.Random.Range(100, 120);

            cardGoldTexts[i].text = cardCostArray[i].ToString();

            GameObject cardUI = Instantiate(cardPrefab, wordCardContainer.transform);
            cardUI.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            CardContent targetCard = cardList[UnityEngine.Random.Range(0, cardList.Count)];
            
            cardUI.GetComponent<CardRewardCardUI>().Setup(targetCard);
            cardUI.GetComponent<Button>().onClick.AddListener(() => BuyCard(cardCostArray[cardIndex], targetCard, cardUI.GetComponent<CanvasGroup>(), cardIndex));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverEnter.AddListener(() => UpdateCardDescPanel(targetCard, cardUI.transform));
            cardUI.GetComponent<CardRewardCardUI>().CardHoverExit.AddListener(() => UpdateCardDescPanel());
        }

        UpdateCostTextColor();

        gameObject.SetActive(true);
        cardDeleteDeckView.SetActive(false);
    }

    void UpdateCardDescPanel(CardContent cardContent = null, Transform cardTransform = null)
    {
        if(cardContent != null)
        {
            cardDescPanel.SetActive(true);
            cardDescPanel.GetComponent<RectTransform>().position = cardTransform.position;
            cardDescText.text = cardContent.description;
        }
        else
        {
            cardDescPanel.SetActive(false);
        }
    }

    void BuyCard(int cost, CardContent cardData, CanvasGroup cardObject, int index)
    {
        if(RunManager.Inst.resourceManager.SpendGold(cost))
        {
            DeckManager.Inst.AddCardToDeck(cardData);

            cardObject.interactable = false;
            cardObject.alpha = 0;
            cardObject.blocksRaycasts = false;

            cardGoldTexts[index].text = "";
            goldImage[index].SetActive(false);

            UpdateCostTextColor();
        }
    }

    void UpdateCostTextColor()
    {
        ResourceManager resourceManager = RunManager.Inst.resourceManager;
        for(int i=0; i<6; i++)
        {
            if(resourceManager.CheckEnoughGold(cardCostArray[i])) cardGoldTexts[i].color = Color.white;
            else cardGoldTexts[i].color = Color.red;
        }

        if(resourceManager.CheckEnoughGold(50)) deleteCardCostText.color = Color.white;
        else deleteCardCostText.color = Color.red;
    }
    
    public void OnCardDeleteButtonClicked()
    {   
        int childCount = 0;
        if(!RunManager.Inst.resourceManager.CheckEnoughGold(50)) return;

        cardDeleteDeckView.SetActive(true);

        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        List<CardContent> cardList = DeckManager.Inst.GetDeckdata();
        foreach(CardContent card in cardList)
        {
            GameObject cardUI = Instantiate(cardPrefab, content);
            cardUI.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardUI.GetComponent<CardRewardCardUI>().Setup(card);
            cardUI.GetComponent<Button>().onClick.AddListener(() => DeleteCard(card));
            childCount += 1;
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((childCount - 1) / 5) + 1) * 330 + 400);
    }

    void DeleteCard(CardContent targetCard)
    {
        RunManager.Inst.resourceManager.SpendGold(50);
        DeckManager.Inst.RemoveCardToDeck(targetCard);
        deleteButton.gameObject.SetActive(false);

        cardDeleteDeckView.SetActive(false);

        UpdateCostTextColor();
    }

    public void ExitShop()
    {
        RunManager.Inst.mapManager.ClearLastRoom();
        RunManager.Inst.mapManager.SetVisible();
        gameObject.SetActive(false);
    }
}
