using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject buildingZonePrefab; // Префаб зоны строительства
    
    public Material defaultMaterial;
    public Material highlightMaterial;
    
    private Renderer _renderer;
    private bool _playerNearby = false;
    
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = defaultMaterial;
    }

    public void RemoveBuilding()
    {
        var spawnPosition = new Vector3(transform.position.x, (float)0.0002, transform.position.z);
        // Создаём зону строительства на месте текущего здания
        Instantiate(buildingZonePrefab, spawnPosition, Quaternion.identity);

        // Удаляем здание
        Destroy(gameObject);

        Debug.Log("Building removed and zone restored!");
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = true;
            _renderer.material = highlightMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;
            _renderer.material = defaultMaterial;
        }
    }
    
    public bool IsPlayerNearby()
    {
        return _playerNearby;
    }
}
