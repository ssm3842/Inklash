using UnityEngine;

public class Arrow : MonoBehaviour
{

    public Vector3 targetPos;
    public Vector3 startPos;

    float elapsedTime = 0f;
    float lifeTime = 1f;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (targetPos == null) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / lifeTime);

        if (elapsedTime < lifeTime / 2f) transform.position = new Vector3(
            Mathf.Lerp(startPos.x, targetPos.x, t),
            Mathf.Lerp(startPos.y, startPos.y + 4f, t % 2f), 0);
        else transform.position = new Vector3(
            Mathf.Lerp(startPos.x, targetPos.x, t),
            Mathf.Lerp(startPos.y + 4f, startPos.y, t % 2f), 0);
        

        //     (targetPos.x - transform.position.x),
        //     isReachPeak ? targetPos.y + 1f * Time.deltaTime * 50f : targetPos.y * Time.deltaTime * 50f,
        //     0);

        // if (transform.position.x >= (transform.position.x + targetPos.x)/2f) isReachPeak = true;
    }

    public void _OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
