using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardUseManager : MonoBehaviour
{

    public enum SpawnPhase { GameStart, Normal, Phase2 }
    public SpawnPhase currentPhase = SpawnPhase.GameStart; 
    private EnemyBaseDataSO currentEnemyData;
    public EnemyBaseDataSO CurrentEnemyData => currentEnemyData;

    float spawnHeight = 0.3f;
    [SerializeField] GameObject playerBase;
    [SerializeField] GameObject enemyBase;
    [SerializeField] GameObject bossPhaseThreshold;
    [SerializeField] EnemyBaseDataSO[] enemyBaseDatas;
    [SerializeField] EnemyBaseDataSO[] bossEnemyBaseDatas;

    List<SealType> stackedWordCardEffect;

    public Coroutine enemySpawnCoroutine = null;
    private bool hasPhase2Bursted = false;
    
    public void InitUnitManager(bool isBoss)
    {
        if (enemySpawnCoroutine != null) 
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }

        //적 데이터 중에서 하나를 랜덤으로 선택.
        if(isBoss)
        {
            currentEnemyData = bossEnemyBaseDatas[Random.Range(0, bossEnemyBaseDatas.Length)];
            bossPhaseThreshold.SetActive(true);
        }
        else currentEnemyData = enemyBaseDatas[Random.Range(0, enemyBaseDatas.Length)];

        stackedWordCardEffect = new List<SealType>();

        playerBase.GetComponent<DamageableObject>().Init(true, new UnitStats(300,0,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemyBase.GetComponent<DamageableObject>().Init(false, new UnitStats(currentEnemyData.maxHP,0,0,0,0,0),currentEnemyData);
        enemyBase.GetComponent<SpriteRenderer>().sprite = currentEnemyData.baseSprite;
        playerBase.gameObject.SetActive(true);
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
                SpawnPlayerUnit(card);
                break;
            case CardType.Spell:
                StartCoroutine(CastPlayerSpell(card));
                break;
            case CardType.Word:
                UseWordCard(card);
                break;
        }
    }

    void SpawnPlayerUnit(CardContent card)
    {
        //유닛 생성.
        GameObject newUnit = Instantiate(card.unit, playerBase.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight) - 0.95f, 0), Quaternion.identity);
        newUnit.transform.SetParent(transform);
        
        Units unitComponent = newUnit.GetComponent<Units>();
        unitComponent.Init(true, card.stats);

        SealManager.ApplySeals(newUnit, FilterWordCard(card));

        //단어카드 리스트 초기화.
        stackedWordCardEffect = new List<SealType>();
    }

    IEnumerator CastPlayerSpell(CardContent card)
    {
        // 1. 원본 리스트 (Copy 포함)
        List<SealType> originalSeals = FilterWordCard(card);
        
        // 2. 필터링된 리스트 (Copy, Purity 제외)
        List<SealType> filteredSeals = originalSeals
            .Where(s => s != SealType.Copy && s != SealType.Purity).ToList();

        int castCount = 1;

        // --- [첫 번째 ApplySeals: 판정용] ---
        // 여기서 딱 한 번 'Copy'가 포함된 리스트를 넣어 로직을 실행시킵니다.
        GameObject checkObj = Instantiate(card.unit);
        SealManager.ApplySeals(checkObj, originalSeals); // <--- 여기서만 Copy 작동
        
        SpellBase spell = checkObj.GetComponent<SpellBase>();

        // Copy 로직에 의해 spell.buffList에 DoubleAttack 등이 추가되었다면 여기서 count가 늘어납니다.
        if(spell.buffList.Exists(b => b.buffName.Equals("DoubleAttack"))) castCount *= 2;
        if(spell.buffList.Exists(b => b.buffName.Equals("Split"))) castCount *= 3;
        
        Destroy(checkObj); // 판정이 끝났으니 삭제

        // --- [두 번째 ApplySeals: 실제 발사용] ---
        for (int i = 0; i < castCount; i++)
        {
            GameObject fireSpell = Instantiate(card.unit);
            
            // 실제 발사체에는 'Copy'가 없는 리스트를 넣습니다.
            // 이렇게 하면 fireSpell 자체는 복제 기능을 수행하지 않습니다.
            SealManager.ApplySeals(fireSpell, filteredSeals); 
            
            float targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            SpellBase currentSpell = fireSpell.GetComponent<SpellBase>();
            currentSpell.ProcessSpell(card.stats.baseATK, card.stats.baseRange, targetPos);

            if (i < castCount - 1) yield return new WaitForSeconds(0.75f);
        }
        
        stackedWordCardEffect = new List<SealType>();
    }

    List<SealType> FilterWordCard(CardContent card)
    {
        List<SealType> sealList = new List<SealType>();

        foreach(SealType type in card.seals)
        {
            sealList.Add(type);
            if(sealList.Count >= 3) return sealList;
        }

        //유닛이 소지한 인장이 3개 미만일 경우 사용한 단어카드 리스트를 검사함.
        foreach(SealType type in stackedWordCardEffect)
        {
            //유닛이 가진 인장과 단어카드 인장이 중복이면 추가하지 않음.
            if(!sealList.Contains(type))
            {
                sealList.Add(type);
                if(sealList.Count >= 3) break;
            }
        }
        
        return sealList;
    }
    
    public void UseWordCard(CardContent card)
    {

        //중복이 있으면 단어카드 스택안함.
        foreach(SealType type in stackedWordCardEffect)
        {
            if(card.seals.Contains(type)) return;
        }
        stackedWordCardEffect.Add(card.seals[0]);
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
                    enemyBase.transform.position + new Vector3(-1, Random.Range(-spawnHeight, spawnHeight) - 0.95f, 0), 
                    Quaternion.identity);
                
                newUnit.transform.SetParent(transform);
                newUnit.GetComponent<Units>().Init(false, enemyUnit.stats);

                SealManager.ApplySeals(newUnit, FilterWordCard(enemyUnit));

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
                    bossPhaseThreshold.SetActive(false);
                    yield return new WaitForSeconds(GameRule.ENEMY_SPAWN_SECONDS);

                    if (!currentEnemyData.isBoss)
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