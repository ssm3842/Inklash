using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    float spawnHeight = 0.3f;
    [SerializeField] GameObject unitSpawnPoint;
    [SerializeField] GameObject enemySpawnPoint;
    private List<CardContent> availableEnemies;

    List<WordBase> stackedWordCardEffect;

    public void InitUnitManager(List<CardContent> enemyPool)
    {
        // availableEnemies = new List<CardContent>(enemyPool);
        availableEnemies = new List<CardContent>(RunManager.Inst.unitDataManager.enemyUnitDatas.Values);
        stackedWordCardEffect = new List<WordBase>();

        unitSpawnPoint.GetComponent<DamageableObject>().Init(true, new UnitStats(10,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemySpawnPoint.GetComponent<DamageableObject>().Init(false, new UnitStats(10,0,0,0,0));
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void SpawnPlayerUnit(CardContent card)
    {
        FilterWordCard(WordCardType.Unit);

        GameObject newUnit = Instantiate(card.unit, unitSpawnPoint.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
        newUnit.GetComponent<Units>().Init(true, card.stats);

        foreach(WordBase wordCard in stackedWordCardEffect)
        {
            wordCard.ApplyBuff(newUnit.GetComponent<BuffController>());
        }
        stackedWordCardEffect = new List<WordBase>();
    }
    public void CastPlayerSpell(CardContent card)
    {
        FilterWordCard(WordCardType.Spell);

        GameObject newUnit = Instantiate(card.unit);
        newUnit.GetComponent<SpellBase>().CastSpell(card.stats.baseATK, Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
    }
    public void AddWordCard(CardContent card)
    {
        WordBase targetWordCard = card.unit.GetComponent<WordBase>();
        //쌓인 카드 효과중 중복이 있으면 아무효과 X.
        foreach(WordBase type in stackedWordCardEffect)
        {
            if(type.cardName == targetWordCard.cardName) return;
        }
        stackedWordCardEffect.Add(targetWordCard); //중복이 아니면 효과 스택.
    }

    void FilterWordCard(WordCardType wordCardType)
    {
        List<WordBase> toRemove = new List<WordBase>();

        foreach(WordBase wordcard in stackedWordCardEffect)
        {
            if(wordcard.wordCardType != wordCardType)
            {
                toRemove.Add(wordcard);
            }
        }

        foreach(WordBase wordcard in toRemove)
        {
            stackedWordCardEffect.Remove(wordcard);
        }
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