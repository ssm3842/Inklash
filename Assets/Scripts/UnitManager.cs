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
        unitSpawnPoint.GetComponent<DamageableObject>().Init(true, new UnitStats(10,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemySpawnPoint.GetComponent<DamageableObject>().Init(false, new UnitStats(10,0,0,0,0));
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void SpawnPlayerUnit(CardContent playerUnit)
    {
        GameObject newUnit = Instantiate(playerUnit.unit, unitSpawnPoint.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
        newUnit.GetComponent<Units>().Init(true, playerUnit.stats);
    }
    public void CastPlayerSpell(CardContent playerUnit)
    {
        GameObject newUnit = Instantiate(playerUnit.unit);
        newUnit.GetComponent<SpellBase>().CastSpell(playerUnit.stats.baseATK, Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        // newUnit.GetComponent<Units>().Init(true, playerUnit);
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, availableEnemies.Count);
            CardContent enemyUnit = availableEnemies[randomIndex];

            GameObject newUnit = Instantiate(enemyUnit.unit, enemySpawnPoint.transform.position + new Vector3(-1, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
            newUnit.GetComponent<Units>().Init(false, enemyUnit.stats);

            yield return new WaitForSeconds(GameRule.ENEMY_SPAWN_SECONDS);
        }
    }
}