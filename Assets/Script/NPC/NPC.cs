using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "장로";
    public string[] dialogueLines;

    [TextArea]
    bool isPlayerNearby = false;
    public Sprite portrait;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Z))
        {
            DialogueManager.instance.StartDialogue(
                npcName,
                dialogueLines,
                portrait,
                OpenShopAfterDialogue // 콜백 연결
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("플레이어가 NPC 근처에 접근함");
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }

    void OpenShopAfterDialogue()
    {
        ShopManager.instance.OpenShop();
    }

}
