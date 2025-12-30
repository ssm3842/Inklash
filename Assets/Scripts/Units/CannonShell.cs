using UnityEngine;

public class CannonShell : MonoBehaviour
{
    public Vector3 targetPos;
    private Vector3 startPos; 

    private float elapsedTime = 0f;
    [SerializeField] private float lifeTime = 0.2f; 

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (targetPos == Vector3.zero) return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / lifeTime;

        transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    public void _OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
