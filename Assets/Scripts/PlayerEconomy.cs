using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public static PlayerEconomy instance; // Синглтон для удобного доступа
    public int money = 10000; // Начальный баланс

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;

            BuildMenu.Instance.RefreshButtons();

            Debug.Log($"Money spent: {amount}. Remaining: {money}");
            return true;
        }

        Debug.LogWarning("Not enough money!");
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;

        BuildMenu.Instance.RefreshButtons();

        Debug.Log($"Money added: {amount}. Total: {money}");
    }
}
