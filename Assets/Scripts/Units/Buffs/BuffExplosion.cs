public class BuffExplosion : Buffs
{
    public BuffExplosion()
    {
        buffName = "Explosion";
        remainTime = -1; // 죽을 때까지 유지
    }
    protected override void ApplyUnit(Units unit)
    {
            unit.hasExplosion = true;
    }
}   