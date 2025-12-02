using UnityEngine;

public class WordBase : MonoBehaviour
{
    public WordCardType wordCardType;
    public string cardName;

    public virtual void ApplyBuff(BuffController targetUnit)
    {
        //각각의 버프 구현.
    }
}
