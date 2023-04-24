using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isDead = false;
    [SerializeField] private float speed;
    [SerializeField] private float distanceHorizontal;
    [SerializeField] private float distanceVertical;
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private Transform checkFront;
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask crate;
    [SerializeField] private AudioSource sfxWalk;
    [SerializeField] private LayerMask pit;
    [SerializeField] private AudioSource sfxFall;

    private bool isMoving = false;
    private int direction = 0;
    private int directionHorizontal;
    private int directionVertical;
    private float pushHorizontal;
    private float pushVertical;
    private Vector2 currentPos;
    private Vector2 movePos;
    private Vector2 movePoint;
    private Vector2 pushVector;
    private Vector2 checkBack;
    private ArrayList directions = new();
    private Animator anim;
    private Collider2D foundCrate;
    private InputAction joystickMoveAction;

    // Start is called before the first frame update
    private void Start()
    {
        movePos = transform.position;
        anim = GetComponent<Animator>();
        joystickMoveAction = GetComponent<PlayerInput>().actions["Move"];
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead) {return;}

        // Current Position
        currentPos = transform.position;

        // Input axes
        pushVertical = Input.GetAxisRaw("Vertical");
        pushHorizontal = Input.GetAxisRaw("Horizontal");
        pushVector = pushVertical > 0f || JoystickUp() ? Vector2.up
            : pushVertical < 0f || JoystickDown() ? Vector2.down
            : pushHorizontal > 0f || JoystickRight() ? Vector2.right
            : pushHorizontal < 0f || JoystickLeft() ? Vector2.left
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
        // If still moving then keep direction
        direction = isMoving ? direction
            // If joystick not neutral then take joystick
            : !JoystickNeutral() ? JoystickDirection()
            // Change direction using the first direction in the queue
            : directions.Count > 0 ? (int)directions[0] : direction;
        // Change animation state to the moving direction,
        // Otherwise snap to idle of that current direction
        anim.SetInteger("state", isMoving ? direction : 0);

        // Update check point
        directionVertical = direction == 1 ? 1 : direction == 2 ? -1 : 0;
        directionHorizontal = direction == 3 ? -1 : direction == 4 ? 1 : 0;
        movePos = new Vector2(
            currentPos.x + directionHorizontal * distanceHorizontal,
            currentPos.y + directionVertical * distanceVertical
            );
        checkFront.position = new Vector2(
            currentPos.x + directionHorizontal * distanceHorizontal / 2,
            currentPos.y + directionVertical * distanceVertical / 2
            );
        checkBack = new Vector2(
            currentPos.x - directionHorizontal * distanceHorizontal / 4,
            currentPos.y - directionVertical * distanceVertical / 4
            );

        // Update move toward point if previous action is finished
        if (!isMoving && (KeyDirectionAny() || !JoystickNeutral()) && !Physics2D.OverlapCircle(
            checkFront.position,
            checkRadius, wall
        ))
        {
            isMoving = true;
            movePoint = movePos;
            sfxWalk.Play();
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

        if (Physics2D.OverlapCircle(checkBack, checkRadius / 4, pit))
        {
            Die();
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

    private bool JoystickUp()
    {
        return joystickMoveAction.ReadValue<Vector2>().y > 0f;
    }

    private bool KeyUpHold()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
    }

    private bool KeyUpUp()
    {
        return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);
    }

    private bool JoystickDown()
    {
        return joystickMoveAction.ReadValue<Vector2>().y < 0f;
    }

    private bool KeyDownHold()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    private bool KeyDownUp()
    {
        return Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);
    }

    private bool JoystickLeft()
    {
        return joystickMoveAction.ReadValue<Vector2>().y == 0f
            && joystickMoveAction.ReadValue<Vector2>().x < 0f;
    }

    private bool KeyLeftHold()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

    private bool KeyLeftUp()
    {
        return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
    }

    private bool JoystickRight()
    {
        return joystickMoveAction.ReadValue<Vector2>().y == 0f
            && joystickMoveAction.ReadValue<Vector2>().x > 0f;
    }

    private bool KeyRightHold()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    }

    private bool KeyRightUp()
    {
        return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
    }

    private bool JoystickNeutral()
    {
        return joystickMoveAction.ReadValue<Vector2>() == Vector2.zero;
    }

    private bool KeyDirectionAny()
    {
        return KeyUpHold() || KeyDownHold() || KeyLeftHold() || KeyRightHold();
    }

    private int JoystickDirection()
    {
        return JoystickUp() ? 1 : JoystickDown() ? 2 : JoystickLeft() ? 3 : JoystickRight() ? 4 : 0;
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("fall");
        sfxFall.Play();
        Debug.Log("NOOOOOOO!");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
