using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedRendererSorting : MonoBehaviour
{
    public string sortingLayer = "Enemy";
    public int orderInLayer = 10;

    void Awake()
    {
        var smr = GetComponent<SkinnedMeshRenderer>();
        smr.sortingLayerName = sortingLayer;
        smr.sortingOrder = orderInLayer;
    }
}
