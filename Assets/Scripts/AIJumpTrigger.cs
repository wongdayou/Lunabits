using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpTrigger : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right
    }

    public Direction jumpDir;
    public float jumpDistrance = 5f;
    public float jumpHeight = 10f;
    public bool ignoreLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        col.transform.GetComponent<AI>().Jump(jumpDir == Direction.Left ? true : false, jumpHeight, jumpDistrance, ignoreLevel);
    }
}
