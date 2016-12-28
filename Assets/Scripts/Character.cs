using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour
{
    Animator animation;
    public Transform sword;
    public Transform hand;
    CharacterController cc;
    public float pace;
    public AudioSource playerAudio;
    private float startingHealth = 100;
    private float currentHealth;
    public Slider healthSlider;
    public Slider energySlider;
    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    private float distance = 5;
    private float gravity = 0.75f;
    private int divisor;
    bool damaged=false;
    float flashSpeed = 5f;
    float range = 4f;
    float attackInterval  = 0.7f;
    float meleeDamage = 30;
    private float energy = 1000;




    void Start()
    {
        animation = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        currentHealth = startingHealth;
        playerAudio = GetComponent<AudioSource>();
        healthSlider.value = currentHealth;
    }

    private void FixedUpdate()
    {
        Vector3 jump = Vector3.zero;
        if (cc.isGrounded && Input.GetButton("Jump"))
        {
            jump.y = 150;
        }
        jump.y -= gravity * Time.fixedDeltaTime;
        cc.Move(jump * Time.fixedDeltaTime);
    }

    void Update()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
        Vector3 moveLeft = transform.TransformDirection(Vector3.right);
        if (Input.GetMouseButtonDown(1))
        {
            meleeAttack();
        }
        if (moveForward == 0)
        {
            animation.ResetTrigger("Walk");
            animation.ResetTrigger("Run");
            animation.SetTrigger("Idle");
            if(energy<1000)
                energy += 1;
        }
        else if (moveForward != 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            animation.SetTrigger("Walk");
            divisor = 25;
            if(energy<1000)
                energy += 1;
        }
        else if (moveForward != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (energy > 0)
            {
                energy -= 2;
                animation.SetTrigger("Run");
                divisor = 15;
            }else
            {
                animation.SetTrigger("Walk");
                divisor = 25;
            } 
        }
        energySlider.value = energy;
        cc.Move(moveDirection * (moveForward / divisor) );
        cc.Move(moveLeft * moveHorizontal / 20);
        Debug.Log("test");
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("enemies"))
        {
            damaged = true;
        }
        if (damaged)
        {
            TakeDamage(10);
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("Game OVER!");
        }
    }

   public void meleeAttack()
    {
        if (energy>400)
        { // only repeat attack after attackInterval
            animation.SetTrigger("RoundKick");
            energy -= 400;
            energySlider.value = energy;
            // get all colliders whose bounds touch the sphere
            Collider[] colls = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hit in colls)
            {
                if (hit.tag == "enemies")
                {
                    float dist = Vector3.Distance(hit.transform.position, transform.position);
                    if (dist <= range)
                    { 
                        Destroy(hit.gameObject);
                    }
                }
            }
        }
    }
}
