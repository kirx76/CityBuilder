using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BuildMenu : MonoBehaviour
{
    public GameObject buildMenuPanelPrefab;
    public GameObject buttonPrefab;

    public List<BuildingData> buildingList; // Список зданий

    public GameObject buildMenuPanel;

    readonly List<GameObject> _createdButtons = new List<GameObject>();

    BuildingZone _currentBuildingZone;

    public static BuildMenu Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (buildMenuPanel != null && buildMenuPanel != buildMenuPanelPrefab)
        {
            Destroy(buildMenuPanel);
            return;
        }

        buildMenuPanel = Instantiate(buildMenuPanelPrefab, transform);

        buildMenuPanel.SetActive(false);
    }

    static void ToggleMenu(bool show)
    {
        if (show)
        {
            // Показать курсор и остановить игру
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Остановка времени
        }
        else
        {
            // Скрыть курсор и продолжить игру
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Возобновление времени
        }
    }

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        Debug.Log(buildMenuPanel);
        Debug.Log(buildMenuPanel.transform);

        var exitButton = buildMenuPanel.transform.Find("ExitButton").GetComponent<Button>();

        if (exitButton == null)
        {
            Debug.LogError("Build menu panel is missing the exit button.");
            return;
        }

        exitButton.onClick.AddListener(ExitMenu);

        foreach (var button in _createdButtons.Where(button => button != null))
        {
            Destroy(button);
        }

        _createdButtons.Clear();

        if (buildingList == null || buildingList.Count == 0)
        {
            Debug.LogWarning("Building list is empty or null.");
            return;
        }

        // Получение ссылки на объект Content
        var buildings = buildMenuPanel.transform.Find("Buildings");
        if (buildings == null)
        {
            Debug.LogError("Buildings object not found.");
            return;
        }

        var viewport = buildings.Find("Viewport");
        if (viewport == null)
        {
            Debug.LogError("Viewport object not found.");
            return;
        }

        var content = viewport.Find("Content");
        if (content == null)
        {
            Debug.LogError("Content object not found.");
            return;
        }

        foreach (var buildingData in buildingList)
        {
            if (buttonPrefab == null)
            {
                Debug.LogWarning("Button prefab is null.");
                return;
            }

            var button = Instantiate(buttonPrefab, content);
            if (button == null)
            {
                Debug.LogError("Failed to instantiate button prefab.");
                continue;
            }

            _createdButtons.Add(button);

            var btnComponent = button.GetComponent<Button>();
            if (btnComponent == null)
            {
                Debug.LogError("Button prefab is missing the Button component.");
                continue;
            }
            btnComponent.onClick.AddListener(() => OnBuildingSelected(buildingData));

            var cost = buildingData.prefab.GetComponent<Building>().cost;
            var buttonText = button.transform.Find("Text (TMP)")?.GetComponent<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = $"{buildingData.name}: {cost} $";
            }
            else
            {
                Debug.LogWarning("Button prefab is missing a TMP_Text component.");
            }

            var icon = button.transform.Find("Icon").GetComponent<Image>();
            if (icon != null && buildingData.icon != null)
            {
                icon.sprite = buildingData.icon;
            }

            // Проверка стоимости
            if (PlayerEconomy.instance.money < cost)
            {
                btnComponent.interactable = false;
            }
        }
    }

    void OnBuildingSelected(BuildingData buildingData)
    {
        var cost = buildingData.prefab.GetComponent<Building>().cost;

        if (!PlayerEconomy.instance.SpendMoney(cost))
            return;

        Debug.Log($"Building selected: {buildingData.name}");

        _currentBuildingZone.Build(buildingData.prefab);
    }

    public void RefreshButtons()
    {
        foreach (var button in _createdButtons)
        {
            var btnComponent = button.GetComponent<Button>();
            if (btnComponent == null)
                continue;

            var buildingData = buildingList[_createdButtons.IndexOf(button)];
            var cost = buildingData.prefab.GetComponent<Building>().cost;

            btnComponent.interactable = PlayerEconomy.instance.money >= cost;
        }
    }

    public void ShowMenu(bool show)
    {
        buildMenuPanel.SetActive(show);

        ToggleMenu(show);
    }

    public void SetBuildingZone(BuildingZone zone)
    {
        _currentBuildingZone = zone;
    }

    public void ExitMenu()
    {
        buildMenuPanel.SetActive(false);

        ToggleMenu(false);
    }
}
