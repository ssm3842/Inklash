using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void OnButtonClick()
    {
        if(audioClip != null) SFXManager.Inst.PlaySFX(audioClip, 0.7f);
    }
}
