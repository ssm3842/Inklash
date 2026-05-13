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
        else
        {
            int currentFloor = RunManager.Inst.mapManager.floorClimbed + 1;
        
            // 현재 층 범위에 맞는 SO만 골라내기
            var candidates = new List<EnemyBaseDataSO>();
            foreach (var data in enemyBaseDatas)
            {
                if (currentFloor >= data.minFloor && currentFloor <= data.maxFloor)
                    candidates.Add(data);
            }
            
            // 안전장치: 후보 없으면 전체에서 랜덤 (인스펙터 설정 빈 경우 대비)
            if (candidates.Count == 0)
            {
                Debug.LogWarning($"Floor {currentFloor}에 맞는 EnemyBaseData 없음. 전체에서 랜덤 선택.");
                currentEnemyData = enemyBaseDatas[Random.Range(0, enemyBaseDatas.Length)];
            }
            else
            {
                currentEnemyData = candidates[Random.Range(0, candidates.Count)];
            }
        }

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
        // FilterWordCard();
        int castCount = 1;
        GameObject newSpell = Instantiate(card.unit);
        float targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        SealManager.ApplySeals(newSpell, FilterWordCard(card));
        SpellBase spell = newSpell.GetComponent<SpellBase>();

        if(spell.buffList.Exists(b => b.buffName.Equals("DoubleAttack"))) castCount *=2;
        if(spell.buffList.Exists(b => b.buffName.Equals("Split"))) castCount *=3;
        Destroy(newSpell);

        for (int i = 0; i < castCount; i++)
        {
            GameObject fireSpell = Instantiate(card.unit);
            SealManager.ApplySeals(fireSpell, FilterWordCard(card));
            
            SpellBase currentSpell = fireSpell.GetComponent<SpellBase>();
            
            currentSpell.ProcessSpell(card.stats.baseATK, card.stats.baseRange, targetPos);

            if (i < castCount - 1)
            {
                yield return new WaitForSeconds(0.75f); 
            }
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

            // 가드: 패턴 자체가 없으면 1초 쉬고 재시도
            if (currentPatternList == null || currentPatternList.Count == 0)
            {
                Debug.LogWarning($"{currentPhase} 페이즈에 패턴 없음. SO 확인 필요.");
                yield return new WaitForSeconds(1f);
                continue;
            }

            EnemyPatternSO selectedPattern = WeightedRandomPick(currentPatternList);
            if (selectedPattern == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            foreach (SpawnEntry entry in selectedPattern.entries)
            {
                if (currentPhase != patternPhase) break;

                for (int i = 0; i < entry.count; i++)
                {
                    CardContent enemyUnit = entry.enemyCard.card;
                    float xOffset = -1 + i * 0.3f;
                    GameObject newUnit = Instantiate(enemyUnit.unit,
                        enemyBase.transform.position + new Vector3(xOffset, Random.Range(-spawnHeight, spawnHeight) - 0.95f, 0),
                        Quaternion.identity);
                    newUnit.transform.SetParent(transform);
                    newUnit.GetComponent<Units>().Init(false, enemyUnit.stats);

                    StatController sc = newUnit.GetComponent<StatController>();
                    sc.ControlBaseStat(StatType.MAX_HP, currentEnemyData.hpMultiplier);
                    sc.ControlBaseStat(StatType.ATK,    currentEnemyData.atkMultiplier);
                    sc.InitMaxHP();

                    List<SealType> finalSeals = new List<SealType>(enemyUnit.seals);
                    if (entry.extraSeals != null) finalSeals.AddRange(entry.extraSeals);
                    SealManager.ApplySeals(newUnit, finalSeals);
                }

                // 항목 사이 대기
                float elapsed = 0f;
                while (elapsed < entry.delayAfter)
                {
                    elapsed += Time.deltaTime;
                    if (currentPhase != patternPhase) break;
                    yield return null;
                }
            }

            // 패턴 종료 후 대기
            yield return new WaitForSeconds(selectedPattern.interPatternDelay);

            // === 페이즈 전환 처리 (살린 블록들) ===
            if (currentPhase != patternPhase) continue;   // ① 도중에 페이즈 바뀌었으면 즉시 새 페이즈

            if (currentPhase == SpawnPhase.GameStart)     // ② Start 끝나면 Normal로
            {
                ChangePhase(SpawnPhase.Normal);
            }
            else if (currentPhase == SpawnPhase.Phase2)   // ③ Phase2 burst 끝나면 Normal로
            {
                if (bossPhaseThreshold != null) bossPhaseThreshold.SetActive(false);
                ChangePhase(SpawnPhase.Normal);
            }
        }
    }

    List<EnemyPatternSO> GetPatternsByPhase(SpawnPhase phase)
    {
        switch (phase)
        {
            case SpawnPhase.GameStart:
            // startPatterns 비어있으면 normalPatterns로 폴백
                if (currentEnemyData.startPatterns == null || currentEnemyData.startPatterns.Count == 0)
                    return currentEnemyData.normalPatterns;
                return currentEnemyData.startPatterns;
            case SpawnPhase.Normal:  return currentEnemyData.normalPatterns;
            case SpawnPhase.Phase2:  return currentEnemyData.phase2Patterns;
            default: return null;
        }
    }

    public void ChangePhase(SpawnPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        Debug.Log($" 적 페이즈 변경: {currentPhase}");
    }

    EnemyPatternSO WeightedRandomPick(List<EnemyPatternSO> list)
    {
        if (list == null || list.Count == 0) return null;

        float total = 0f;
        foreach (var p in list) total += Mathf.Max(0f, p.weight);
        if (total <= 0f) return list[Random.Range(0, list.Count)];  // fallback

        float roll = Random.Range(0f, total);
        float acc = 0f;
        foreach (var p in list)
        {
            acc += p.weight;
            if (roll <= acc) return p;
        }
        return list[list.Count - 1];
    }
}