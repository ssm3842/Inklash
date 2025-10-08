using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooms", menuName ="Scriptable Object/Rooms")]
public class RoomsSO : ScriptableObject
{
    public RoomContent[] cardContents;
}

