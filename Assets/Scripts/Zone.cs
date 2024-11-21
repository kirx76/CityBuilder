using UnityEngine;

public class Zone : MonoBehaviour
{
    bool _isPurchased = false;
    int _cost;
    public int rawCost;

    void Start()
    {
        SetCost(rawCost * ZoneGenerator.Instance.zoneLevel);
    }

    public void SetPurchased(bool isPurchased)
    {
        _isPurchased = isPurchased;
        UpdateZoneAppearance();
    }

    void SetCost(int zoneCost)
    {
        _cost = zoneCost;
        UpdateZoneAppearance();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isPurchased)
        {
            ShowPurchasePrompt();
        }
    }

    void ShowPurchasePrompt()
    {
        Debug.Log($"Zone available for purchase: {_cost}");

    }

    public void PurchaseZone()
    {
        if (!PlayerEconomy.instance.SpendMoney(_cost))
            return;

        _isPurchased = true;
        UpdateZoneAppearance();

        ZoneGenerator.Instance.GeneratePlatformsAroundPlatform(gameObject);
    }

    void UpdateZoneAppearance()
    {
        if (!_isPurchased)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
