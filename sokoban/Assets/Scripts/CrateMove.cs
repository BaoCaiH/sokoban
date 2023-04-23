using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private LayerMask obstacle;

    private bool isMoving = false;
    private Vector2 movePoint;

    // Start is called before the first frame update
    private void Start()
    {
        movePoint = transform.position;
        //movePoint = (Vector2)transform.position + Vector2.down;
        //Push(Vector2.down);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(transform.position, movePoint) > 0f)
        {
            MoveTowards();
        }
        else
        {
            isMoving = false;
        }
    }

    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            movePoint,
            speed * Time.deltaTime
        );
    }

    public void Push(Vector2 pushVector)
    {
        Vector2 checkPos = (Vector2)transform.position + pushVector / 2;
        //Debug.Log("Pushed: " + checkPos);
        Debug.Log("Collided with: " + Physics2D.OverlapCircleAll(checkPos, checkRadius, obstacle).Length);
        if (!isMoving && Physics2D.OverlapCircleAll(checkPos, checkRadius, obstacle).Length < 2)
        {
            movePoint = (Vector2)transform.position + pushVector;
            isMoving = true;
        }
        Debug.Log(movePoint);
    }
}
