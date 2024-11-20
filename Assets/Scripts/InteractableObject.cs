using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Material defaultMaterial;
    public Material highlightMaterial;

    void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void Highlight()
    {
        GetComponent<Renderer>().material = highlightMaterial;
    }

    public void ResetHighlight()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
