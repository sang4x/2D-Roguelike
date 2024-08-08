using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public AudioClip moveSound1;                
    public AudioClip moveSound2;                
    public AudioClip eatSound1;                 
    public AudioClip eatSound2;                
    public AudioClip drinkSound1;               
    public AudioClip drinkSound2;              
    public AudioClip gameOverSound;
    public int wallDamage = 1;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    private Animator animator;
    private int food;
  #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private Vector2 touchOrigin = -Vector2.one;
  #endif
    protected override void Start()
    {
        foodText.text = "Food: " + food;
        base.Start();
        animator = GetComponent<Animator>();
        food = GameManager.instance.FoodPoint;
    }
    private void OnDisable()
    {
        GameManager.instance.FoodPoint = food;
    }
    private void Checkifgameover()
    {
        if (food <= 0)
        {
            SoundManager.intance.PlayeSingle(gameOverSound);
            SoundManager.intance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
    protected override void AttemptMove<T>(int xdir, int ydir)
    {
        food--;
        foodText.text = "Food: " + food;
        RaycastHit2D hit;
        base.AttemptMove<T>(xdir, ydir);
        if (move(xdir,ydir,out hit))
        {
            SoundManager.intance.RamdomizeSfx(moveSound1, moveSound2);
        }
        Checkifgameover();
        GameManager.instance.playerturn = false;
    }
    private void Update()
    {
        if (!GameManager.instance.playerturn) return;
        
        int horizontal = 0;
        int vertical = 0;
#if UNITY_STANDALONE || UNITY_WEBPLAYER


        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0) 
        { 
            vertical = 0; 
        }
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (Input.touchCount > 0)
            {               
                Touch myTouch = Input.touches[0];              
                if (myTouch.phase == TouchPhase.Began)
                {                  
                    touchOrigin = myTouch.position;
                }           
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {                  
                    Vector2 touchEnd = myTouch.position;                  
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;                
                    touchOrigin.x = -1;                 
                    if (Mathf.Abs(x) > Mathf.Abs(y))       
                        horizontal = x > 0 ? 1 : -1;
                    else                        
                        vertical = y > 0 ? 1 : -1;
                }
            }           
#endif
        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }
    protected override void OnCantMove<T>(T component)
    {
      
        Wall hitwall = component as Wall;
        hitwall.DamgeWall(wallDamage);
        animator.SetTrigger("Playerchop");


    }

    
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void loseFood(int loss)
    {
        animator.SetTrigger("Playerhit");
        food -= loss;
        foodText.text = "-" + loss + "Food:" + food;
        Checkifgameover();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            foodText.text = "+" + pointPerFood + "Food: " + food;
            food += pointPerFood;
            SoundManager.intance.RamdomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            foodText.text = "+" + pointPerSoda + "Food: " + food;
            food += pointPerSoda;
            SoundManager.intance.RamdomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }
  
}