public class BuffSplit : Buffs
{
    private int splitCount;

    public BuffSplit()
    {
        buffName = "Split";
        remainTime = -1;    
    }
    protected override void ApplyUnit(Units unit)
    {
            unit.hasSplit = true;
    }
}