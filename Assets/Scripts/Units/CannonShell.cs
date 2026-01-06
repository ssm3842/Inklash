using UnityEngine;

public class CannonShell : MonoBehaviour
{
    public Vector3 targetPos;
    private Vector3 startPos; 

    private float elapsedTime = 0f;
    [SerializeField] private float lifeTime = 0.2f; 
    [SerializeField] private float rotateSpeed = 720f; 

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (targetPos == null ) return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / lifeTime;

        transform.position = Vector3.Lerp(startPos, targetPos, t);

        transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);

        if (elapsedTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}