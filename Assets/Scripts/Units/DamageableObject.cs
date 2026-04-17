using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    [SerializeField]protected StatController statController;
    [SerializeField]public BuffController buffController;

    [SerializeField]Slider healthBarSlider;
    [SerializeField]TextMeshProUGUI healthBarText;

    public bool isPlayers;

    private bool isPhase2Triggered = false;

    public List<string> onHitBuffTags = new List<string>();
    
    virtual public void Init(bool players, UnitStats stats)
    {        
        statController.InitStat(stats);
        buffController.ClearBuffs();
        if(healthBarSlider) 
        {
            healthBarSlider.value = statController.GetCurHp() / statController.GetStat(StatType.MAX_HP);
            healthBarText.text = statController.GetCurHp().ToString() + "/" + statController.GetStat(StatType.MAX_HP).ToString();
        }
        isPlayers = players;
    }

    //각 베이스의 경우만 이 함수 사용.  
    virtual public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        yield return new WaitForSeconds(delayTime);

        DamageTextCanvas.Inst.InstDamageText(amount, transform.position, isPlayers);

        if (statController.GetCurHp() <= amount)
        {
            if (!isPlayers)
            {   //TODO: 플레이어 승리 시 동작
                // if()
                RunManager.Inst.battleManager.OnBattleWin();
            }
            else
            {   //TODO: 플레이어 패배 시 동작
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

            if (currentHp <= maxHp * 0.5f)
            {
                isPhase2Triggered = true;
                RunManager.Inst.battleManager.cardUseManager.ChangePhase(CardUseManager.SpawnPhase.Phase2);
            }
        }
    }
}