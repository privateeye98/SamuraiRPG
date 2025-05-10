using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator anim;
    int hashAttackCount = Animator.StringToHash("AtkCount");


    void Start()
    {
        TryGetComponent(out anim);
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AttackCount = (AttackCount + 1) % 3;
            anim.SetTrigger("Attack");

        }
    }

    // Update is called once per frame
    public int AttackCount
    {
        get => anim.GetInteger(hashAttackCount);
        set => anim.SetInteger(hashAttackCount, value);
    }
}
