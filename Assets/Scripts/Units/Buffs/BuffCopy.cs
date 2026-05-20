public class BuffCopy : Buffs
{
    public BuffCopy()
    {
        buffName = "Copy";
        remainTime = -1; 
    }

    protected override void ApplyCardSystem(CardManager cardManager)
    {
        cardManager.isCopy = true;
    }
}