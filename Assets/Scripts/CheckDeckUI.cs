using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckDeckUI : MonoBehaviour
{
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]CardInfo cardInfo;

    [SerializeField]TextMeshProUGUI unitCardText;
    [SerializeField]TextMeshProUGUI spellCardText;
    [SerializeField]TextMeshProUGUI wordCardText;
    [SerializeField]TextMeshProUGUI averageCostText;

    public void ViewCheckDeckScreen()
    {
        gameObject.SetActive(true);

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int count = 0;
        int totalCost = 0;
        List<CardContent> deck = RunManager.Inst.deckManager.GetDeckdata();
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Unit)
            {
                count++;
                totalCost += card.cost;

                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
            }
            unitCardText.text = count.ToString();
        }

        count = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Spell)
            {
                count++;
                totalCost += card.cost;

                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
            }
            spellCardText.text = count.ToString();
        }

        count = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Word)
            {
                count++;
                totalCost += card.cost;

                GameObject cardUI = Instantiate(cardPrefab, content);
                cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
            }
            wordCardText.text = count.ToString();
        }

        //반올림 안하고 자리 버림.
        float average = Mathf.Floor((float)totalCost / RunManager.Inst.deckManager.GetDeckdata().Count * 10f);
        averageCostText.text = (average / 10).ToString("F1");

        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((content.childCount - 1) / 5) + 1) * 470 + 50);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
