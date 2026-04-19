using UnityEngine;

[CreateAssetMenu(fileName = "HitEffectSpawner", menuName = "FX/Hit Effect Spawner")]
public class HitEffectSpawner : ScriptableObject
{
    [Header("Particle Prefab")]
    [SerializeField] private GameObject hitParticlePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnOffset = 0.2f;
    [SerializeField] private float groundYOffset = -0.4f;

    [Header("Colors")]
    [SerializeField] private Color playerHitColor = Color.black;
    [SerializeField] private Color enemyHitColor = Color.white;

    [Header("Ground Plane")]
    [SerializeField] private string groundPlaneChildName = "GroundPlane";

    public void Spawn(Vector3 hitPosition, bool flipX, bool isEnemyHit)
    {
        if (hitParticlePrefab == null) return;

        Vector3 direction = flipX ? Vector3.right : Vector3.left;
        Vector3 spawnPos = hitPosition + direction * spawnOffset;
        Quaternion baseRotation = hitParticlePrefab.transform.rotation;

        GameObject fx = Instantiate(hitParticlePrefab, spawnPos, baseRotation);

        // 방향 반전
        if (flipX)
        {
            ParticleSystem[] systems = fx.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in systems)
            {
                var shape = ps.shape;
                Vector3 rot = shape.rotation;
                rot.y += 180f;
                shape.rotation = rot;
            }
        }

        // ▼ 색상 적용
        Color targetColor = isEnemyHit ? enemyHitColor : playerHitColor;
        ApplyColor(fx, targetColor);

        // Ground Plane 위치 조정
        Transform groundPlane = FindChildRecursive(fx.transform, groundPlaneChildName);
        if (groundPlane != null)
        {
            Vector3 planePos = groundPlane.position;
            planePos.y = hitPosition.y + groundYOffset;
            groundPlane.position = planePos;
        }

        Destroy(fx, 2f);
    }

    private void ApplyColor(GameObject fx, Color color)
    {
        ParticleSystem[] systems = fx.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in systems)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindChildRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }
}