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

    /*
    public float currentHP;
    public float maxHP = 100f;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;
    */

    public void Init()
    {
        UpdateGoldUI();
        UpdateLifeUI();

        /*
        currentHP = maxHP;
        UpdateHpUI();
        */
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

    /*
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            UpdateHpUI();
            Debug.Log("Game Over");
            return;
        }
        UpdateHpUI();
    }

    public void RepairBase(float amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        UpdateHpUI();
    }

    private void UpdateHpUI()
    {
        if (hpText != null)
        {
            hpText.text = $"{currentHP:F0} / {maxHP}";
        }
        if (hpSlider != null)
        {
            hpSlider.value = currentHP / maxHP;
        }
    }
    */
}