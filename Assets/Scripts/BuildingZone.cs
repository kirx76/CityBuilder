using UnityEngine;

public class BuildingZone : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightMaterial;
    Renderer _renderer;

    bool _isPlayerNearby = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = defaultMaterial;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (BuildMenu.Instance.buildMenuPanel.activeSelf)
            return;

        if (!_isPlayerNearby)
            return;

        BuildMenu.Instance.SetBuildingZone(this);

        BuildMenu.Instance.ShowMenu(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _renderer.material = highlightMaterial;
        _isPlayerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _renderer.material = defaultMaterial;
        _isPlayerNearby = false;
    }

    public void Build(GameObject building)
    {
        Debug.Log($"Building {building.name} in zone {name}");

        var buildingSize = building.GetComponent<Renderer>().bounds.size;
        var zoneSize = GetComponent<Renderer>().bounds.size;

        if (buildingSize.x <= zoneSize.x && buildingSize.z <= zoneSize.z)
        {
            var spawnPosition = transform.position + Vector3.up * (buildingSize.y / 2);
            Instantiate(building, spawnPosition, Quaternion.identity);

            Destroy(gameObject);

            Debug.Log($"Building {building.name} placed at {spawnPosition}!");

            BuildMenu.Instance.ShowMenu(false);
        }

        Debug.LogWarning($"Building {building.name} can't be placed in zone {name}!");
    }
}
