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
    [SerializeField] List<CardContent> graveYardDeck;
    [SerializeField] List<Card> playerHands;

    public Card draggingCard;
    public bool isDraggingCard = false;


    public void Init() //TODO: 이름 바꾸기
    {
        // List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        currentBattleDeck = new List<CardContent>(RunManager.Inst.deckManager.GetDeckdata());
        graveYardDeck = new List<CardContent>();

        costManager.Init();

        StartBattle();
    }

    void StartBattle()
    {
        // currentBattleDeck.shuffle(); //TODO: 게임 시작 시 덱 섞는 기능 추가하기. 덱매니저에다가.
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

            MoveCardToGraveYard(draggingCard);
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

    void MoveCardToGraveYard(Card targetCard) //매개변수 카드를 패에서 묘지로 보냄.
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
}