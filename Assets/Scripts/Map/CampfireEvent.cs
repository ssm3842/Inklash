using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEvent : MonoBehaviour
{
    [SerializeField]EventManager eventManager;

    [SerializeField]GameObject selectSlot;

    [SerializeField]CardRewardCardUI cardPreviewBefore;
    [SerializeField]CardRewardCardUI cardPreviewAfter;
    [SerializeField]TextMeshProUGUI atkText;
    [SerializeField]TextMeshProUGUI hpText;
    [SerializeField]TextMeshProUGUI costText;

    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]Image eventStone;
    [SerializeField]Sprite eventStoneDeactivated;
    [SerializeField]Sprite eventStoneActivated;

    [SerializeField]Image atkButton;
    [SerializeField]Image hpButton;
    [SerializeField]Image costButton;

    [SerializeField]Button confirmButton;

    UpgradeType upgradeType = UpgradeType.CardATK;
    CardRewardCardUI selectCard;

    public void SetEvent()
    {
        selectSlot.SetActive(false);

        confirmButton.interactable = false;
        eventStone.sprite = eventStoneDeactivated;

        selectCard = null;
        _OnATKButtonClicked();

        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        //카드 타입이 같은 카드만 골라 표시.
        List<CardContent> deck = DeckManager.Inst.GetDeckdata();
        foreach(CardContent card in deck)
        {
            if(card.cardType != CardType.Word)
            {
                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>()));
            }
        }

        FilterScrollView();
    }

    void FilterScrollView()
    {
        int activatedCount = 0;

        foreach (Transform child in content)
        {
            child.gameObject.SetActive(false);

            if(child.gameObject.GetComponent<CardRewardCardUI>().cardContent.cardType == CardType.Spell && upgradeType == UpgradeType.CardHP) continue;
            child.gameObject.SetActive(true);
            activatedCount++;
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((activatedCount - 1) / 4) + 1) * 270);
    }

    void SelectCard(CardRewardCardUI targetCard)
    {
        selectSlot.SetActive(true);

        if(selectCard == targetCard)
        {
            selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            selectCard = null;
            selectSlot.SetActive(false);
            eventStone.sprite = eventStoneDeactivated;
            return;
        }

        //이미 선택된 카드를 선택해제.
        if(selectCard != null)selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        selectCard = targetCard;
        selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 0.3f;

        confirmButton.interactable = selectCard != null;

        hpText.color = Color.black;
        atkText.color = Color.black;
        costText.color = Color.white;

        cardPreviewBefore.Setup(targetCard.cardContent);
        cardPreviewAfter.Setup(targetCard.cardContent);
        eventStone.sprite = eventStoneActivated;

        ChangePreview();
    }

    void ChangePreview()
    {   
        if(selectCard == null) return;
        if(upgradeType == UpgradeType.CardATK)
        {
            if(selectCard.cardContent.id == "Freeze") atkText.text = (selectCard.cardContent.stats.baseATK + 3).ToString();
            else atkText.text = (selectCard.cardContent.stats.baseATK + 5).ToString();
            hpText.text = selectCard.cardContent.stats.baseMaxHp.ToString();
            costText.text = selectCard.cardContent.cost.ToString();

            atkText.color = Color.red;
            hpText.color = Color.black;
            costText.color = Color.white;
        }
        else if(upgradeType == UpgradeType.CardHP)
        {
            atkText.text = selectCard.cardContent.stats.baseATK.ToString();
            hpText.text = (selectCard.cardContent.stats.baseMaxHp + 10).ToString();
            costText.text = selectCard.cardContent.cost.ToString();

            atkText.color = Color.black;
            hpText.color = Color.red;
            costText.color = Color.white;
        }
        else if(upgradeType == UpgradeType.CardCost)
        {
            atkText.text = selectCard.cardContent.stats.baseATK.ToString();
            hpText.text = selectCard.cardContent.stats.baseMaxHp.ToString();
            costText.text = Mathf.Max(0, selectCard.cardContent.cost - 1).ToString();

            atkText.color = Color.black;
            hpText.color = Color.black;
            costText.color = Color.red;
        }
    }

    public void ConfirmUpgradeCard()
    {
        if(upgradeType == UpgradeType.CardATK) 
        {
            if(selectCard.cardContent.id == "Freeze") selectCard.cardContent.stats.baseMaxHp += 3;
            else selectCard.cardContent.stats.baseATK += 10;
        }
        else if(upgradeType == UpgradeType.CardHP) selectCard.cardContent.stats.baseMaxHp += 10;
        else if(upgradeType == UpgradeType.CardCost) selectCard.cardContent.cost = Mathf.Max(0, selectCard.cardContent.cost - 1);

        eventManager._OnEventEnd();
    }

    void OnUpgradeTypeButtonClicked(UpgradeType newUpgradeType)
    {
        upgradeType = newUpgradeType;
        FilterScrollView();
        ChangePreview();
    }
    public void _OnATKButtonClicked()
    {
        OnUpgradeTypeButtonClicked(UpgradeType.CardATK);
        atkButton.color = Color.red;
        hpButton.color = Color.white;
        costButton.color = Color.white;
    }
    public void _OnHPButtonClicked()
    {
        OnUpgradeTypeButtonClicked(UpgradeType.CardHP);
        if(selectCard != null && selectCard.cardContent.cardType == CardType.Spell) 
        {
            selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            selectSlot.SetActive(false);
            selectCard = null;
            eventStone.sprite = eventStoneDeactivated;
        }
        atkButton.color = Color.white;
        hpButton.color = Color.red;
        costButton.color = Color.white;
    }
    public void _OnCostButtonClicked()
    {
        OnUpgradeTypeButtonClicked(UpgradeType.CardCost);
        atkButton.color = Color.white;
        hpButton.color = Color.white;
        costButton.color = Color.red;
    }

    public enum UpgradeType
    {
        CardATK, CardHP, CardCost,
    }    
}
