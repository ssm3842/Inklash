using UnityEngine;

[CreateAssetMenu(fileName = "SealData", menuName ="Scriptable Object/SealData")]
public class SealDataSO : ScriptableObject
{
    public string sealName;
    [TextArea(2, 10)]
    public string sealDescription;

    public Sprite sealIcon; 
}
