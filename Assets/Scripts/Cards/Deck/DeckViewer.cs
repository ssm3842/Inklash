using System.Linq; // 정렬(OrderBy)을 위해 필수
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class DeckViewer : MonoBehaviour
{
    [SerializeField] private CardManager cardManager; 

    [SerializeField] private GameObject deckViewerPanel;
    [SerializeField] private TextMeshProUGUI titleText;  
    [SerializeField] private Button closeButton;        

    [SerializeField] private Transform cardListContent;   
    
    // [SerializeField] private GameObject cardPreviewPrefab; // (CardManager의 것을 쓰므로 필요 없음)

    [SerializeField] private Button drawDeckButton;      
    [SerializeField] private Button discardDeckButton; 

    void Start()
    {
        drawDeckButton.onClick.AddListener(ShowDrawDeck);
        discardDeckButton.onClick.AddListener(ShowDiscardDeck);
        closeButton.onClick.AddListener(ClosePanel);

        deckViewerPanel.SetActive(false);
    }

    void Update()
    {
        if (deckViewerPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void ShowDrawDeck()
    {
        List<CardContent> deck = cardManager.GetDrawPile();
        PopulatePanel(deck, "뽑을 카드 더미");
    }

    public void ShowDiscardDeck()
    {
        List<CardContent> deck = cardManager.GetDiscardPile();
        PopulatePanel(deck, "버린 카드 더미");
    }

    private void PopulatePanel(List<CardContent> cardList, string title)
    {
        titleText.text = $"{title} ({cardList.Count}장)";

        foreach (Transform child in cardListContent)
        {
            Destroy(child.gameObject);
        }

        List<CardContent> sortedList = cardList
            .OrderBy(card => card.type)
            .ThenBy(card => card.cost)
            .ToList();

        foreach (CardContent cardData in sortedList)
        {
            GameObject cardObj = Instantiate(cardManager.CardPrefab, cardListContent);
            
            Card cardScript = cardObj.GetComponent<Card>();

            if (cardScript != null)
            {
                cardScript.Setup(cardManager, cardData); 
            }
            else
            {
                Debug.LogError("Card 프리팹에 Card.cs 스크립트가 없습니다!");
            }
        }

        OpenPanel();
    }
    
    private void OpenPanel()
    {
        deckViewerPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ClosePanel()
    {
        deckViewerPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}