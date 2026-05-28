using TMPro;
using UnityEngine;

public class DamageTextCanvas : MonoBehaviour
{
    public static DamageTextCanvas Inst { get; private set; }
    void Awake()
    {
        if(Inst == null) Inst = this;
        else Destroy(gameObject);
    }

    [SerializeField] DamageText damateTextPrefab;

    public void Init()
    {
        foreach(Transform child in transform) Destroy(child.gameObject);
    }

    public void InstDamageText(float amount, Vector3 spawnPos, bool isPlayers, bool isEffectDamage = false, Color? overrideColor = null)
    {
        GameObject newDamageText = Instantiate(damateTextPrefab.gameObject, transform);
        newDamageText.GetComponent<DamageText>().Setup(amount, spawnPos, isPlayers, isEffectDamage, overrideColor);
    }
}
