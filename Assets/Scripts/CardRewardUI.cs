using System.Collections.Generic;
using UnityEngine;

public class CardRewardUI : MonoBehaviour
{
    bool isRewardSetted = false;

    List<CardContent> rewards;

    [SerializeField]GameObject cardRewardUIPrefab;
    [SerializeField]GameObject cardRewardUIContainer;
    [SerializeField]CardDataLinkSO cardLinkSo;
    List<CardDataSO> cardData;

    public void SetReward()
    {   
        //보상이 이미 설정된 상태라면 생략.
        if(isRewardSetted) return;

        cardData = new List<CardDataSO>();

        //보상 풀 설정
        foreach(CardLink data in cardLinkSo.playerUnits)
        {
            cardData.Add(data.cardContents);
        }
        foreach(CardLink data in cardLinkSo.playerSpells)
        {
            cardData.Add(data.cardContents);
        }
        foreach(CardLink data in cardLinkSo.playerWords)
        {
            cardData.Add(data.cardContents);
        }

        isRewardSetted = true;

        for(int i=0; i<3; i++)
        {
            int randomI = Random.Range(0, cardData.Count);

            GameObject newCardReward = Instantiate(cardRewardUIPrefab);
            newCardReward.transform.SetParent(cardRewardUIContainer.transform);
            //newCardReward에서 컴포넌트 가져와서 셋업.
            //클릭에 리스너 설정해서 보상 클릭 시 덱매니저에 전달.

            // cardData[randomI];
            // rewards.Add(cardData.)

            gameObject.SetActive(true);
        }
    }
}
