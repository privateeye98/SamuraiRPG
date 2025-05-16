using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "���";
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
                OpenShopAfterDialogue // �ݹ� ����
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("�÷��̾ NPC ��ó�� ������");
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
