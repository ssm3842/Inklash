using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]protected float MaxHP = 10f;
    [SerializeField]protected float CurHP;

    [SerializeField] TextMeshPro healthBar;

    public bool isPlayers;
    virtual public void Init(bool players)
    {
        CurHP = MaxHP;
        if(healthBar) healthBar.text = MaxHP.ToString() + " / " + CurHP.ToString();

        isPlayers = players;
    }

    virtual public void TakeDamage(float amount)
    {
        if (CurHP < amount)
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
