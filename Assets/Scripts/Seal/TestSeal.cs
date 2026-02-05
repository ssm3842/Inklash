using UnityEngine;

// 두 번째 인장 테스트를 위한 스크립트
public class TestSeal : UnitSeal
{
    private void Start()
    {
        transform.localScale *= 1.5f;
        Debug.Log($"{gameObject.name}: 테스트용 인장");
    }
}