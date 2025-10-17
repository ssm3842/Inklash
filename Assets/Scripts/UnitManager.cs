using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] GameObject unitSpawnPoint;
    [SerializeField] GameObject enemySpawnPoint;
    private List<CardContent> availableEnemies;

    public void SpawnUnit(CardContent playerUnit)
    {
        GameObject newUnit = Instantiate(playerUnit.unit, unitSpawnPoint.transform.position + new Vector3(0, Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
        newUnit.GetComponent<Units>().Init(true, playerUnit);
    }

    void Start()
    {
        availableEnemies = new List<CardContent>(DataManager.Inst.enemyUnitDatas.Values);
        unitSpawnPoint.GetComponent<Entity>().Init(true);
        enemySpawnPoint.GetComponent<Entity>().Init(false);
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableEnemies.Count);
            CardContent enemyUnit = availableEnemies[randomIndex];

            GameObject newUnit = Instantiate(enemyUnit.unit, enemySpawnPoint.transform.position - new Vector3(1, Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
            newUnit.GetComponent<Units>().Init(false, enemyUnit);

            yield return new WaitForSeconds(3f);
        }
    }
}