using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 targetPos;
    Vector3 middlePoint;

    bool isReachPeak = false;

    void Update()
    {
        if (targetPos == null) return;

        transform.Translate(
            (targetPos.x - transform.position.x) * Time.deltaTime * 6f,
            isReachPeak ? targetPos.y + 1f * Time.deltaTime * 50f : targetPos.y * Time.deltaTime * 50f,
            0);

        if (transform.position.x >= (transform.position.x + targetPos.x)/2f) isReachPeak = true;
    }

    public void _OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
