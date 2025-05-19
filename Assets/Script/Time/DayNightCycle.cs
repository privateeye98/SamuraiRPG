using UnityEngine;

[System.Serializable]
public class LayerGroup
{
    public string name;
    public Transform parent;               // BACKGROUND or MIDDLEGROUND
    public Gradient colorOverTime;        // 시간에 따른 색상
    [HideInInspector] public SpriteRenderer[] renderers;
}

public class DayNightCycle : MonoBehaviour
{
    public float cycleDuration = 60f;
    public LayerGroup[] layers;

    float timer;

    void Start()
    {
        foreach (var layer in layers)
        {
            if (layer.parent != null)
            {
                layer.renderers = layer.parent.GetComponentsInChildren<SpriteRenderer>();
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = (timer % cycleDuration) / cycleDuration;

        foreach (var layer in layers)
        {
            Color c = layer.colorOverTime.Evaluate(t);
            foreach (var sr in layer.renderers)
            {
                if (sr != null)
                    sr.color = c;
            }
        }
    }
}
