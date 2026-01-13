using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUseManager : MonoBehaviour
{
    float spawnHeight = 0.3f;
    [SerializeField] GameObject playerBase;
    [SerializeField] GameObject enemyBase;
    [SerializeField] EnemyBaseDataSO[] enemyBaseDatas;

    List<CardContent> availableEnemies;

    List<WordBase> stackedWordCardEffect;

    public Coroutine enemySpawnCoroutine = null;

    bool isCloneCardUsed = false;

    public void InitUnitManager()
    {
        //적 데이터 중에서 하나를 랜덤으로 선택.
        EnemyBaseDataSO enemyData = enemyBaseDatas[Random.Range(0, enemyBaseDatas.Length)];

        availableEnemies = new List<CardContent>();

        foreach(CardDataSO enemyCard in enemyData.enemyDeck)
        {
            availableEnemies.Add(enemyCard.card);
        }

        stackedWordCardEffect = new List<WordBase>();

        playerBase.GetComponent<DamageableObject>().Init(true, new UnitStats(300,0,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemyBase.GetComponent<DamageableObject>().Init(false, new UnitStats(enemyData.startingHP,0,0,0,0,0));
        enemyBase.GetComponent<SpriteRenderer>().sprite = enemyData.baseSprite;
        enemyBase.gameObject.SetActive(true);

        if(enemySpawnCoroutine == null) enemySpawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }

    public void UseCard(CardContent card)
    {
        switch (card.cardType)
        {
            case CardType.Unit:
                StartCoroutine(SpawnPlayerUnit(card));
                break;
            case CardType.Spell:
                StartCoroutine(CastPlayerSpell(card));
                break;
            case CardType.Word:
                AddWordCard(card);
                break;
        }
    }

    IEnumerator SpawnPlayerUnit(CardContent card)
    {
        FilterWordCard(WordCardType.Unit);

        //유닛 생성.
        GameObject newUnit = Instantiate(card.unit, playerBase.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
        newUnit.transform.SetParent(transform);
        newUnit.GetComponent<Units>().Init(true, card.stats);

        //유닛 생성 후 버프 적용
        foreach(WordBase wordCard in stackedWordCardEffect)
        {
            wordCard.ApplyBuff(newUnit.GetComponent<BuffController>());
        }

        //복제 카드 사용 시 버프까지 복사해서 생성.
        if(isCloneCardUsed)
        {
            yield return new WaitForSeconds(0.15f);

            isCloneCardUsed = false;
            StartCoroutine(SpawnPlayerUnit(card));
        }

        //단어카드 리스트 초기화.
        stackedWordCardEffect = new List<WordBase>();
    }

    IEnumerator CastPlayerSpell(CardContent card)
    {
        FilterWordCard(WordCardType.Spell);

        GameObject newSpell = Instantiate(card.unit);
        float targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        newSpell.GetComponent<SpellBase>().CastSpell(card.stats.baseATK, card.stats.baseRange, targetPos);

        //복제 카드 사용 시 두번 시전.
        if(isCloneCardUsed)
        {
            yield return new WaitForSeconds(0.3f);

            GameObject clonedSpell = Instantiate(card.unit);
            clonedSpell.GetComponent<SpellBase>().CastSpell(card.stats.baseATK, card.stats.baseRange, targetPos);
            isCloneCardUsed = false;
        }
    }

    void FilterWordCard(WordCardType wordCardType)
    {
        List<WordBase> toRemove = new List<WordBase>();

        foreach(WordBase wordcard in stackedWordCardEffect)
        {
            if(!wordcard.wordCardType.HasFlag(wordCardType))
            {
                toRemove.Add(wordcard);
            }
        }

        foreach(WordBase wordcard in toRemove)
        {
            stackedWordCardEffect.Remove(wordcard);
        }
    }
    
    public void AddWordCard(CardContent card)
    {
        WordBase targetWordCard = card.unit.GetComponent<WordBase>();

        //복제카드의 경우 별도로 처리하고 리턴.
        if(targetWordCard.cardName == "Clone")
        {
            isCloneCardUsed = true;
            return;
        }

        //쌓인 카드 효과중 중복이 있으면 아무효과 X.
        foreach(WordBase type in stackedWordCardEffect)
        {
            if(type.cardName == targetWordCard.cardName) return;
        }
        stackedWordCardEffect.Add(targetWordCard); //중복이 아니면 효과 스택.
    }

    public void StopSpawnEnemyCoroutine()
    {
        StopCoroutine(enemySpawnCoroutine);
        enemySpawnCoroutine = null;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, availableEnemies.Count);
            CardContent enemyUnit = availableEnemies[randomIndex];

            GameObject newUnit = Instantiate(enemyUnit.unit, enemyBase.transform.position + new Vector3(-1, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
            newUnit.transform.SetParent(transform);
            newUnit.GetComponent<Units>().Init(false, enemyUnit.stats);

            yield return new WaitForSeconds(GameRule.ENEMY_SPAWN_SECONDS);
        }
    }
}