using UnityEditor.Tilemaps;
using UnityEngine;

public class ResetAtk : StateMachineBehaviour
{


    [SerializeField] string triggerName = "Attack";
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
    }


}
