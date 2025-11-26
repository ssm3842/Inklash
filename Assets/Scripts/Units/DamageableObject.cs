using System.Collections;
using TMPro;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField]protected StatController statController;
    [SerializeField]public BuffController buffController;

    [SerializeField]TextMeshPro healthBar;

    public bool isPlayers;
    
    virtual public void Init(bool players, UnitStats stats)
    {
        statController.InitStat(stats);
        if(healthBar) healthBar.text = statController.GetCurHp().ToString() + " / " + statController.GetStat(StatType.MAX_HP).ToString();

        isPlayers = players;
    }

    //각 베이스의 경우만 이 함수 사용.  
    virtual public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        yield return new WaitForSeconds(delayTime);
        if (statController.GetCurHp() <= amount)
        {
            if (!isPlayers)
            {   //TODO: 플레이어 승리 시 동작
                RunManager.Inst.battleManager.OnBattleWin();
                Debug.Log("Player win");
            }
            else
            {   //TODO: 플레이어 패배 시 동작
                Debug.Log("Player Lose");
            }

            Destroy(this.gameObject);
        }
        else
        {
            statController.ChangeCurHp(amount);
            if (healthBar) healthBar.text = statController.GetCurHp().ToString() + " / " + statController.GetStat(StatType.MAX_HP).ToString();
        }
    }
}