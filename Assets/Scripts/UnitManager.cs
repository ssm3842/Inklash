using Unity.Mathematics;
using System.Collections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] GameObject enemyUnit;

    [SerializeField] GameObject unitSpawnPoint;
    [SerializeField] GameObject enemySpawnPoint;

    public void SpawnUnit(CardContent cardContent)
    {
        GameObject newUnit = Instantiate(cardContent.unit, unitSpawnPoint.transform.position, quaternion.identity);
        newUnit.GetComponent<Units>().Init(true);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
        unitSpawnPoint.GetComponent<Entity>().Init(true);
        enemySpawnPoint.GetComponent<Entity>().Init(false);
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            GameObject newUnit = Instantiate(enemyUnit, enemySpawnPoint.transform.position, quaternion.identity);
            newUnit.GetComponent<Units>().Init(false);
            yield return new WaitForSeconds(5f);
        }
    }
}
