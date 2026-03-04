using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardInfo : MonoBehaviour
{
    [SerializeField] Image cardInfoImage;
    [SerializeField] TextMeshProUGUI cardInfoName;
    [SerializeField] TextMeshProUGUI cardInfoCost;
    [SerializeField] TextMeshProUGUI cardInfoATK;
    [SerializeField] TextMeshProUGUI cardInfoHP;
    [SerializeField] TextMeshProUGUI cardInfoATKTerm;
    [SerializeField] TextMeshProUGUI cardInfoSpd;

    [SerializeField]Dictionary<SealType, Sprite> sealIconDict;
    [SerializeField]Sprite[] sealSprites;
    [SerializeField]Image[] sealImageUI;

    public void OnEnable()
    {
        sealIconDict = new Dictionary<SealType, Sprite>
        {
            { SealType.Ignite, sealSprites[0] },
            { SealType.ExtraHit, sealSprites[1] },
            { SealType.Cold, sealSprites[2] },
            { SealType.KnockBack, null },
            { SealType.Pierce, null },
        };
    }

    public void Setup(CardContent cardContent)
    {
        cardInfoImage.sprite = cardContent.cardImage;
        cardInfoName.text = cardContent.name;
        cardInfoCost.text = cardContent.cost.ToString();
        cardInfoATK.text = cardContent.stats.baseATK.ToString();
        cardInfoHP.text = cardContent.stats.baseMaxHp.ToString();
        cardInfoATKTerm.text = cardContent.stats.baseATKTerm.ToString();
        cardInfoSpd.text = cardContent.stats.baseATKSpd.ToString();

        //인장 이미지 일단 비활성화 후 있으면 활성화
        for(int i=0; i<sealImageUI.Length; i++)
        {
            sealImageUI[i].gameObject.SetActive(false);
        }
        
        int index = 0;
        foreach (SealType type in System.Enum.GetValues(typeof(SealType)))
        {
            if (type != SealType.None && (cardContent.seals & type) == type)
            {
                sealImageUI[index].gameObject.SetActive(true);
                sealImageUI[index].sprite = sealIconDict[type];
                index++;
            }
        }
    }
}
