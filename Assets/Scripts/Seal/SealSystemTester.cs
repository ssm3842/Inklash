using UnityEngine;
using System.Collections.Generic;

public class SealSystemTester : MonoBehaviour
{
    [Header("테스트 설정")]
    [Tooltip("Q키로 박을 인장 타입")]
    public SealType qSeal = SealType.StartCost;
    [Tooltip("W키로 박을 인장 타입")]
    public SealType wSeal = SealType.Test; 

    void Update()
    {
        // Q, W 키로 인장 각인 테스트 (덱의 0번 카드 기준)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddSealToCard(0, qSeal);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddSealToCard(0, wSeal);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnCardByIndex(0);
        }

    }

    private void AddSealToCard(int cardIndex, SealType type)
    {
        if (type == SealType.None) return;

        // RunManager를 통해 현재 덱 데이터를 가져옴
        var deck = RunManager.Inst.deckManager.GetDeckdata();
        
        if (deck.Count > cardIndex)
        {
            CardContent targetCard = deck[cardIndex];

            // 중복 체크: 이미 같은 종류의 인장이 데이터 리스트에 있는지 확인
            if (!targetCard.seals.Contains(type))
            {
                targetCard.seals.Add(type);
                Debug.Log($"<color=cyan>[각인 성공]</color> {targetCard.name} 카드에 {type} 인장을 추가했습니다. (총 인장 수: {targetCard.seals.Count})");
            }
            else
            {
                Debug.LogWarning($"<color=yellow>[각인 실패]</color> {targetCard.name}에는 이미 {type} 인장이 존재합니다.");
            }
        }
    }

    private void SpawnCardByIndex(int index)
    {
        var deck = RunManager.Inst.deckManager.GetDeckdata();
        
        if (index < deck.Count)
        {
            CardContent cardToUse = deck[index];
            Debug.Log($"<color=green>[테스트 소환]</color> {index + 1}번 키 입력: {cardToUse.name} 소환을 시도합니다.");
            
            // CardUseManager를 호출하여 소환 로직 실행 (SealManager.ApplySeal이 여기서 호출됨)
            RunManager.Inst.battleManager.cardUseManager.UseCard(cardToUse);
        }
        else
        {
            Debug.LogWarning($"덱에 {index + 1}번째 카드가 존재하지 않습니다.");
        }
    }
}