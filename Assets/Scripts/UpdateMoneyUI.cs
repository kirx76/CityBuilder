using TMPro;
using UnityEngine;

public class UpdateMoneyUI : MonoBehaviour
{
    public GameObject moneyText;

    void Update()
    {
        if (moneyText && PlayerEconomy.instance)
        {
            moneyText.GetComponent<TextMeshProUGUI>().text = $"Money: {PlayerEconomy.instance.money} $";
        }
        else
        {
            Debug.LogWarning("MoneyText or PlayerEconomy.Instance is not set!");
        }
    }

}
