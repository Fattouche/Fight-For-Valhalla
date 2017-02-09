using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform hero;
    public Animator anim;
    private float runDist;
    private float attackDist;
    private float speed;

    //initialize the enemies variables
    private void Start()
    {
        speed = 4;
        attackDist = 1;
        runDist = 10;
    }

    // Update is called once per frame, changes how fast they will run depending on their distance. Also changes when they will attack based off their distance.
    //Enemies will always follow the character, even through buildings which is a slight bug at this point.
    void Update()
    {
        transform.LookAt(hero.position);
        if (Vector3.Distance(transform.position, hero.position) >= attackDist && Vector3.Distance(transform.position, hero.position) < runDist)
        {
            speed = 5;
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
