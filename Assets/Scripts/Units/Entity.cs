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