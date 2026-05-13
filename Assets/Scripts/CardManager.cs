using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Threading.Tasks;

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

    [SerializeField] TextMeshProUGUI drawCountTMP;
    [SerializeField] TextMeshProUGUI graveCountTMP;

    [SerializeField] GameObject cardDescPanel;
    [SerializeField] TextMeshProUGUI cardDescText;

    [SerializeField]SpellAreaViewer spellAreaViewer;
    [SerializeField]RectTransform rect;
    [SerializeField]RectTransform canvasRect;

    public Card draggingCard;
    private CanvasGroup draggingCardCanvasGroupComponent;
    public bool isDraggingCard = false;
    bool isStickyMode = false;

    public void Init() //TODO: 이름 바꾸기
    {
        // List<CardContent> playerCardPool = DataManager.Inst.playerCardDatas.Values.ToList();

        currentBattleDeck = new List<CardContent>(DeckManager.Inst.GetDeckdata());
        discardBattleDeck = new List<CardContent>();

        spellAreaViewer.gameObject.SetActive(false);

        costManager.Init();

        StartBattle();
    }

    void StartBattle()
    {
        for(int i=playerHands.Count-1; i>=0; i--)
        {
            Destroy(playerHands[i].gameObject);
            playerHands.Remove(playerHands[i]);
        }

        Shuffle(currentBattleDeck);

        StartCoroutine(DrawNewHand());
    }

    async Task DrawCard()
    {
        if (playerHands.Count >= GameRule.MAX_HAND_CARD_NUM) return; //플레이어 패가 5장 이상이면 드로우 불가.

        CardContent cardData = await PopCardFromDeck();
        if (cardData == null) return;

        var cardObject = Instantiate(cardPrefab, handLayout.transform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(this, cardData, card.transform.GetSiblingIndex());
        playerHands.Add(card);

        graveCountTMP.text = discardBattleDeck.Count.ToString("D2");
        drawCountTMP.text = currentBattleDeck.Count.ToString("D2");

        handLayout.AlignCards();
    }

    public IEnumerator DrawNewHand() //패가 가득 찰 때까지 카드를 뽑음.
    {   
        // if(!isFree) //전투 시작 시 또는 패를 다 사용했을 때는 비용 없이 카드 다시뽑기.
        // {
        //     if (!costManager.CheckUseCostAvailable(playerHands.Count + 1)) return;
        //     costManager.UseCost(playerHands.Count + 1);
        // }

        for (int i = playerHands.Count - 1; i >= 0; i--) //에러 방지를 위해 역순으로 묘지로 이동
        {
            MoveCardToDiscardDeck(playerHands[i]);
        }

        for (int i = 0; i < GameRule.MAX_HAND_CARD_NUM; i++) //카드 5장 다시 뽑기
        {
            var task = DrawCard();
            yield return new WaitUntil(() => task.IsCompleted);

            yield return new WaitForSeconds(0.1f);
        }
        
        handLayout.AlignCards();
    }

    public IEnumerator RerollAndDraw()
    {
        // 1. 손패의 카드들을 드로우 더미로 직접 합치기
        for (int i = playerHands.Count - 1; i >= 0; i--)
        {
            MoveCardToDiscardDeck(playerHands[i]);
        }
        
        // 2. 버린 카드 더미도 드로우 더미로 합치기
        currentBattleDeck.AddRange(discardBattleDeck);
        discardBattleDeck.Clear();
        
        // 3. 전체 셔플
        Shuffle(currentBattleDeck);
        
        // 4. UI 업데이트
        graveCountTMP.text = discardBattleDeck.Count.ToString("D2");
        drawCountTMP.text = currentBattleDeck.Count.ToString("D2");
        handLayout.AlignCards();
        
        // 5. 5장 새로 뽑기 (대기 없이 즉시)
        for (int i = 0; i < GameRule.MAX_HAND_CARD_NUM; i++)
        {
            var task = DrawCard();
            yield return new WaitUntil(() => task.IsCompleted);
            yield return new WaitForSeconds(0.1f);
        }
        
        handLayout.AlignCards();
    }

    async Task<CardContent> PopCardFromDeck()
    {

        if (currentBattleDeck.Count == 0) return null;
        
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
            UpdateCardAlpha(Input.mousePosition);

            // Sticky 모드일 때 우클릭하면 취소 로직은 기존 CardRightClicked 활용
            if (isStickyMode && Input.GetMouseButtonDown(1))
            {
                CardRightClicked();
            }
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
            draggingCardCanvasGroupComponent.alpha = 1f;

            draggingCard.transform.SetParent(handLayout.transform);
            draggingCard.transform.SetSiblingIndex(draggingCard.originalIndex);

            draggingCard = null;
            draggingCardCanvasGroupComponent = null;
        }
        
        spellAreaViewer.gameObject.SetActive(false);
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

    void Shuffle(List<CardContent> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1); 
            
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
        draggingCardCanvasGroupComponent = draggingCard.GetComponent<CanvasGroup>();
        
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
        UpdateCardAlpha(eventData.position);
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
        spellAreaViewer.gameObject.SetActive(false);
    }

    private void UseSelectedCard(Card card)
    {
        costManager.UseCost(card.cardContent.cost);
        isDraggingCard = false;
        cardUseManager.UseCard(card.cardContent);

        if (card.cardContent.isCopied == true)
        {
            playerHands.Remove(card);
            Destroy(card.gameObject);
        }
        else
        {
            MoveCardToDiscardDeck(card);
        }

        graveCountTMP.text = discardBattleDeck.Count.ToString("D2");
        cardDescPanel.SetActive(false);

        draggingCard = null;
        
        CheckHandLeft();
    }

    void CheckHandLeft()
    {
        if (playerHands.Count <= 0)
        {
            // costManager.AddCost(3);
            StartCoroutine(ReshuffleAndDraw());
        }
        
        handLayout.AlignCards();
    }

    IEnumerator ReshuffleAndDraw()
    {
        // 드로우 더미가 비어있으면 (=한 바퀴 다 돈 상태) 셔플 + 페이백
        if (currentBattleDeck.Count == 0 && discardBattleDeck.Count > 0)
        {
            currentBattleDeck = new List<CardContent>(discardBattleDeck);
            discardBattleDeck.Clear();
            Shuffle(currentBattleDeck);
            
            // costManager.AddCost(3);   // 페이백 (셔플 시작 시 즉시 지급)
            
            graveCountTMP.text = discardBattleDeck.Count.ToString("D2");
            drawCountTMP.text = currentBattleDeck.Count.ToString("D2");
            
            yield return new WaitForSecondsRealtime(1.5f);   // 셔플 대기
        }
        
        StartCoroutine(DrawNewHand());
    }

    // 투명도 조절 로직 분리
    private void UpdateCardAlpha(Vector3 mousePos)
    {
        float targetAlpha = 1f;
        if(draggingCard.transform.position.y >= 350)
        {
            cardDescPanel.SetActive(false);
            targetAlpha = 0.35f;
            if(draggingCard.cardContent.cardType == CardType.Spell)
            {
                targetAlpha = 0f;
                spellAreaViewer.gameObject.SetActive(true);
                spellAreaViewer.SetAreaWidth(draggingCard.cardContent.stats.baseRange);

                float mousePercentX = Input.mousePosition.x / Screen.width;
                float targetX = (mousePercentX - 0.5f) * canvasRect.rect.width;

                rect.anchoredPosition = new Vector2(targetX, -150f);

                // spellAreaViewer.transform.position = new Vector2(mousePos.x, transform.position.y);
                // spellAreaViewer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75f);

            }
        }
        else
        {
            cardDescPanel.SetActive(true);
            spellAreaViewer.gameObject.SetActive(false);
            targetAlpha = 1f;
        }
        
        draggingCardCanvasGroupComponent.alpha = targetAlpha;
    }

    public void OnCardHoverStart(CardContent cardContent, GameObject gameObject)
    {
        cardDescPanel.SetActive(true);
        cardDescPanel.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;
        cardDescText.text = cardContent.description;
    }

    public void OnCardHoverEnd()
    {
        cardDescPanel.SetActive(false);
    }

    public void ExecuteCopyEffect()
    {
        if (draggingCard == null) return;

        CardContent copyContent = new CardContent(draggingCard.cardContent);
        copyContent.cost = 0;

        SealManager.RemoveSealFromCard(copyContent, SealType.Copy);

        var cardObject = Instantiate(cardPrefab, handLayout.transform);
        var card = cardObject.GetComponent<Card>();
        card.Setup(this, copyContent, 0);
        card.cardContent.isCopied = true;

        card.Setup(this, copyContent, 0);
        playerHands.Insert(0, card);
        cardObject.transform.SetAsFirstSibling();

        handLayout.AlignCards();
    }

    public void ExecutePurityEffect()
    {
        if (draggingCard == null) return;

        CardContent originalContent = new CardContent(draggingCard.cardContent);
        SealManager.RemoveSealFromCard(originalContent, SealType.Purity);

        for (int i = playerHands.Count - 1; i >= 0; i--)
        {   
            Card target = playerHands[i];
            if (target == draggingCard) continue;
            if (target.cardContent.isCopied == true)
            {
                playerHands.Remove(target);
                Destroy(target.gameObject);
            }
            else
            {
                MoveCardToDiscardDeck(target);
            }

        }
        
        // 현재 카드의 복제본 5장 생성
        for (int i = 0; i < 5; i++)
        {
            var cardObject = Instantiate(cardPrefab, handLayout.transform);
            var card = cardObject.GetComponent<Card>();
            card.Setup(this, originalContent, i);
            playerHands.Add(card);
            
            card.cardContent.isCopied = true;
        }

        handLayout.AlignCards();
    }

}