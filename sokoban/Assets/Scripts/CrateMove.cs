using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateMove : MonoBehaviour
{
    [SerializeField] private bool movable = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private LayerMask obstacle;
    [SerializeField] private AudioSource sfxCrateSlide;

    private bool isMoving = false;
    private Vector2 movePoint;

    // Start is called before the first frame update
    private void Start()
    {
        movePoint = transform.position;
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
        if (movable && !isMoving && Physics2D.OverlapCircleAll(checkPos, checkRadius, obstacle).Length < 2)
        {
            movePoint = (Vector2)transform.position + pushVector;
            isMoving = true;
            sfxCrateSlide.Play();
        }
    }

    public void BecomeRigid()
    {
        movable = false;
    }
}
