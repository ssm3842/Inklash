using UnityEngine;
using DG.Tweening;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Inst { get; private set; }

    AudioSource bgmManagerComponent;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;

        bgmManagerComponent = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (bgmManagerComponent.clip == clip) return; // 이미 같은 곡이 재생 중이면 무시

        bgmManagerComponent.clip = clip;
        bgmManagerComponent.volume = volume;
        bgmManagerComponent.Play();
    }

    public void StopBGM()
    {
        bgmManagerComponent.Stop();
    }
    
    public void ChangeBGM(AudioClip newClip, float volume = 1f, float fadeDuration = 1f)
    {
        bgmManagerComponent.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            bgmManagerComponent.clip = newClip;
            bgmManagerComponent.Play();
            bgmManagerComponent.DOFade(volume, fadeDuration).SetUpdate(true);
        });
    }
}