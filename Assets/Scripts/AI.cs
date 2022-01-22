using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    private Transform player;

    public float moveSpeed = 3f;

    IAstarAI ai;

    private Vector3 lastPos;
    private int lengthStayed;

    public List<Vector3> buffer;
    private bool stale;

    private Rigidbody2D r2d;
    public Collider2D col;

    public LayerMask ground;

    public enum Aggresive
    {
        Bad,
        Good
    }
    public Aggresive rabbitType;
    private Aggresive currentRabbitType;

    void Awake()
    {
        ai = GetComponent<IAstarAI>();
        player = FindObjectOfType<PlayerController>().transform;
        col = this.GetComponent<Collider2D>();
        r2d = this.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (rabbitType == Aggresive.Bad)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            currentRabbitType = Aggresive.Bad;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;
            currentRabbitType = Aggresive.Good;
        }

        
        if (rabbitType == Aggresive.Bad)
        {
            //this.transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed* Time.deltaTime);
            ai.destination = player.position;
            ai.GetRemainingPath(buffer, out stale);

            if (buffer.Count > 1 && isGrounded())
            {
                bool goingRight = buffer[2].x > this.transform.position.x;
                Vector3 dir = goingRight ? Vector2.right : Vector2.left;
                this.transform.Translate(dir * moveSpeed * Time.fixedDeltaTime);
            }

            if ((this.transform.position.x < lastPos.x + 0.1f && this.transform.position.x > lastPos.x - 0.1f) 
            && (this.transform.position.y < lastPos.y + 0.1f && this.transform.position.y > lastPos.y - 0.1f) )
            {
                lengthStayed++;
            }
            else
            {
                lengthStayed = 0;
                lastPos = this.transform.position;
            }

            if (lengthStayed > 50)
            {
                lengthStayed = 0;
                lastPos = this.transform.position;
                r2d.AddForce(new Vector2(Random.Range(0,2) == 1 ? -8 : 8, 5f), ForceMode2D.Impulse);
            }
        }
        else
        {
            lengthStayed = 0;
        }
        
        
    }

    public void Jump(bool jumpLeft, float jumpHeight, float jumpDistance, bool ignoreLevel = false)
    {
        if (ignoreLevel)
        {
            r2d.AddForce(new Vector2(jumpLeft ? -jumpDistance : jumpDistance, jumpHeight), ForceMode2D.Impulse);
            return;
        }
        if (!OnSameLevel()) 
        {
            r2d.AddForce(new Vector2(jumpLeft ? -jumpDistance : jumpDistance, jumpHeight), ForceMode2D.Impulse);
        }
    }

    private bool isGrounded()
    {
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= col.bounds.extents.x;
        topLeftPoint.y += col.bounds.extents.y;

        Vector2 bottomRight = transform.position;
        bottomRight.x += col.bounds.extents.x;
        bottomRight.y -= col.bounds.extents.y;

        return Physics2D.OverlapArea(topLeftPoint, bottomRight, ground);
    }

    private bool OnSameLevel()
    {
        if (this.transform.position.y > player.transform.position.y - 1f)
        {
            return true;
        }
        return false;
    }
}
