public class BuffPurity : Buffs
{
    public BuffPurity()
    {
        buffName = "Purity";
        remainTime = -1;
    }

    protected override void ApplyCardSystem(CardManager cardManager)
    {
        cardManager.ExecutePurityEffect();
    }
}