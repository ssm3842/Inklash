using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    // 생성할 카드 프리팹
    public GameObject cardPrefab;

    // 카드들이 생성될 위치 (Hand Panel)
    public Transform handTransform;

    // HandLayout을 참조할 변수 추가
    private HandLayout handLayout;

    void Start()
    {
        // handTransform (Hand Panel)에 붙어있는 HandLayout을 가져옴
        handLayout = handTransform.GetComponent<HandLayout>();

        InitializeGame();
    }

    void InitializeGame()
    {
        StartCoroutine(DrawCardCoroutine());
    }

    IEnumerator DrawCardCoroutine()
    {
        // 핸드에 카드가 5장 미만일 때까지 반복
        while (handTransform.childCount < 5)
        {
            yield return new WaitForSeconds(3f); // 3초 대기
            DrawCard();
        }
        Debug.Log("핸드가 가득 찼습니다.");
    }

    void DrawCard()
    {
        // 1. 카드 프리팹을 handTransform의 자식으로 하여 생성
        GameObject newCard = Instantiate(cardPrefab, handTransform);

        // 2. 생성된 카드를 HandLayout에 등록하여 정렬을 요청
        if (handLayout != null)
        {
            handLayout.AddCardToHand(newCard);
        }
    }
}