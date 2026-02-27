using UnityEngine;
using TMPro;
using DG.Tweening; // DoTween 사용 시

public class DamageText : MonoBehaviour
{
    const float lifeTime = 1f;
    public void Setup(float damage, Vector3 startPos, Color textColor)
    {
        TextMeshProUGUI tmpro = GetComponent<TextMeshProUGUI>();

        tmpro.text = damage.ToString();
        tmpro.alpha = 1f;
        tmpro.color = textColor;
        transform.position = startPos;

        // 위로 솟구치며 사라지는 연출
        transform.DOMoveY(startPos.y + 1f, lifeTime).SetLink(gameObject); // 0.5초 동안 위로 1만큼 이동
        tmpro.DOFade(0f, lifeTime).SetLink(gameObject).OnComplete(() => {
            // 연출이 끝나면 오브젝트 풀로 반환
            if(this != null && gameObject != null) Destroy(gameObject);
        });
    }
}