using System.Linq; // 정렬(OrderBy)을 위해 필수
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class DeckViewer : MonoBehaviour
{
    [SerializeField] private CardManager cardManager; 

    [SerializeField] private TextMeshProUGUI titleText;  
    [SerializeField] private Transform cardListContent;   
    
    [SerializeField] private GameObject cardPrefab;


    // void Start()
    // {
    //     gameObject.SetActive(false);
    // }

    void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) ClosePanel();
    }

    public void ShowDrawDeck()
    {
        PopulatePanel(cardManager.GetDrawPile(), "뽑을 카드 더미");
    }

    public void ShowDiscardDeck()
    {
        PopulatePanel(cardManager.GetDiscardPile(), "버린 카드 더미");
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
            GameObject cardObj = Instantiate(cardPrefab, cardListContent);
            
            Card cardScript = cardObj.GetComponent<Card>();

            if (cardScript != null)
            {
                cardScript.Setup(cardManager, cardData, 1); 
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
        gameObject.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; 
    }
}