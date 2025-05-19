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
        transform.rotation = Quaternion.identity; // ī�޶� ȸ���� �����ϰ� �׻� ������ ����
    }
}
