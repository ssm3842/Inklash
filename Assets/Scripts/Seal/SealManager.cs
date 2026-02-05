using System;
using System.Collections.Generic;
using UnityEngine;

public class SealManager : MonoBehaviour
{
    public static SealManager Inst { get; private set; }

    private Dictionary<SealType, Type> sealMap;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            InitSealMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 인장 추가시 작성
    private void InitSealMap()
    {
        sealMap = new Dictionary<SealType, Type>
        {
            { SealType.StartCost, typeof(StartManaSeal) },
            { SealType.Test, typeof(TestSeal) }
        };
    }

    public void ApplySeal(GameObject unitObj, SealType type)
    {
        if (type == SealType.None) return;

        if (!sealMap.TryGetValue(type, out Type targetComponentType))
        {
            return;
        }

        // 중복 체크
        if (unitObj.GetComponent(targetComponentType) != null)
        {
            Debug.Log("중복 부착 방지: " + type);
            return;
        }

        // 인장 부착
        unitObj.AddComponent(targetComponentType);
        Debug.Log($"{unitObj.name}에게 {type} 인장");
    }
}