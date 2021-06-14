 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playercontroller : MonoBehaviour
{
    //Start()
    private Rigidbody2D key;
    private Animator anim;
    private Collider2D collider;

    //States
    private State state = State.idle;
    private enum State {idle,run,jump,falling,hurt};

   //Variable to pass final score
    private LevelWin levelWin;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed= 5f;
    [SerializeField] private float jumpforce= 10f;
    [SerializeField] private int coins = 0;
    [SerializeField] private Text CoinText;
    [SerializeField] private float hurtforce = 10f;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private int lives;
    [SerializeField] private Text HealthAmount;
 
    // Start is called before the first frame update
    void Start()
    {
        key = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        HealthAmount.text = lives.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if(state != State.hurt)
        {
        Movements();
        }
        StateSwitch();
        anim.SetInteger("state",(int)state);
       
    }

void Awake()
    {
    levelWin =  GameObject.FindObjectOfType<LevelWin>();
    }

//coin collection
private void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.tag == "Coins")
    {
        coin.Play();
        Destroy(collision.gameObject);
        coins += 1;
        CoinText.text= coins.ToString();
        levelWin.updateScore(coins);
    }
}

//Hurt
private void OnCollisionEnter2D(Collision2D enemy)
{
    if(enemy.gameObject.tag == "Enemy1")
    {
        Enemy1 enemy1 = enemy.gameObject.GetComponent<Enemy1>();
        
        if(state==State.falling)
        {
        enemy1.EnemyDeath();
        jump();
        }
        else
        {
            state = State.hurt;
            hurt.Play();
            HandleHealth();

            if(enemy.gameObject.transform.position.x > transform.position.x)
            {
                key.velocity = new Vector2(-hurtforce , key.velocity.y);
            }
            else
            {
                key.velocity = new Vector2(hurtforce , key.velocity.y);
            }
        }

    }
}

//Lives 
private void HandleHealth()
{
    lives -= 1;
    HealthAmount.text = lives.ToString();
    if(lives <= 0)
    {
        SceneManager.LoadScene("GameOver");
    }
}

//Movements
private void Movements()
{
        float horizontaldirection =  Input.GetAxis("Horizontal");

        //player moving right
        if(horizontaldirection < 0)

         {
             key.velocity = new Vector2(-speed,key.velocity.y);
             transform.localScale = new Vector2(-1,1);
             
         }

        //player moving left
          else if(horizontaldirection > 0)
         {
             key.velocity = new Vector2(speed,key.velocity.y);
             transform.localScale = new Vector2(1,1);
             
         }

        //jump
         if(Input.GetButtonDown("Jump") && collider.IsTouchingLayers(ground))
         {
             jump();
         }
}

//Jump
private void jump()
    {
        key.velocity = new Vector2(key.velocity.x,jumpforce);
        state = State.jump;
    } 

//Animationstates
private void StateSwitch()
{
    if(state == State.jump)
    {
        if(key.velocity.y < 0.1f)
        {
            state = State.falling;
        }
    }

    else if(state == State.falling)
    {
        if(collider.IsTouchingLayers(ground))
        {
            state = State.idle;
        }
    }

    else if(state == State.hurt)
    {
        if(Mathf.Abs(key.velocity.x) < .1f)
        {
            state = State.idle;
            hurt.Play();
        }
    }
    else if(Mathf.Abs(key.velocity.x) > 2f)
    {
        //moving 
        state = State.run;
    }

    else
    {
      //idle
      state = State.idle;  
    }
}

//footstep sound
private void Footstep()
    {
        footstep.Play();
    }
}