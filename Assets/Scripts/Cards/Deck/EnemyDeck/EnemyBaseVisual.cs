using UnityEngine;

public class EnemyBaseVisual : MonoBehaviour
{
    [SerializeField] Transform visualRoot;   // 프리팹이 꽂힐 자리
    [SerializeField] Transform healthBar;
    GameObject currentVisual;

    public void SetVisual(GameObject visualPrefab)
    {
        // 시각만 파괴 — 루트/콜라이더/스폰 기준은 건드리지 않음
        if (currentVisual != null) Destroy(currentVisual);
        if (visualPrefab == null) return;

        // 프리팹의 로컬 트랜스폼이 그대로 유지됨 (worldPositionStays = false)
        currentVisual = Instantiate(visualPrefab, visualRoot);

        var refs = currentVisual.GetComponent<EnemyBaseVisualRefs>();
        if (refs != null && refs.healthBarAnchor != null)
            GetComponent<DamageableObject>().RepositionHealthBar(refs.healthBarAnchor.position);
    }
}
