using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform handTransform;
    public CostManager costManager;
    public UnitManager unitManager;
    HandLayout handLayout;

    [SerializeField] List<CardContent> currentBattleDeck;
    [SerializeField] List<Card> playerHands;

    private RectTransform draggingCardRectTransform;
    public Card draggingCard;
    public bool isDraggingCard = false;


    public void Init() //TODO: 이름 바꾸기
    {
        // List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        currentBattleDeck = new List<CardContent>(RunManager.Inst.deckManager.GetDeckdata());

        handLayout = handTransform.GetComponent<HandLayout>();

        costManager.Init();
        StartBattle();
    }

    void StartBattle()
    {
        // currentBattleDeck.shuffle(); //TODO: 게임 시작 시 덱 섞는 기능 추가하기. 덱매니저에다가.
        for (int i = 0; i < 5; i++) DrawCard();
    }

    void DrawCard()
    {
        if (playerHands.Count >= GameRule.MAX_HAND_CARD_NUM) return; //플레이어 패가 5장 이상이면 드로우 불가. //TODO: 반응 추가하기 ex)카드를 더 뽑을 수 없어 메시지 등

        var cardObject = Instantiate(cardPrefab, handTransform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(this, PopCardFromDeck());
        playerHands.Add(card);

        if (handLayout != null)
        {
            handLayout.AddCardToHand(cardObject);
        }
    }

    public CardContent PopCardFromDeck()
    {
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
            card.slot.isEmpty = true;

            playerHands.Remove(draggingCard);
            currentBattleDeck.Add(draggingCard.cardContent);
            Card temp = draggingCard;
            draggingCard = null;
            Destroy(temp.gameObject);

            DrawCard();
        }
    }

    public void CardRightClicked()
    {
        draggingCardRectTransform.position = draggingCard.slot.GetComponent<RectTransform>().position + new Vector3(0, 145, 0);
        draggingCardRectTransform = null;

        isDraggingCard = false;
        draggingCard = null;
    }

    // void AlignHand()
    // {
    //     float step = 1f / (playerHands.Count + 1); //1장이면 2로 계산해서 0.5, 2장이면 3으로 계산해 0.33

    //     float[] posBase = new float[playerHands.Count];
    //     for (int i = 0; i < playerHands.Count; i++)
    //     {
    //         posBase[i] = step * (i + 1);
    //     }

    //     for (int i = 0; i < playerHands.Count; i++)
    //     {
    //         playerHands[i].transform.position = new Vector3(10 * posBase[i] - 5, -3.5f);
    //         // playerHands[i].SetOrderInLayer(i);
    //     }
    // }
}