using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] HandLayout handLayout;
    [SerializeField] Canvas battleUICanvas;
    public CostManager costManager;
    public UnitManager unitManager;

    [SerializeField] List<CardContent> currentBattleDeck;
    [SerializeField] List<CardContent> discardBattleDeck;
    [SerializeField] List<Card> playerHands;

    public Card draggingCard;
    public bool isDraggingCard = false;

    public void Init() //TODO: 이름 바꾸기
    {
        // List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        currentBattleDeck = new List<CardContent>(RunManager.Inst.deckManager.GetDeckdata());
        discardBattleDeck = new List<CardContent>();

        costManager.Init();

        StartBattle();
    }

    void StartBattle()
    {
        Shuffle(currentBattleDeck);
        DrawNewHand(true);
    }

    void DrawCard()
    {
        if (currentBattleDeck.Count <= 0)
        {
            currentBattleDeck = new List<CardContent>(graveYardDeck);
            graveYardDeck.Clear();
        }

        if (playerHands.Count >= GameRule.MAX_HAND_CARD_NUM) return; //플레이어 패가 5장 이상이면 드로우 불가. //TODO: 반응 추가하기 ex)카드를 더 뽑을 수 없어 메시지 등

        var cardObject = Instantiate(cardPrefab, handLayout.transform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(this, PopCardFromDeck(), card.transform.GetSiblingIndex());
        playerHands.Add(card);

        handLayout.AlignCards();
    }

    public void DrawNewHand(bool isFree) //패가 가득 찰 때까지 카드를 뽑음.
    {   
        if(!isFree) //전투 시작 시 또는 패를 다 사용했을 때는 비용 없이 카드 다시뽑기.
        {
            if (!costManager.CheckUseCostAvailable(playerHands.Count + 1)) return;
            costManager.UseCost(playerHands.Count + 1);
        }

        for (int i = playerHands.Count - 1; i >= 0; i--) //에러 방지를 위해 역순으로 묘지로 이동
        {
            MoveCardToGraveYard(playerHands[i]);
        }

        for (int i = 0; i < GameRule.MAX_HAND_CARD_NUM; i++) //카드 5장 다시 뽑기
        {
            DrawCard();
        }
        
        handLayout.AlignCards();
    }

    CardContent PopCardFromDeck()
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
            draggingCard.transform.position = Input.mousePosition;
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
            draggingCard.transform.SetParent(battleUICanvas.transform);
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
                    unitManager.CastPlayerSpell(draggingCard.cardContent);
                    break;
                case CardType.Word:
                    unitManager.SpawnPlayerUnit(draggingCard.cardContent); //TODO: 단어카드 사용 효과 구현.
                    break;
            }

            MoveCardToDiscardDeck(draggingCard);
            draggingCard = null;

            RunManager.Inst.battleManager.OnCardUse();

            if (playerHands.Count <= 0)
            {
                costManager.AddCost(3);
                DrawNewHand(true);
            }

            // Card temp = draggingCard;
            // draggingCard = null;
            // Destroy(temp.gameObject);

            handLayout.AlignCards();
        }
    }

    public void CardRightClicked()
    {
        isDraggingCard = false;
        draggingCard.transform.SetParent(handLayout.transform);
        draggingCard.transform.SetSiblingIndex(draggingCard.originalIndex);
        draggingCard = null;

        handLayout.AlignCards();
    }

    void MoveCardToDiscardDeck(Card targetCard) //매개변수 카드를 패에서 묘지로 보냄.
    {
        playerHands.Remove(targetCard); //패에서 카드 데이터 제거.
        graveYardDeck.Add(targetCard.cardContent);

        targetCard.transform.SetParent(battleUICanvas.transform);
        Destroy(targetCard.gameObject);
    }

    public void OnCardUse()
    {
        // Debug.Log(transform.childCount);
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