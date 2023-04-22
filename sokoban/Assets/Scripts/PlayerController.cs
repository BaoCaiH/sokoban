using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distanceHorizontal;
    [SerializeField] private float distanceVertical;
    [SerializeField] private float lagTime;
    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private Transform checkFront;
    [SerializeField] private LayerMask collide;

    private bool isMoving = false;
    private bool isMoveUp = false;
    private int direction = 0;
    private Vector2 checkPosition;
    private Vector2 currentPos;
    private Vector2 movePoint;
    private SpriteRenderer sprite;
    private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        movePoint = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(isMoving);
        // Current Position
        currentPos = transform.position;
        // Check Direction
        //isMoving? direction : 
        if (!isMoving)
        {
            direction = KeyUp() ? 1 : KeyDown() ? 2 : KeyLeft() ? 3 : KeyRight() ? 4 : direction;
        }
        anim.SetInteger("state", isMoving ? direction : 0);
        switch (direction)
        {
            case 1:
                checkFront.position = new Vector2(currentPos.x, currentPos.y + distanceVertical);
                break;
            case 2:
                checkFront.position = new Vector2(currentPos.x, currentPos.y - distanceVertical);
                break;
            case 3:
                checkFront.position = new Vector2(currentPos.x - distanceHorizontal, currentPos.y);
                break;
            case 4:
                checkFront.position = new Vector2(currentPos.x + distanceHorizontal, currentPos.y);
                break;
            default:
                break;
        }

        if (!isMoving && KeyDirectionAny())
        {
            isMoving = true;
            movePoint = checkFront.position;
            switch (direction)
            {
                case 1:
                    isMoveUp = true;
                    break;
                case 2:
                    sprite.sortingOrder++;
                    break;
                default:
                    break;
            }
        }

        //if (!isMoving && KeyUp())
        //{
        //    isMoving = true;
        //    isFaceUp = true;
        //    anim.SetInteger("state", 1);
        //    //movePoint = Physics2D.OverlapCircle(checkPosition, checkRadius, collide) ? transform.position : checkPosition;
        //    movePoint = checkPosition;
        //    Debug.Log(Physics2D.OverlapCircle(checkPosition, checkRadius, collide));
        //}
        //else if (!isMoving && KeyDown())
        //{
        //    isMoving = true;
        //    sprite.sortingOrder++; // Update layer first when moving down
        //    anim.SetInteger("state", 2);
        //    movePoint = checkPosition;
        //    Debug.Log(Physics2D.OverlapCircle(checkPosition, checkRadius, collide));
        //}
        //else if (!isMoving && KeyLeft())
        //{
        //    isMoving = true;
        //    anim.SetInteger("state", 3);
        //    movePoint = checkPosition;
        //    Debug.Log(Physics2D.OverlapCircle(checkPosition, checkRadius, collide));
        //}
        //else if (!isMoving && KeyRight())
        //{
        //    isMoving = true;
        //    anim.SetInteger("state", 4);
        //    movePoint = checkPosition;
        //    Debug.Log(Physics2D.OverlapCircle(checkPosition, checkRadius, collide));
        //}

        //movePoint = Physics2D.OverlapCircle(checkPosition, checkRadius, collide) ? movePoint : checkPosition;

        // Move
        if (isMoving && Distance() > 0f)
        {
            MoveTowards();
        }
        else if (isMoving)
        {
            isMoving = false;
            if (isMoveUp)
            {
                sprite.sortingOrder--;
                isMoveUp = false;
            }
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

    private float Distance()
    {
        Vector2 curr = transform.position;
        float x1 = curr.x;
        float y1 = curr.y;
        float x2 = movePoint.x;
        float y2 = movePoint.y;
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

    private bool KeyUp()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
    }

    private bool KeyDown()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        //return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
    }

    private bool KeyLeft()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

    private bool KeyRight()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    }

    private bool KeyDirectionAny()
    {
        return KeyUp() || KeyDown() || KeyLeft() || KeyRight();
    }
}
