using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageableObject : MonoBehaviour
{
    [SerializeField]protected StatController statController;
    [SerializeField]public BuffController buffController;

    private EnemyBaseDataSO currentEnemyData;
    public EnemyBaseDataSO CurrentEnemyData => currentEnemyData;
    [SerializeField]Slider healthBarSlider;
    [SerializeField]TextMeshProUGUI healthBarText;

    [Header("FX")]
    [SerializeField] protected HitEffectSpawner hitEffectSpawner;
    [SerializeField] protected Vector2 hitEffectOffset = Vector2.zero;

    public bool isPlayers;

    private bool isPhase2Triggered = false;

    public List<string> onHitBuffTags = new List<string>();
    
    virtual public void Init(bool players, UnitStats stats, EnemyBaseDataSO data = null)
    {        
        statController.InitStat(stats);
        buffController.ClearBuffs();

        currentEnemyData = data;
        if(healthBarSlider) 
        {
            healthBarSlider.value = statController.GetCurHp() / statController.GetStat(StatType.MAX_HP);
            healthBarText.text = statController.GetCurHp().ToString() + "/" + statController.GetStat(StatType.MAX_HP).ToString();
        }
        isPhase2Triggered = false;
        isPlayers = players;
    }

    public Vector3 GetHitPosition()
    {
        float xSign = isPlayers ? -1f : 1f;
        Vector3 localOffset = new Vector3(hitEffectOffset.x * xSign, hitEffectOffset.y, 0f);
        return transform.TransformPoint(localOffset);
    }

    //각 베이스의 경우만 이 함수 사용.  
    virtual public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        yield return new WaitForSeconds(delayTime);

        DamageTextCanvas.Inst.InstDamageText(amount, transform.position, isPlayers);

        if (statController.GetCurHp() <= amount)
        {
            transform.DOShakePosition(1.5f, new Vector3(0.4f, 0, 0), 15, 0, false, false).SetUpdate(true);
            Time.timeScale = 0f;
            if (!isPlayers)
            {   //TODO: 플레이어 승리 시 동작
                // if()
                yield return new WaitForSecondsRealtime(2f);
                RunManager.Inst.battleManager.OnBattleWin();
            }
            else
            {   //TODO: 플레이어 패배 시 동작
                yield return new WaitForSecondsRealtime(2f);
                RunManager.Inst.battleManager.OnBattleLose();
            }

            //적 스폰 코루틴을 제거.
            RunManager.Inst.battleManager.cardUseManager.StopSpawnEnemyCoroutine();
            gameObject.SetActive(false);
        }
        else
        {
            statController.ChangeCurHp(amount);
            if (healthBarSlider)
            {
                healthBarSlider.value = statController.GetCurHp() / statController.GetStat(StatType.MAX_HP);
                healthBarText.text = statController.GetCurHp().ToString() + "/" + statController.GetStat(StatType.MAX_HP).ToString();
            }
        }

        if (!isPlayers && !isPhase2Triggered)
        {
            float currentHp = statController.GetCurHp();
            float maxHp = statController.GetStat(StatType.MAX_HP);

            if (currentHp <= maxHp * 0.5f && currentEnemyData.isBoss)
            {
                isPhase2Triggered = true;
                RunManager.Inst.battleManager.cardUseManager.ChangePhase(CardUseManager.SpawnPhase.Phase2);
            }
        }
    }
}