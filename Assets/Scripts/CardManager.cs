using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] CardContentSO cardContentSO;

    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform handTransform;
    HandLayout handLayout;

    [SerializeField] List<CardContent> playerDeck;
    [SerializeField] List<Card> playerHands;

    private RectTransform draggingCardRectTransform;
    public Card draggingCard;
    public bool isDraggingCard = false;


    void Start()
    {
        List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        //임시코드
        playerDeck = new List<CardContent>(10);
        for (int i = 0; i < 10; i++)
        {
            playerDeck.Add(cardContentSO.cardContents[Random.Range(0,playerCardPool.Count)]);
        }

        handLayout = handTransform.GetComponent<HandLayout>();
        SetupGame();
    }

    void SetupGame()
    {
        for (int i = 0; i < 4; i++) DrawCard();
        StartCoroutine(DrawCardCoroutine());
    }

    IEnumerator DrawCardCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DrawCard();
        }
    }
    void DrawCard()
    {
        Debug.Log("try draw");
        if (playerHands.Count >= GameRule.MAX_HAND_CARD_NUM) return; //플레이어 패가 5장 이상이면 드로우 불가.

        var cardObject = Instantiate(cardPrefab, handTransform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopCardFromDeck());
        playerHands.Add(card);

        if (handLayout != null)
        {
            handLayout.AddCardToHand(cardObject);
        }
    }

    public CardContent PopCardFromDeck()
    {
        CardContent cardContent = playerDeck[0];
        playerDeck.RemoveAt(0);

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
            isDraggingCard = true;
            draggingCard = card;
            draggingCardRectTransform = card.GetComponent<RectTransform>();
            // draggingCard.SetOrderInLayer(GameRule.MAX_HAND_CARD_NUM + 1);
        }
        else //카드가 손에 있을 때 좌클릭 하면 사용.
        {
            if (!CostManager.Inst.UseCost(card.cardContent.stats.cost)) return;

            isDraggingCard = false;
            UnitManager.Inst.SpawnUnit(draggingCard.cardContent);

            card.slot.isEmpty = true;

            playerHands.Remove(draggingCard);
            Card temp = draggingCard;
            draggingCard = null;
            Destroy(temp.gameObject);
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