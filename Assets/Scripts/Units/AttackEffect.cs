using UnityEngine;

public abstract class AttackEffect : MonoBehaviour
{
    // 패킷에 자신의 효과를 써넣는 함수
    public abstract void ApplyEffect(HitEffectPacket packet);
}