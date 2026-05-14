using UnityEngine;
using DG.Tweening;

public class CostBackground : MonoBehaviour
{
    [SerializeField]RectTransform costBackground;

    public void OnCostChanged()
    {
        costBackground.transform.localScale = new Vector3(1f, 1f, 1f);
        costBackground.DOKill();
        costBackground.DOPunchScale(new Vector3(0.12f, 0.12f, 0f), 0.25f, 2, 0.5f); //크기, 시간, 진동, 탄성
    }
}
