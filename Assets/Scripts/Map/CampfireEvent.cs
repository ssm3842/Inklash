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
    [SerializeField]TextMeshProUGUI text;

    [SerializeField]Button confirmButton;

    UpgradeType mapType;
    CardRewardCardUI selectCard;

    public void SetEvent()
    {
        selectSlot.SetActive(false);

        confirmButton.interactable = false;

        mapType = (UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(UpgradeType)).Length);
        selectCard = null;

        //기존에 있던 카드 오브젝트 삭제.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        if(mapType == UpgradeType.UnitHP)
        {
            text.text = "유닛 체력 강화";
        }
        else if(mapType == UpgradeType.UnitATK)
        {
            text.text = "유닛 공격력 강화";
        }
        else if(mapType == UpgradeType.SpellCost)
        {
            text.text = "마법 코스트 감소";
        }

        //카드 타입이 같은 카드만 골라 표시.
        List<CardContent> deck = DeckManager.Inst.GetDeckdata();
        foreach(CardContent card in deck)
        {
            switch(mapType)
            {
                case UpgradeType.UnitHP:
                    if(card.cardType == CardType.Unit)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>(), mapType));
                    }
                    break;
                case UpgradeType.UnitATK:
                    if(card.cardType == CardType.Unit)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>(), mapType));
                    }
                    break;
                case UpgradeType.SpellCost:
                    if(card.cardType == CardType.Spell)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>(), mapType));
                    }
                    break;
            }
        }

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 4) + 1) * 270);
    }

    void SelectCard(CardRewardCardUI targetCard, UpgradeType targetStat)
    {
        selectSlot.SetActive(true);

        if(selectCard == targetCard)
        {
            selectCard.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            selectCard = null;
            selectSlot.SetActive(false);
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
        if(targetStat == UpgradeType.UnitHP)
        {
            hpText.text = (targetCard.cardContent.stats.baseMaxHp + 10).ToString();
            hpText.color = Color.red;
        }
        else if(targetStat == UpgradeType.UnitATK)
        {
            atkText.text = (targetCard.cardContent.stats.baseATK + 5).ToString();
            atkText.color = Color.red;
        }
        else if(targetStat == UpgradeType.SpellCost)
        {
            costText.text = Mathf.Max(0, selectCard.cardContent.cost - 1).ToString();
            costText.color = Color.red;
        }
    }

    public void ConfirmUpgradeCard()
    {
        if(mapType == UpgradeType.UnitHP) selectCard.cardContent.stats.baseMaxHp += 10;
        else if(mapType == UpgradeType.UnitATK) selectCard.cardContent.stats.baseATK += 5;
        else if(mapType == UpgradeType.SpellCost) selectCard.cardContent.cost = Mathf.Max(0, selectCard.cardContent.cost - 1);

        eventManager._OnEventEnd();
    }

    public enum UpgradeType
    {
        UnitATK, UnitHP, SpellCost,
    }    
}
