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
        transform.rotation = Quaternion.identity; // 카메라 회전과 무관하게 항상 정방향 유지
    }
}
