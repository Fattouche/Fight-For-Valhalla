using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animation;
    CharacterController cc;
    public Camera camera;
    float distance = 5;
    public int pace;
    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveForward = Input.GetAxis("Vertical");
        float gravity = 20;
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
        Vector3 moveLeft = transform.TransformDirection(Vector3.right);
        Vector3 jump = Vector3.zero;
        if (cc.isGrounded && Input.GetButton("Jump"))
        {
            animation.Play("RoundKick");
            jump.y = 100;
        }
        jump.y -= gravity * Time.deltaTime;
        cc.Move(jump * Time.deltaTime);
        cc.Move(moveLeft * moveHorizontal / 20);
        if (moveForward == 0)
        {
            if (moveHorizontal == 1)
            {
                animation.Play("Walk");
            }
            else
            {
                animation.Play("Idle");
            }

        }
        else if (moveForward != 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            animation.Play("Walk");
            cc.Move(moveDirection * (moveForward / 25));
        }
        else if (moveForward != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            animation.Play("Run");
            cc.Move(moveDirection * (moveForward / 15));
        }
    }
}
