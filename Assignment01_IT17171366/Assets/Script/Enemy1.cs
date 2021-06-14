using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    
    public bool MoveRight;
    protected Animator anim1;
    protected Rigidbody2D rigitbody;
    protected AudioSource deathsound;

    [SerializeField] float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
       anim1 = GetComponent<Animator>();
       rigitbody = GetComponent<Rigidbody2D>();
       deathsound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if(MoveRight)
       {
           transform.Translate(2 * Time.deltaTime * moveSpeed , 0,0);
           transform.localScale = new Vector2 (1,1);
       } 
       else
       {
           transform.Translate(-2 * Time.deltaTime * moveSpeed , 0,0);
           transform.localScale = new Vector2 (-1,1);
       }
    }

    //Enemy Turn
    void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.CompareTag("Turn"))
        {
            if(MoveRight)
            {
                MoveRight = false;
            }
            else
            {
                MoveRight = true;
            }
            
        }
    }
    
  //Enemy Death
  public void EnemyDeath()
  {
      anim1.SetTrigger("death");
      deathsound.Play();

  }

  private void death()
  {
      Destroy(this.gameObject);
  }
}
