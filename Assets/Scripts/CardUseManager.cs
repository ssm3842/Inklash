using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    List<SealType> stackedWordCardEffect;

    public Coroutine enemySpawnCoroutine = null;
    private bool hasPhase2Bursted = false;

    public void InitUnitManager()
    {
        if (enemySpawnCoroutine != null) 
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }

        //적 데이터 중에서 하나를 랜덤으로 선택.
        currentEnemyData = enemyBaseDatas[Random.Range(0, enemyBaseDatas.Length)];

        stackedWordCardEffect = new List<SealType>();

        playerBase.GetComponent<DamageableObject>().Init(true, new UnitStats(300,0,0,0,0,0)); //TODO: 건물 체력 임시 생성.
        enemyBase.GetComponent<DamageableObject>().Init(false, new UnitStats(currentEnemyData.maxHP,0,0,0,0,0));
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
        GameObject newUnit = Instantiate(card.unit, playerBase.transform.position + new Vector3(0, Random.Range(-spawnHeight, spawnHeight) - 0.5f, 0), Quaternion.identity);
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

        if(spell.buffList.Exists(b => b.buffName.Equals("Split"))) castCount =3;
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
        
        // 비트 플래그 체크. 유닛 카드에 있는 인장을 가져옴.
        foreach (SealType type in System.Enum.GetValues(typeof(SealType)))
        {
            if (type != SealType.None && (card.seals & type) == type)
            {
                sealList.Add(type);
                //인장이 3개 이상일 경우 바로 리턴.
                if(sealList.Count >= 3) return sealList;
            }
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
            if(type == card.seals) return;
        }
        stackedWordCardEffect.Add(card.seals);
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