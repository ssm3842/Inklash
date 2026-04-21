using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Inst { get; private set; }

    [SerializeField]GameObject audioSourcePrefab;
    [SerializeField]int poolSize = 20;

    List<AudioSource> sourcePool = new List<AudioSource>();

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;

        DontDestroyOnLoad(gameObject);

        // 미리 소스들을 만들어둠 (오디오 풀링)
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            AudioSource source = obj.GetComponent<AudioSource>();
            source.playOnAwake = false;
            obj.SetActive(false);
            sourcePool.Add(source);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1.0f, float pitchVariation = 0.1f)
    {
        if (clip == null) return;

        AudioSource availableSource = GetAvailableSource();
        if (availableSource != null)
        {
            availableSource.gameObject.SetActive(true);
            availableSource.clip = clip;
            
            // 귀의 피로도를 줄이기 위해 피치(높낮이)에 살짝 변동을 줌
            availableSource.pitch = 1.0f + Random.Range(-pitchVariation, pitchVariation);
            availableSource.volume = volume;
            
            availableSource.Play();
            
            // 소리가 끝나면 비활성화 (코루틴이나 Invoke 사용)
            StartCoroutine(DisableSourceAfterPlaying(availableSource));
        }
    }

    AudioSource GetAvailableSource()
    {
        foreach (var source in sourcePool)
        {
            if (!source.gameObject.activeSelf) return source;
        }
        // 모든 소스가 사용 중이면 가장 오래된 걸 뺏거나 무시 (여기선 무시)
        return null;
    }

    System.Collections.IEnumerator DisableSourceAfterPlaying(AudioSource source)
    {
        yield return new WaitForSecondsRealtime(source.clip.length);
        source.gameObject.SetActive(false);
    }
}