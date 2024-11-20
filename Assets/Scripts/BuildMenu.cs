using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    
    public GameObject createMenuPanel;
    public GameObject updateMenuPanel;
    
    private BuildingZone _currentBuildingZone;
    private Building _currentBuilding;
    
    public void OpenCreateMenu(BuildingZone zone)
    {
        _currentBuildingZone = zone;

        ShowCreateMenu(true);
    }

    public void OpenUpdateMenu(Building building)
    {
        _currentBuilding = building;

        ShowUpdateMenu(true);
    }

    private void ShowCreateMenu(bool show)
    {
        createMenuPanel.SetActive(show);
        
        ToggleMenu(show);
    }
    
    private void ShowUpdateMenu(bool show)
    {
        updateMenuPanel.SetActive(show);
        
        ToggleMenu(show);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentBuildingZone != null && _currentBuildingZone.IsPlayerNearby())
            {
                ShowCreateMenu(true);
            }
            
            if (_currentBuilding != null && _currentBuilding.IsPlayerNearby())
            {
                ShowUpdateMenu(true);
            }
        }
    }

    public void ExitMenu()
    {
        ToggleMenu(false);
    }

    public void Build(int buildingIndex)
    {
        if (_currentBuildingZone != null)
        {
            Debug.Log($"Building {buildingIndex} in zone {_currentBuildingZone.name}");
            
            // Проверяем размер зоны и здания
            GameObject selectedBuilding = buildingPrefabs[buildingIndex];
            Vector3 buildingSize = selectedBuilding.GetComponent<Renderer>().bounds.size;
            Vector3 zoneSize = _currentBuildingZone.GetComponent<Renderer>().bounds.size;

            if (buildingSize.x <= zoneSize.x && buildingSize.z <= zoneSize.z)
            {
                Vector3 spawnPosition = _currentBuildingZone.transform.position + Vector3.up * (buildingSize.y / 2);
                Instantiate(selectedBuilding, spawnPosition, Quaternion.identity);
                
                Destroy(_currentBuildingZone.gameObject);
                _currentBuildingZone = null;
                
                Debug.Log($"Building {selectedBuilding.name} placed at {spawnPosition}");
                ShowCreateMenu(false);
            }
            else
            {
                Debug.LogWarning("Building is too large for this zone!");
            }
        }
        else
        {
            Debug.LogWarning("No building zone available!");
        }
    }

    private static void ToggleMenu(bool show)
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

    public void RemoveBuilding(Building building)
    {
        building.RemoveBuilding();
        ToggleMenu(false);
    }
}
