using TMPro;
using UnityEngine;

public class NPCNameTag : MonoBehaviour
{
    [SerializeField] string npcName = "NPC";
    [SerializeField] TextMeshProUGUI nameText;

     void Start()
    {

        if (nameText != null)
            nameText.text = npcName;    
        
    }
    void LateUpdate()
    {
        if (nameText != null)
        {
            nameText.transform.rotation = Quaternion.identity;
            
        }
        transform.localPosition = new Vector3(0f, -1.2f, 0f);
    }
}
