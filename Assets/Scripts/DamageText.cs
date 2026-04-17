using UnityEngine;
using TMPro;
using DG.Tweening; // DoTween 사용 시

public class DamageText : MonoBehaviour
{
    const float lifeTime = 1f;
    public void Setup(float damage, Vector3 startPos, Color textColor)
    {
        TextMeshProUGUI tmpro = GetComponent<TextMeshProUGUI>();

        tmpro.text = damage.ToString("F0");
        tmpro.alpha = 1f;
        tmpro.color = textColor;
        transform.position = startPos + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.2f, 0.4f), 0f);

        // 위로 솟구치며 사라지는 연출
        transform.DOMoveY(startPos.y + 1f, lifeTime).SetLink(gameObject); // lifeTime만큼의 초 동안 위로 이동
        tmpro.DOFade(0f, lifeTime).SetEase(Ease.InQuad).SetLink(gameObject).OnComplete(() => {
            if(this != null && gameObject != null) Destroy(gameObject);
        });
    }
}