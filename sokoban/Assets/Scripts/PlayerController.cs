using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distanceHorizontal;
    [SerializeField] private float distanceVertical;
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private Transform checkFront;
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask crate;

    private bool isMoving = false;
    //private bool isMoveUp = false;
    private int direction = 0;
    private float pushHorizontal;
    private float pushVertical;
    private Vector2 currentPos;
    private Vector2 movePos;
    private Vector2 movePoint;
    private Vector2 pushVector;
    private ArrayList directions = new();
    //private SpriteRenderer sprite;
    private Animator anim;
    private Collider2D foundCrate;

    // Start is called before the first frame update
    private void Start()
    {
        movePos= transform.position;
        //movePoint = transform.position;
        //sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Current Position
        currentPos = transform.position;

        // Input axes
        pushVertical = Input.GetAxisRaw("Vertical");
        pushHorizontal = Input.GetAxisRaw("Horizontal");
        pushVector = pushVertical > 0f ? Vector2.up
            : pushVertical < 0f ? Vector2.down
            : pushHorizontal > 0f ? Vector2.right
            : pushHorizontal < 0f ? Vector2.left
            : Vector2.zero;
        foundCrate = null;

        // Input Queue
        // This will give the effects that the next input would be consider
        // when the previous one is released
        if (KeyUpHold() && !directions.Contains(1))
        {
            directions.Add(1);
        }
        else if (KeyDownHold() && !directions.Contains(2))
        {
            directions.Add(2);
        }
        else if (KeyLeftHold() && !directions.Contains(3))
        {
            directions.Add(3);
        }
        else if (KeyRightHold() && !directions.Contains(4))
        {
            directions.Add(4);
        }

        // Dequeue on release
        directions.Remove(KeyUpUp() ? 1 : KeyDownUp() ? 2 : KeyLeftUp() ? 3 : KeyRightUp() ? 4 : 0);

        // Check Direction
        // Change direction using the first direction in the queue
        direction = isMoving ? direction : directions.Count > 0 ? (int)directions[0] : direction;
        // Change animation state to the moving direction,
        // Otherwise snap to idle of that current direction
        anim.SetInteger("state", isMoving ? direction : 0);

        // Update check point
        switch (direction)
        {
            case 1:
                movePos = new Vector2(currentPos.x, currentPos.y + distanceVertical);
                checkFront.position = new Vector2(currentPos.x, currentPos.y + distanceVertical / 2);
                break;
            case 2:
                movePos = new Vector2(currentPos.x, currentPos.y - distanceVertical);
                checkFront.position = new Vector2(currentPos.x, currentPos.y - distanceVertical / 2);
                break;
            case 3:
                movePos = new Vector2(currentPos.x - distanceHorizontal, currentPos.y);
                checkFront.position = new Vector2(currentPos.x - distanceHorizontal / 2, currentPos.y);
                break;
            case 4:
                movePos = new Vector2(currentPos.x + distanceHorizontal, currentPos.y);
                checkFront.position = new Vector2(currentPos.x + distanceHorizontal / 2, currentPos.y);
                break;
            default:
                break;
        }

        // Update move toward point if previous action is finished
        if (!isMoving && KeyDirectionAny() && !Physics2D.OverlapCircle(
            checkFront.position,
            checkRadius, wall
        ))
        {
            isMoving = true;
            movePoint = movePos;
        }
        // Only push if not moving
        else if (!isMoving)
        {
            if (pushVector != Vector2.zero)
            {
                foundCrate = Physics2D.OverlapCircle((Vector2)transform.position + pushVector / 2, checkRadius, crate);
            }
            if (foundCrate is not null)
            {
                foundCrate.gameObject.GetComponent<CrateMove>().Push(pushVector);
            }
        }

            // Move
            if (isMoving && Distance() > 0f)
        {
            MoveTowards();
        }
        else if (isMoving)
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

    private float Distance()
    {
        return Vector2.Distance(transform.position, movePoint);
    }

    private bool KeyUpHold()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
    }

    private bool KeyUpUp()
    {
        return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);
    }

    private bool KeyDownHold()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    private bool KeyDownUp()
    {
        return Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);
    }

    private bool KeyLeftHold()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

    private bool KeyLeftUp()
    {
        return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
    }

    private bool KeyRightHold()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    }

    private bool KeyRightUp()
    {
        return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
    }

    private bool KeyDirectionAny()
    {
        return KeyUpHold() || KeyDownHold() || KeyLeftHold() || KeyRightHold();
    }
}
