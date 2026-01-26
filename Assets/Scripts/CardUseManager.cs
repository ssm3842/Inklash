using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUseManager : MonoBehaviour
{

    public enum SpawnPhase { GameStart, Normal, Phase2 }
    public SpawnPhase currentPhase = SpawnPhase.GameStart; 
    private EnemyBaseDataSO currentEnemyData;
    public EnemyBaseDataSO CurrentEnemyData => currentEnemyData;

    float spawnHeight = 0.3f;
    [SerializeField] GameObject playerBase;
    [SerializeField] GameObject enemyBase;
    [SerializeField] EnemyBaseDataSO[] enemyBaseDatas;

    List<CardContent> availableEnemies;

    List<WordBase> stackedWordCardEffect;

    public Coroutine enemySpawnCoroutine = null;
    private bool hasPhase2Bursted = false;

    bool isCloneCardUsed = false;
    

    public void InitUnitManager()
    {
        if (enemySpawnCoroutine != null) 
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }

        //적 데이터 중에서 하나를 랜덤으로 선택.
        currentEnemyData = enemyBaseDatas[Random.Range(0, enemyBaseDatas.Length)];

        stackedWordCardEffect = new List<WordBase>();

        playerBase.GetComponent<DamageableObject>().Init(true, new UnitStats(300,0,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemyBase.GetComponent<DamageableObject>().Init(false, new UnitStats(currentEnemyData.maxHP,0,0,0,0,0));
        enemyBase.GetComponent<SpriteRenderer>().sprite = currentEnemyData.baseSprite;
        enemyBase.gameObject.SetActive(true);

        currentPhase = SpawnPhase.GameStart;
        hasPhase2Bursted = false;

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
            SpawnPhase patternPhase = currentPhase;
            List<EnemyPatternSO> currentPatternList = GetPatternsByPhase(currentPhase);

            int randomIndex = Random.Range(0, currentPatternList.Count);
            EnemyPatternSO selectedPattern = currentPatternList[randomIndex];

            foreach (CardDataSO cardData in selectedPattern.enemyDeck)
            {
                if (currentPhase != patternPhase) break;

                CardContent enemyUnit = cardData.card; 

                GameObject newUnit = Instantiate(enemyUnit.unit, 
                    enemyBase.transform.position + new Vector3(-1, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), 
                    Quaternion.identity);
                
                newUnit.transform.SetParent(transform);
                newUnit.GetComponent<Units>().Init(false, enemyUnit.stats);

                float waitTime = GameRule.ENEMY_SPAWN_SECONDS;

                if (currentPhase == SpawnPhase.Phase2 && !hasPhase2Bursted)
                {
                    waitTime = 0.01f;
                }

                float elapsed = 0f;
                while(elapsed < waitTime)
                {
                    elapsed += Time.deltaTime;
                    
                    if (currentPhase != patternPhase) 
                    {
                        break; 
                    }
                    
                    yield return null;
                }
            }

            if (currentPhase != patternPhase)    continue; 

            if (currentPhase == SpawnPhase.GameStart)
            {
                ChangePhase(SpawnPhase.Normal);
            }
            else if (currentPhase == SpawnPhase.Phase2)
            {
                if (!hasPhase2Bursted)
                {
                    hasPhase2Bursted = true; 
                    yield return new WaitForSeconds(GameRule.ENEMY_SPAWN_SECONDS);

                    if (!currentEnemyData.isElite)
                    {
                        ChangePhase(SpawnPhase.Normal);
                    }
                }
            }
        }
    }

    List<EnemyPatternSO> GetPatternsByPhase(SpawnPhase phase)
    {
        switch (phase)
        {
            case SpawnPhase.GameStart: return currentEnemyData.startPatterns;
            case SpawnPhase.Normal:    return currentEnemyData.normalPatterns;
            case SpawnPhase.Phase2:    return currentEnemyData.phase2Patterns;
            default: return null;
        }
    }

    public void ChangePhase(SpawnPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        Debug.Log($" 적 페이즈 변경: {currentPhase}");
    }
}