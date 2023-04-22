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
    private Vector2 currentPos;
    private Vector2 movePoint;
    private ArrayList directions = new();
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
        // Current Position
        currentPos = transform.position;

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

        // Update move toward point if previous action is finished
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
