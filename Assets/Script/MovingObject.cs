using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockinglayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private float inverseMovetime;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        inverseMovetime = 1f / moveTime;
    }
    protected bool move(int xDir, int ydir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, ydir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockinglayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
          StartCoroutine(smoothmovement (end));
          return true;
        }
        return false;
    }
    protected virtual void AttemptMove<T>(int xdir, int ydir) where T : Component
    {
        RaycastHit2D hit;
        bool CanMove = move(xdir, ydir, out hit);
        if (hit.transform == null) 
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();
        if (!CanMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }
    protected IEnumerator smoothmovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, end, inverseMovetime * Time.deltaTime);
            rb.MovePosition (newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
    protected abstract void OnCantMove<T>(T component) 
        where T : Component;
}
