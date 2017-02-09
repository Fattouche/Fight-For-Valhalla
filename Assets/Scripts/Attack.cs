using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour {

    //When the character starts swinging
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Character character = animator.GetComponent<Character>();
        character.spinning = true;
    }

    //during the characters swing animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Character character = animator.GetComponent<Character>();
        character.meleeAttack(true);
	}

    //after the characters swing animation
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Character character = animator.GetComponent<Character>();
        character.spinning = false;
    }
}
