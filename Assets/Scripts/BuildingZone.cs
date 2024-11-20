using UnityEngine;

public class BuildingZone : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightMaterial;
    private Renderer _renderer;
    private bool _playerNearby = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = defaultMaterial;
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
