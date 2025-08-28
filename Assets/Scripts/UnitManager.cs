using Unity.Mathematics;
using System.Collections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] GameObject enemyUnit;

    [SerializeField] Transform unitSpawnPoint;
    [SerializeField] Transform enemySpawnPoint;

    public void SpawnUnit(CardContent cardContent)
    {
        GameObject newUnit = Instantiate(cardContent.unit, unitSpawnPoint.position, quaternion.identity);
        newUnit.GetComponent<Units>().Init(true);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            GameObject newUnit = Instantiate(enemyUnit, enemySpawnPoint.position, quaternion.identity);
            newUnit.GetComponent<Units>().Init(false);
            yield return new WaitForSeconds(5f);
        }
    }
}
