using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLayout : MonoBehaviour
{
    // 카드 사이의 간격, 카드의 넓이, 카드가 얼마나 둥글게 배치될지 등을 설정.
    public float cardSpacing = 150f; // 카드 사이의 간격
    public float arcAmount = 20f;    // 카드가 휘어지는 정도
    public float cardRotation = 10f; // 개별 카드의 기울기

    private List<RectTransform> handCards = new List<RectTransform>();

    // 새로운 카드가 핸드에 추가될 때 호출되는 함수
    public void AddCardToHand(GameObject cardObject)
    {
        // RectTransform 컴포넌트를 가져와 리스트에 추가
        RectTransform cardRect = cardObject.GetComponent<RectTransform>();
        if (cardRect != null)
        {
            handCards.Insert(0, cardRect);
            AlignCards(); // 카드 정렬 함수 호출
        }
    }

    // 카드가 핸드에서 제거될 때 호출되는 함수
    public void RemoveCardFromHand(GameObject cardObject)
    {
        RectTransform cardRect = cardObject.GetComponent<RectTransform>();
        if (cardRect != null && handCards.Contains(cardRect))
        {
            handCards.Remove(cardRect);
            AlignCards(); // 카드 정렬 함수 호출
        }
    }

    // 모든 카드를 정렬하는 핵심 함수
    private void AlignCards()
    {
        // 현재 핸드에 있는 카드의 총 개수
        int cardCount = handCards.Count;
        if (cardCount == 0) return;

        // 전체 핸드가 차지할 총 넓이를 계산
        float totalWidth = (cardCount - 1) * cardSpacing;

        // 첫 번째 카드가 시작될 X 위치를 계산 (중앙 정렬을 위해)
        float startX = -totalWidth / 2f;

        // 모든 카드를 순회하며 위치와 회전을 설정
        for (int i = 0; i < cardCount; i++)
        {
            RectTransform card = handCards[i];
            
            // 1. 위치(Position) 설정
            float xPos = startX + i * cardSpacing;
            // 포물선 형태를 만들기 위해 y 위치 계산 (xPos가 0(중앙)에서 멀어질수록 아래로 내려감)
            float yPos = -Mathf.Abs(xPos) * (arcAmount / 1000f);
            card.anchoredPosition = new Vector2(xPos, yPos);

            // 2. 회전(Rotation) 설정
            // 중앙을 기준으로 얼마나 떨어져 있는지에 따라 회전 각도를 계산
            float normalizedPosition = xPos / (totalWidth / 2f + 1f); // 위치를 -1 ~ 1 사이로 정규화
            float zRot = -normalizedPosition * cardRotation;
            card.localRotation = Quaternion.Euler(0, 0, zRot);
            
            // 3. 순서(Sibling Index) 설정 (나중에 추가되는 카드가 위로 올라오게)
            card.SetSiblingIndex(i);
        }
    }
}