using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5f;

    private Camera _playerCamera;
    
    private BuildingZone _currentBuildingZone; // Текущая зона постройки
    private Building _currentBuilding; // Текущая зона здания
    public BuildMenu buildMenu;      // Ссылка на меню строительства

    void Start()
    {
        _playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentBuildingZone != null && !buildMenu.createMenuPanel.activeSelf)
            {
                buildMenu.OpenCreateMenu(_currentBuildingZone);
            }

            if (_currentBuilding != null && !buildMenu.createMenuPanel.activeSelf)
            {
                buildMenu.OpenUpdateMenu(_currentBuilding);
            }
        }

        if (_currentBuildingZone == null)
        {
            buildMenu.ExitMenu();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BuildingZone>() != null)
        {
            Debug.Log($"Entered building zone: {other.name}");
            _currentBuildingZone = other.GetComponent<BuildingZone>();
        }

        if (other.GetComponent<Building>() != null)
        {
            Debug.Log($"Entered building: {other.name}");
            _currentBuilding = other.GetComponent<Building>();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BuildingZone>() != null)
        {
            Debug.Log($"Exited building zone: {other.name}");
            _currentBuildingZone = null;
        }
        
        if (other.GetComponent<Building>() != null)
        {
            Debug.Log($"Exited building: {other.name}");
            _currentBuilding = null;
        }
    }
}
