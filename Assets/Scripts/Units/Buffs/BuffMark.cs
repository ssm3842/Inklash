using UnityEngine;

public class BuffMarker : Buffs
{
    public BuffMarker()
    {
        buffName = "Marker";
        remainTime = -1; 
    }
}

public class BuffMarking : Buffs
{
    public BuffMarking(float duration = 5f)
    {
        buffName = "Marking";
        remainTime = duration; 
    }
}