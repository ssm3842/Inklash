using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    float spawnHeight = 0.3f;
    [SerializeField] GameObject unitSpawnPoint;
    [SerializeField] GameObject enemySpawnPoint;
    private List<CardContent> availableEnemies;

    public void InitUnitManager(List<CardContent> enemyPool)
    {
        // availableEnemies = new List<CardContent>(enemyPool);
        availableEnemies = new List<CardContent>(RunManager.Inst.unitDataManager.enemyUnitDatas.Values);
        unitSpawnPoint.GetComponent<Entity>().Init(true);
        enemySpawnPoint.GetComponent<Entity>().Init(false);
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void SpawnPlayerUnit(CardContent playerUnit)
    {
        GameObject newUnit = Instantiate(playerUnit.unit, unitSpawnPoint.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight), 0), Quaternion.identity);
        newUnit.GetComponent<Units>().Init(true, playerUnit);
    }
    public void CastPlayerSpell(CardContent playerUnit)
    {
        GameObject newUnit = Instantiate(playerUnit.unit);
        newUnit.GetComponent<Lightning>().CastSpell(playerUnit.stats.atk, Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        // newUnit.GetComponent<Units>().Init(true, playerUnit);
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, availableEnemies.Count);
            CardContent enemyUnit = availableEnemies[randomIndex];

            GameObject newUnit = Instantiate(enemyUnit.unit, enemySpawnPoint.transform.position - new Vector3(1, Random.Range(-spawnHeight, spawnHeight), 0), Quaternion.identity);
            newUnit.GetComponent<Units>().Init(false, enemyUnit);

            yield return new WaitForSeconds(GameRule.ENEMY_SPAWN_SECONDS);
        }
    }
}