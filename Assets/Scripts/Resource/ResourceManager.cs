using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public int currentGold = 100;
    [SerializeField] private TextMeshProUGUI goldText;


    int playerMaxHP;
    int playerCurrentHP;

    int maxLives = 2;
    int currentLives = 2;
    [SerializeField] private TextMeshProUGUI lifeText;

    public void Init()
    {
        UpdateGoldUI();
        UpdateLifeUI();

    }

    public void EarnGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    public bool CheckEnoughGold(int amount)
    {
        if(currentGold >= amount) return true;
        else return false; 
    }

    private void UpdateGoldUI()
    {
        if (goldText != null) goldText.text = $"{currentGold} G";
    }

    public void TakeDamage(int damage = 1)
    {
        playerCurrentHP -= damage;
        if (playerCurrentHP <= 0)
        {
            playerCurrentHP = 0;
            UpdateLifeUI();
            Debug.Log("Game Over");
            return;
        }
        UpdateLifeUI();
    }

    public void HealLife(int amount)
    {
        playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + amount);
        UpdateLifeUI();
    }

    public bool DecreaseLife(int amount)
    {
        currentLives -= amount;

        if (currentLives <= 0)
        {
            currentLives = 0;
            UpdateLifeUI();
            return false;
        }
        else
        {
            UpdateLifeUI();
            return true;
        }
    }

    private void UpdateLifeUI()
    {        
        if (lifeText != null)
        {
            string hearts = "";
            for (int i = 0; i < currentLives; i++) hearts += "<color=red>O</color> ";
            for (int i = 0; i < maxLives - currentLives; i++) hearts += "<color=#888888>O</color> ";

            lifeText.text = hearts;
        }
    }

    
}