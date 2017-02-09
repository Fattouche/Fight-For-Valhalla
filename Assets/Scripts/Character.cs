using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour
{

    public Transform sword;
    public Transform hand;
    public Transform[] spawnPoints;

    public int score;
    public int kills;
    private int divisor;

    public Text scoreText;
    public Text waveText;

    public bool spinning = false;

    public float currentHealth;
    private float startingHealth;
    private float gravity;
    private float energy;
    float flashSpeed;
    float range;

    Animator animation;
    
    CharacterController cc;
    private AudioSource playerAudio;
    
    public Slider healthSlider;
    public Slider energySlider;

    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
   
    public GameObject coin;
    
    //initialize all the game variables
    void Start()
    {
        score = 0;
        kills = 0;
        divisor = 15;
        startingHealth = 100;
        gravity = 0.75f;
        energy = 1000;
        flashSpeed = 5f;
        range = 1f;
        animation = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        currentHealth = startingHealth;
        playerAudio = GetComponent<AudioSource>();
        healthSlider.value = currentHealth;
        //spawn all the coins
        for(int i=0;i<10;i++)
            spawnCoin();
    }

    //used for rigidbodies 
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

    //the main function of the game
    void Update()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
        Vector3 moveLeft = transform.TransformDirection(Vector3.right);
        //if right click set attacking animations
        if (Input.GetMouseButtonDown(1))
        {
            if (!spinning)
            {
                if (energy > 400)
                    energy -= 400;
                energySlider.value = energy;
                animation.SetTrigger("RoundKick");
            }
        }
        if (moveForward == 0) // if completely idle set all animations
        {
            animation.ResetTrigger("Walk");
            animation.ResetTrigger("Run");
            animation.SetTrigger("Idle");
            if (energy < 1000)
                energy += 2;
        }
        else if (moveForward != 0 && !Input.GetKey(KeyCode.LeftShift)) //if moving forward but not sprinting set animation
        {
            animation.SetTrigger("Walk");
            divisor = 15;
            if (energy < 1000)
                energy += 1;
        }
        else if (moveForward != 0 && Input.GetKey(KeyCode.LeftShift)) //if sprinting set animation
        {
            if (energy > 0)
            {
                energy -= 2;
                animation.SetTrigger("Run");
                divisor = 5;
            }
            else
            {
                animation.SetTrigger("Walk");
                divisor = 15;
            }
        }
        energySlider.value = energy;
        cc.Move(moveDirection * (moveForward / divisor));
        cc.Move(moveLeft * moveHorizontal / 20);
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        scoreText.text = "Score: " + score.ToString();
    }

    //take damage for character
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
    }

    //if the character grabs a coin
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            spawnCoin();
            score++;
            Destroy(other.gameObject);
        }
    }

    //attacking enemies
    public void meleeAttack(bool attacking)
    {
        if (attacking)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hit in colls)
            {
                if (hit.tag == "enemies")
                {
                    float dist = Vector3.Distance(hit.transform.position, transform.position);
                    if (dist <= range)
                    {
                        Destroy(hit.gameObject);
                        kills++;
                    }
                }
            }
        }
        else
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hit in colls)
            {
                if (hit.tag == "enemies")
                {
                    float dist = Vector3.Distance(hit.transform.position, transform.position);
                    if (dist <= range)
                    {
                        TakeDamage(10);
                        damageImage.color = flashColour;
                    }
                }
            }
        }
    }
    //spawn the coin
    public void spawnCoin()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(coin, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    //show the text for waves
    public IEnumerator showText(int waveCount)
    {
        waveText.gameObject.SetActive(true);
        waveText.text = "Wave " + waveCount;
        waveText.CrossFadeAlpha(0.0f, 5, false);
        yield return new WaitForSeconds(5);
        waveText.CrossFadeAlpha(1.0f, 1, false);
        waveText.gameObject.SetActive(false);
    }
}

