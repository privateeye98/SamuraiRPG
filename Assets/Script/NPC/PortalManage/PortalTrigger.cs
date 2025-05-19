using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public string portalID;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PortalManager.Instance.OpenPortal(portalID);
    }
}
