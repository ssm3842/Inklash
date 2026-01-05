using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomEventData", menuName ="Scriptable Object/RandomEventData")]
public class RandomEventSO : ScriptableObject
{
    public string eventID;
    public Sprite eventImage;
    public string eventDescription;


}

