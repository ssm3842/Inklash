using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLayout : MonoBehaviour
{
    public float radius = 300f;       // 부채꼴 반지름
    public float maxRotateAngle = 60f;      // 가장 바깥쪽 카드의 최대 회전 각도 (양수)

    public void AlignCards()
    {
        if (transform.childCount <= 0) return;
        for(int i =0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Card>().originIndex = transform.GetChild(i).GetSiblingIndex();
        }

        //카드의 위치를 지정하는 부분.
        float step = 1f / (transform.childCount + 1); //1장이면 2로 계산해서 0.5, 2장이면 3으로 계산해 0.33

        float[] posBase = new float[transform.childCount];
        for (int i = 0; i < posBase.Length; i++)
        {
            posBase[i] = step * (i + 1) - 0.5f;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(900 * posBase[i] - 5, Mathf.Cos(posBase[i] * Mathf.Deg2Rad * 180f) * 100f, 0);
        }

        //카드의 회전을 담당하는 부분
        int count = transform.childCount;
        if (count == 0) return;

        float angleStep = (count > 1) ? maxRotateAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -maxRotateAngle / 2 + angleStep * i;

            // 부채꼴의 중심에서 각도를 기준으로 위치 계산
            transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}