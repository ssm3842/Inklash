using UnityEngine;
using TMPro;
using DG.Tweening; // DoTween 사용 시

public class DamageText : MonoBehaviour
{
    const float lifeTime = 1f;

    [Header("Outline presets")]
    [SerializeField] Material allyOutlineMat;    
    [SerializeField] Material enemyOutlineMat;  

    [Header("Big hit")]
    [SerializeField] float bigDamageThreshold = 15f;
    [SerializeField] Color bigDamageColor = new Color(1f, 0.45f, 0.15f); // 주황빛
    [SerializeField] Color effectDamageColor = Color.red;
    [SerializeField] float normalFontSize = 36f;
    [SerializeField] float bigFontSize    = 52f;

    [Header("Spawn FX")]
    [SerializeField] float popInTime = 0.18f;
    [SerializeField] float driftX    = 0.25f;

    public void Setup(float damage, Vector3 startPos, bool isPlayers, bool isEffectDamage = false, Color? overrideColor = null)
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.text  = damage.ToString("F0");
        tmp.alpha = 1f;

        bool isBig = damage >= bigDamageThreshold;

        // 1) & 3) 외곽선 + 아군/적군 기본 색
        if (isPlayers)
        {
            tmp.fontSharedMaterial = allyOutlineMat; 
            tmp.color = Color.black;                  
        }
        else
        {
            tmp.fontSharedMaterial = enemyOutlineMat; 
            tmp.color = Color.white;                 
        }

        // 2) 센 데미지: 기울임 + 색상 변화 (외곽선은 아군/적군 그대로 유지 → 대비 유지)
        if (damage >= bigDamageThreshold)
        {
            tmp.fontStyle = FontStyles.Italic | FontStyles.Bold;
            tmp.color     = bigDamageColor;
            tmp.fontSize  = bigFontSize;
        }
        else
        {
            tmp.fontStyle = FontStyles.Normal;
            tmp.fontSize  = normalFontSize;
        }

        if (overrideColor.HasValue)
        {
            tmp.color = overrideColor.Value;
        }
        else if (isEffectDamage)
        {
            tmp.color = effectDamageColor;
        }

        transform.position = startPos + new Vector3(Random.Range(-0.3f, 0.3f),
                                                    Random.Range(0.2f, 0.4f), 0f);

        
        PlaySpawnFX(tmp, isBig);
    }

    void PlaySpawnFX(TextMeshProUGUI tmp, bool isBig)
    {
        Vector3 baseScale = transform.localScale;   // ← 프리팹 원래 스케일 보존

        float life  = isBig ? lifeTime * 1.3f : lifeTime;
        float popTo = isBig ? 1.25f : 1f;
        float riseH = isBig ? 1.4f  : 0.9f;

        transform.localEulerAngles = new Vector3(0, 0, Random.Range(-7f, 7f));

        // 1) 팝인 — baseScale 기준으로 (절대값 1 금지!)
        transform.localScale = baseScale * 0.2f;
        Sequence scaleSeq = DOTween.Sequence().SetLink(gameObject);
        scaleSeq.Append(transform.DOScale(baseScale * popTo, popInTime).SetEase(Ease.OutBack, 3f));
        if (isBig) scaleSeq.Append(transform.DOPunchScale(baseScale * 0.3f, 0.25f, 8, 0.7f));

        // 2) 위로 솟구치며 감속 + 3) 좌우 분산
        transform.DOMoveY(transform.position.y + riseH, life).SetEase(Ease.OutCubic).SetLink(gameObject);
        transform.DOMoveX(transform.position.x + Random.Range(-driftX, driftX), life)
                .SetEase(Ease.OutCubic).SetLink(gameObject);

        // 4) 또렷하게 보여주다 후반에 페이드아웃
        tmp.DOFade(0f, life * 0.4f).SetEase(Ease.InQuad).SetDelay(life * 0.6f).SetLink(gameObject)
        .OnComplete(() => { if (this != null && gameObject != null) Destroy(gameObject); });
    }
}
