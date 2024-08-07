using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    private Animator animator;
    private int food;
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

        }
        Checkifgameover();
        GameManager.instance.playerturn = false;
    }
    private void Update()
    {
        if (!GameManager.instance.playerturn) return;
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0) { vertical = 0; }
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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            foodText.text = "+" + pointPerSoda + "Food: " + food;
            food += pointPerSoda;
            other.gameObject.SetActive(false);
        }
    }
  
}