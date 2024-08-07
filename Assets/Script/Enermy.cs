using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MovingObject
{

    public int playerDamge;
    private Animator animator;
    private Transform target;
    private bool Skipmove;
    protected override void Start()
    {
        GameManager.instance.AddEnermyToList(this);
        base.Start();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
    protected override void AttemptMove<T>(int xdir, int ydir)
    {
        base.AttemptMove<T>(xdir, ydir);
        if (Skipmove)
        {
            Skipmove = false;
            return;
        }
        Skipmove = true;
    }
    public void moveenermy()
    {
        int xdir = 0;
        int ydir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            ydir = target.position.y > transform.position.y ? 1 : -1;
        else
            xdir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xdir, ydir);
    }
    protected override void OnCantMove<T>(T component)
    {
        animator.SetTrigger("Attack");
       
        Player hitPlayer = component as Player;
        hitPlayer.loseFood(playerDamge);
    }
}
