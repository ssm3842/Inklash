using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // Hierarchy에 있는 5개의 핸드 슬롯(Hand_1 ~ Hand_5)을 순서대로 여기에 할당.
    public List<GameObject> handSlots;

    // 현재 활성화된 카드의 개수를 추적하는 변수
    private int currentCardCount = 0;

    void Start()
    {
        InitializeHand();
    }

    // 핸드를 초기 상태로 설정하는 함수
    void InitializeHand()
    {
        // 1. 시작할 때 모든 핸드 슬롯을 비활성화하여 숨김.
        foreach (GameObject slot in handSlots)
        {
            slot.SetActive(false);
        }
        
        // 2. 5초마다 카드를 '나타나게' 하는 코루틴을 시작.
        StartCoroutine(RevealCardCoroutine());
    }

    // 5초마다 카드를 하나씩 활성화하는 코루틴
    IEnumerator RevealCardCoroutine()
    {
        // currentCardCount가 최대 슬롯 개수보다 적을 때까지 반복.
        while (currentCardCount < handSlots.Count)
        {
            // 5초를 기다림.
            yield return new WaitForSeconds(5f);

            // 다음 슬롯을 활성화.
            handSlots[currentCardCount].SetActive(true);

            // 활성화된 카드 개수를 1 증가.
            currentCardCount++;
        }
        
        // 모든 카드가 다 채워지면 코루틴이 종료.
        Debug.Log("핸드가 가득 찼습니다.");
    }
}