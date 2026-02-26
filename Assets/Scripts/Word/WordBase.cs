using UnityEngine;

public class WordBase : MonoBehaviour
{
    public string cardName;

    public virtual void ApplyBuff(BuffController targetUnit)
    {
        //각각의 버프 구현.
    }
}
