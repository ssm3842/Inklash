using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] HandLayout handLayout;
    [SerializeField] Canvas battleUICanvas;
    public CostManager costManager;
    public CardUseManager cardUseManager;

    [SerializeField] List<CardContent> currentBattleDeck;
    [SerializeField] List<CardContent> discardBattleDeck;
    [SerializeField] List<Card> playerHands;

    public Card draggingCard;
    private Image draggingCardImageComponent;
    public bool isDraggingCard = false;
    bool isStickyMode = false;

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
            currentBattleDeck = new List<CardContent>(discardBattleDeck);
            discardBattleDeck.Clear();
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
            MoveCardToDiscardDeck(playerHands[i]);
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
        if (isDraggingCard && draggingCard != null)
        {
            // 드래그 중이거나 Sticky 모드일 때 마우스 추적
            draggingCard.transform.position = Input.mousePosition;
            UpdateCardAlpha();

            // Sticky 모드일 때 우클릭하면 취소 로직은 기존 CardRightClicked 활용
            if (isStickyMode && Input.GetMouseButtonDown(1))
            {
                CardRightClicked();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawNewHand(false);
        }
    }

    public void CardLeftClicked(Card card)
    {
        if (!isDraggingCard) 
        {
            // 1. 아무것도 안 잡고 있을 때 클릭 -> 따라다니기 시작
            StartDraggingCard(card, isSticky: true);
        }
        else if (isStickyMode && draggingCard == card)
        {
            // 2. 이미 Sticky 상태로 잡고 있는 카드를 다시 클릭 -> 내려놓기(사용)
            EndDraggingCard(card);
        }
    }

    public void CardRightClicked()
    {
        if(!isDraggingCard) return;
        
        isDraggingCard = false;

        if(draggingCard)
        {
            Color currentColor = draggingCardImageComponent.color;
            currentColor.a = 1f;
            draggingCardImageComponent.color = currentColor;

            draggingCard.transform.SetParent(handLayout.transform);
            draggingCard.transform.SetSiblingIndex(draggingCard.originalIndex);

            draggingCard = null;
            draggingCardImageComponent = null;
        }
        
        handLayout.AlignCards();
    }

    void MoveCardToDiscardDeck(Card targetCard) //매개변수 카드를 패에서 묘지로 보냄.
    {
        playerHands.Remove(targetCard); //패에서 카드 데이터 제거.
        discardBattleDeck.Add(targetCard.cardContent);

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

    public void StartDraggingCard(Card card, bool isSticky)
    {
        if (!costManager.CheckUseCostAvailable(card.cardContent.cost)) return;

        isDraggingCard = true;
        isStickyMode = isSticky; // 모드 설정
        draggingCard = card;
        draggingCardImageComponent = draggingCard.GetComponent<Image>();
        
        // 드래그 중인 카드가 패의 다른 카드 뒤로 가지 않도록 캔버스 최상단으로 이동
        draggingCard.transform.SetParent(battleUICanvas.transform);
        draggingCard.transform.SetAsLastSibling();
    }

    // 드래그 중 매 프레임 호출
    public void ProcessDraggingCard(PointerEventData eventData)
    {
        if (!isDraggingCard || draggingCard == null) return;

        // 카드 위치를 마우스 위치로 업데이트
        draggingCard.transform.position = eventData.position;

        // 높이에 따른 투명도 조절 (기존 Update 로직 활용)
        UpdateCardAlpha();
    }

    // 마우스를 뗐을 때 호출
    public void EndDraggingCard(Card card)
    {
        if (!isDraggingCard) return;

        // 카드를 사용할 수 있는 높이인지 확인 (예: y > 250)
        if (draggingCard.transform.position.y >= 250)
        {
            // 카드 사용 로직
            UseSelectedCard(card);
        }
        else
        {
            // 사용 취소: 패로 되돌리기
            CardRightClicked(); 
        }
    }

    private void UseSelectedCard(Card card)
    {
        costManager.UseCost(card.cardContent.cost);
        isDraggingCard = false;
        cardUseManager.UseCard(card.cardContent);
        MoveCardToDiscardDeck(card);
        draggingCard = null;
        
        RunManager.Inst.battleManager.OnCardUse();
        if (playerHands.Count <= 0)
        {
            costManager.AddCost(3);
            DrawNewHand(true);
        }
        
        handLayout.AlignCards();
    }

    // 투명도 조절 로직 분리
    private void UpdateCardAlpha()
    {
        float targetAlpha = (draggingCard.transform.position.y >= 250) ? 0.2f : 1f;
        if (!Mathf.Approximately(draggingCardImageComponent.color.a, targetAlpha))
        {
            Color c = draggingCardImageComponent.color;
            c.a = targetAlpha;
            draggingCardImageComponent.color = c;
        }
    }
}