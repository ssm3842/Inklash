using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] GameObject freezeEffect;

    public void SetFreezeEffect(bool isActivated)
    {
        freezeEffect.SetActive(isActivated);
    }
}
