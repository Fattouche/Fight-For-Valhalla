using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private int speed = 1;
    private int runSpeed = 6;
    public Transform hero;
    public Animator anim;
    private float runDist = 10;
    private float attackDist = 1;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(hero.position);
        if (Vector3.Distance(transform.position, hero.position) >= attackDist && Vector3.Distance(transform.position, hero.position) < runDist)
        {
            speed = 2;
            transform.position += transform.forward * speed * Time.deltaTime;
            anim.SetTrigger("Charging");
        }
        if (Vector3.Distance(transform.position, hero.position) < attackDist)
        {
            anim.SetTrigger("Attacking");
        }else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            anim.SetTrigger("Walking");
        }
    }
}
