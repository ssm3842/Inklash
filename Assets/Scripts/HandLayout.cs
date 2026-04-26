using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLayout : MonoBehaviour
{
    public float radius = 300f; // 부채꼴 반지름
    public float maxRotateAngle = 60f; // 가장 바깥쪽 카드의 최대 회전 각도 (양수)

    public void AlignCards()
    {
        if (transform.childCount <= 0) return;
        
        for(int i =0; i<transform.childCount; i++)
        {
            Card targetCard = transform.GetChild(i).GetComponent<Card>();
            targetCard.originalIndex = transform.GetChild(i).GetSiblingIndex();
        }

        int count = transform.childCount;

        //카드의 위치를 지정하는 부분.
        float[] posArray = new float[count];
        switch (count)
        {
            case 1:
                posArray = new float[] { 0f };
                break;
            case 2:
                posArray = new float[] {-0.1f, 0.1f};
                break;
            case 3:
                posArray = new float[] {-0.2f, 0f, 0.2f};
                break;
            default: // 4장부터는 계산 후 적용.
                float step = 1f / (count + 1); // 1장이면 2로 계산해서 0.5, 2장이면 3으로 계산해 0.33, 0.25, 0.2, 0.16
                for (int i = 0; i < posArray.Length; i++) posArray[i] = -0.5f + step * (i + 1);
                break;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(800 * posArray[i], Mathf.Cos(posArray[i] * Mathf.Deg2Rad * 180f) * 100f, 0);
        }



        //카드의 회전을 담당하는 부분
        if (count == 0) return;

        List<float> angleList = new List<float>();

        switch (count)
        {
            case 1:
                angleList.Add(0f);
                break;
            case 2:
                angleList.Add(-10f);
                angleList.Add(10f);
                break;
            case 3:
                angleList.Add(-15f);
                angleList.Add(0f);
                angleList.Add(15f);
                break;
            default:
                // 4장 이상은 계산 후 적용
                float angleStep = maxRotateAngle / (count - 1);
                for (int i = 0; i < count; i++)
                {
                    float angle = -maxRotateAngle / 2 + angleStep * i;
                    angleList.Add(angle);
                }
                break;
        }

        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, -angleList[i]);
        }
    }
}