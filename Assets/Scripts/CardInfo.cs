using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardInfo : MonoBehaviour
{
    [SerializeField]Image cardInfoImage;
    [SerializeField]TextMeshProUGUI cardInfoName;
    [SerializeField]TextMeshProUGUI cardInfoCost;
    [SerializeField]TextMeshProUGUI cardInfoATK;
    [SerializeField]TextMeshProUGUI cardInfoHP;
    [SerializeField]TextMeshProUGUI cardInfoATKTerm;
    [SerializeField]TextMeshProUGUI cardInfoSpd;
    [SerializeField]TextMeshProUGUI cardDescription;
    [SerializeField]SealPopupTrigger[] sealImageUI;

    Color defaultCostTextColor;
    Color defaultATKTextColor;
    Color defaultHPTextColor;
    bool defaultTextColorsCached;

    void Awake()
    {
        CacheDefaultTextColors();
    }

    public void Setup(CardContent cardContent)
    {
        HideContent(false);
        
        cardInfoImage.sprite = cardContent.cardImage;
        cardInfoName.text = cardContent.name;
        cardInfoCost.text = cardContent.cost.ToString();
        cardInfoATK.text = cardContent.stats.baseATK.ToString();
        cardInfoHP.text = cardContent.stats.baseMaxHp.ToString();
        cardInfoATKTerm.text = cardContent.stats.baseATKTerm.ToString();
        cardInfoSpd.text = cardContent.stats.baseATKSpd.ToString();
        cardDescription.text = cardContent.description;
        ResetStatTextColors();
        HighlightUpgradedStats(cardContent);

        //인장 이미지 일단 비활성화 후 있으면 활성화
        for(int i=0; i<sealImageUI.Length; i++)
        {
            sealImageUI[i].gameObject.SetActive(false);
        }
        
        //카드에 부착된 인장 수 만큼 반복.
        for(int i=0; i<cardContent.seals.Count; i++)
        {
            sealImageUI[i].gameObject.SetActive(true);
            sealImageUI[i].SetSealData(SpriteDataContainer.Inst.GetSealData(cardContent.seals[i]));
        }
    }
    
    void ResetStatTextColors()
    {
        CacheDefaultTextColors();

        cardInfoCost.color = defaultCostTextColor;
        cardInfoATK.color = defaultATKTextColor;
        cardInfoHP.color = defaultHPTextColor;
    }

    void HighlightUpgradedStats(CardContent cardContent)
    {
        if(cardContent.IsCostUpgraded()) cardInfoCost.color = Color.red;
        if(cardContent.IsATKUpgraded()) cardInfoATK.color = Color.red;
        if(cardContent.IsHPUpgraded()) cardInfoHP.color = Color.red;
    }

    void CacheDefaultTextColors()
    {
        if(defaultTextColorsCached) return;

        defaultCostTextColor = cardInfoCost.color;
        defaultATKTextColor = cardInfoATK.color;
        defaultHPTextColor = cardInfoHP.color;
        defaultTextColorsCached = true;
    }

    public void HideContent(bool isHide)
    {
        if(isHide)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        
    }
}
