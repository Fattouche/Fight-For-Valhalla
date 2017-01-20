using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Character car = animator.GetComponent<Character>();
        car.spinning = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Character car = animator.GetComponent<Character>();
        car.meleeAttack(true);
	}

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Character car = animator.GetComponent<Character>();
        car.spinning = false;
    }
}
