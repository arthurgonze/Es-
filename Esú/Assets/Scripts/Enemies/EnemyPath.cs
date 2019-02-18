using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed = 0.3f;

    private int cur = 0;

    void FixedUpdate()
    {
        Movement();
        MovementAnimation();
    }

    private void Movement()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }
    }

    private void MovementAnimation()
    {
        // Animation
        Vector2 dir = waypoints[cur].position - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
        GetComponent<FOV>().SetDirection(dir);
    }

    public void ResetCur()
    {
        cur = 0;
    }
}
