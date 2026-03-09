using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckDeckUI : MonoBehaviour
{
    [SerializeField]Transform content;
    [SerializeField]GameObject cardPrefab;

    [SerializeField]CardInfo cardInfo;

    [SerializeField]TextMeshProUGUI allCardText;
    [SerializeField]TextMeshProUGUI unitCardText;
    [SerializeField]TextMeshProUGUI spellCardText;
    [SerializeField]TextMeshProUGUI wordCardText;
    [SerializeField]TextMeshProUGUI averageCostText;

    public void OpenCheckDeckScreen()
    {
        gameObject.SetActive(true);

        InitText();

        SetScrollViewType(0);
    }

    void InitText()
    {
        int count = 0;
        int totalCost = 0;

        List<CardContent> deck = DeckManager.Inst.GetDeckdata();
        allCardText.text = deck.Count.ToString();
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Unit)
            {
                count++;
                totalCost += card.cost;
            }
        }
        unitCardText.text = count.ToString();

        count = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Spell)
            {
                count++;
                totalCost += card.cost;
            }
        }
        spellCardText.text = count.ToString();

        count = 0;
        foreach(CardContent card in deck)
        {
            if(card.cardType == CardType.Word)
            {
                count++;
                totalCost += card.cost;
            }
        }
        wordCardText.text = count.ToString();

        //반올림 안하고 자리 버림.
        float average = Mathf.Floor((float)totalCost /DeckManager.Inst.GetDeckdata().Count * 10f);
        averageCostText.text = (average / 10).ToString("F1");
    }

    public void SetScrollViewType(int typeIndex)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        List<CardContent> deck = DeckManager.Inst.GetDeckdata();
        
        int spawnedCount = 0;
        switch(typeIndex)
        {
            case 0: //모두 보기
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Unit)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Spell)
                    {

                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Word)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                break;
            case 1: //유닛만 보기
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Unit)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                break;
            case 2: //마법만 보기
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Spell)
                    {

                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                break;
            case 3: //단어만 보기
                foreach(CardContent card in deck)
                {
                    if(card.cardType == CardType.Word)
                    {
                        GameObject cardUI = Instantiate(cardPrefab, content);
                        cardUI.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        cardUI.GetComponent<CardRewardCardUI>().Setup(card);
                        cardUI.GetComponent<Button>().onClick.AddListener(() => cardInfo.Setup(card));
                        spawnedCount++;
                    }
                }
                break;
        }
        //카드 수에 따라 스크롤 뷰 높이를 변경.
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((spawnedCount - 1) / 4) + 1) * 370);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
