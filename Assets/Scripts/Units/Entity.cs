using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]protected float MaxHP;
    [SerializeField]protected float CurHP;

    [SerializeField] TextMeshPro healthBar;

    public bool isPlayers;
    
    virtual public void Init(bool players)
{
    this.MaxHP = 10; 
    CurHP = MaxHP;
    if(healthBar) healthBar.text = CurHP.ToString() + " / " + MaxHP.ToString();

    isPlayers = players;
}
    virtual public void Init(bool players, CardContent card)
    {
        this.MaxHP = card.stats.hp;
        CurHP = MaxHP;
        if (healthBar) healthBar.text = MaxHP.ToString() + " / " + CurHP.ToString();

        isPlayers = players;
    }

    virtual public void TakeDamage(float amount)
    {
        if (CurHP <= amount)
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
            CurHP -= amount;
            if(healthBar) healthBar.text = CurHP.ToString() + " / " + MaxHP.ToString();
            Debug.Log(CurHP);
        }
    }
}