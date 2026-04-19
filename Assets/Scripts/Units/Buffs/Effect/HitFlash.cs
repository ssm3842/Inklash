using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float peakBlend = 0.1f;

    private static readonly int BlendID = Shader.PropertyToID("_HitEffectBlend");
    private MaterialPropertyBlock mpb;
    private Coroutine routine;

    void Awake()
    {
        mpb = new MaterialPropertyBlock();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            // 0 → 1 → 0 부드럽게
            float v = Mathf.Sin((t / duration) * Mathf.PI) * peakBlend;
            SetBlend(v);
            yield return null;
        }
        SetBlend(0f);
    }

    void SetBlend(float value)
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(BlendID, value);
        sr.SetPropertyBlock(mpb);
    }
}