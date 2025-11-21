using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public GameObject CardPrefab { get { return cardPrefab; } }

    [SerializeField] HandLayout handLayout;
    public CostManager costManager;
    public UnitManager unitManager;

    [SerializeField] List<CardContent> currentBattleDeck;
    [SerializeField] List<CardContent> discardBattleDeck = new List<CardContent>();
    [SerializeField] List<Card> playerHands;

    private RectTransform draggingCardRectTransform;
    public Card draggingCard;
    public bool isDraggingCard = false;

    private readonly KeyCode[] rerollKeys = new KeyCode[] 
        { 
            KeyCode.Alpha1, 
            KeyCode.Alpha2, 
            KeyCode.Alpha3, 
            KeyCode.Alpha4, 
            KeyCode.Alpha5 
        };

    public void Init() //TODO: 이름 바꾸기
    {
        // List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        currentBattleDeck = new List<CardContent>(RunManager.Inst.deckManager.GetDeckdata());

        costManager.Init();
        StartBattle();
    }

    void StartBattle()
    {
        Shuffle(currentBattleDeck);
        for (int i = 0; i < 5; i++) DrawCard();
    }

    void DrawCard()
    {
        if (playerHands.Count >= GameRule.MAX_HAND_CARD_NUM) return; 

        var cardObject = Instantiate(cardPrefab, handLayout.transform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(this, PopCardFromDeck());
        playerHands.Add(card);

        handLayout.AlignCards();
    }

    public CardContent PopCardFromDeck()
    {

        if (currentBattleDeck.Count == 0)
        {
            if (discardBattleDeck.Count == 0)
            {
                Debug.LogWarning("뽑을 카드와 버린 카드가 모두 없습니다!");
                return null;
            }
            currentBattleDeck = new List<CardContent>(discardBattleDeck);
            discardBattleDeck.Clear();

            Shuffle(currentBattleDeck);
        }
        
        CardContent cardContent = currentBattleDeck[0];
        currentBattleDeck.RemoveAt(0);

        return cardContent;
    }

    void Update()
    {
        if (isDraggingCard)
        {
            draggingCardRectTransform.position = Input.mousePosition;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshHand();
            handLayout.AlignCards();
        }

        for (int i = 0; i < rerollKeys.Length; i++)
        {
            if (Input.GetKeyDown(rerollKeys[i]))
            {
                RerollCardAtIndex(i); // i가 곧 0, 1, 2, 3, 4 인덱스가 됨
            }
        }
    }

    public void CardLeftClicked(Card card)
    {
        if (!isDraggingCard) //카드가 손에 없을 때 좌클릭 되면 마우스 따라 이동.
        {
            if (!costManager.CheckUseCostAvailable(card.cardContent.cost)) return; //코스트 부족 시 드래그 불가.

            isDraggingCard = true;
            draggingCard = card;
            draggingCardRectTransform = card.GetComponent<RectTransform>();
            // draggingCard.SetOrderInLayer(GameRule.MAX_HAND_CARD_NUM + 1);
        }
        else //카드가 손에 있을 때 좌클릭 하면 사용.
        {
            costManager.UseCost(card.cardContent.cost); //실제로 코스트 사용.

            isDraggingCard = false;
            switch (card.cardContent.type)
            {
                case CardType.Unit:
                    unitManager.SpawnPlayerUnit(draggingCard.cardContent);
                    break;
                case CardType.Spell:
                    unitManager.SpawnPlayerUnit(draggingCard.cardContent);
                    break;
                case CardType.Word:
                    unitManager.SpawnPlayerUnit(draggingCard.cardContent); //TODO: 단어카드 사용 효과 구현.
                    break;
            }

            //카드 사용 후 뒤에서부터 정렬
            playerHands.Remove(draggingCard);
            discardBattleDeck.Add(draggingCard.cardContent);
            Card temp = draggingCard;
            draggingCard = null;
            temp.transform.SetParent(null);
            Destroy(temp.gameObject);
            handLayout.AlignCards();

            DrawCard();

            /* 카드 사용시 사용한 카드와의 교환
            int cardIndex = playerHands.IndexOf(draggingCard);
            discardBattleDeck.Add(draggingCard.cardContent);
            
            Card temp = draggingCard;
            draggingCard = null;
            
            temp.transform.SetParent(null);
            Destroy(temp.gameObject);

            CardContent newCardData = PopCardFromDeck();

            if (newCardData != null)
            {
                var newCardObject = Instantiate(cardPrefab, handLayout.transform);
                var newCardScript = newCardObject.GetComponent<Card>();
                newCardScript.Setup(this, newCardData);

                playerHands[cardIndex] = newCardScript; 

                newCardObject.transform.SetSiblingIndex(cardIndex);
            }
            else
            {
                playerHands.RemoveAt(cardIndex);
            }

            handLayout.AlignCards();
            */
        }
    }

    public void CardRightClicked()
    {
        handLayout.AlignCards();
        draggingCardRectTransform = null;

        isDraggingCard = false;
        draggingCard = null;
    }

    public List<CardContent> GetDrawPile()
    {
        return currentBattleDeck;
    }

    public List<CardContent> GetDiscardPile()
    {
        return discardBattleDeck;
    }

    private void Shuffle(List<CardContent> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); 
            
            CardContent value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

public void RefreshHand() //DrawCard와 중복되는 내용 정리 필요
    {

        int cardsToDraw = playerHands.Count;

        while (playerHands.Count > 0)
        {
            Card card = playerHands[0]; 
            discardBattleDeck.Add(card.cardContent);
            playerHands.RemoveAt(0);
            
            card.transform.SetParent(null); 
            Destroy(card.gameObject);     
        }

        handLayout.AlignCards(); 

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardContent cardData = PopCardFromDeck();
            if (cardData == null) break; 

            var cardObject = Instantiate(cardPrefab, handLayout.transform); 
            var card = cardObject.GetComponent<Card>();
            card.Setup(this, cardData);
            playerHands.Add(card);
        }

        handLayout.AlignCards();
    }

// 개별 리롤
    private void RerollCardAtIndex(int index)
    {
        if (isDraggingCard) return;
        if (index < 0 || index >= playerHands.Count) return;

        Card targetCard = playerHands[index];
        RerollSingleCard(targetCard);
    }

    public void RerollSingleCard(Card cardToReroll)
    {
        if (cardToReroll == null) return;

        int originalIndex = playerHands.IndexOf(cardToReroll);

        if (originalIndex == -1) return;
        discardBattleDeck.Add(cardToReroll.cardContent);
        
        cardToReroll.transform.SetParent(null); 
        Destroy(cardToReroll.gameObject);

        CardContent newCardData = PopCardFromDeck();

        if (newCardData != null)
        {
            var cardObject = Instantiate(cardPrefab, handLayout.transform);
            var card = cardObject.GetComponent<Card>();
            card.Setup(this, newCardData);

            playerHands[originalIndex] = card; 

            card.transform.SetSiblingIndex(originalIndex);
        }
        else
        {
            playerHands.RemoveAt(originalIndex);
        }

        // 4. 정렬
        handLayout.AlignCards();
    }
}