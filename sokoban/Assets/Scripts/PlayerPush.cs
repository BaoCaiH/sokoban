using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    //[SerializeField] private float checkRadius = 0.45f;
    //[SerializeField] private Transform checkFront;
    //[SerializeField] private LayerMask crate;

    //private float pushHorizontal;
    //private float pushVertical;
    //private Collider2D foundCrate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //pushVertical = Input.GetAxisRaw("Vertical");
        //pushHorizontal = Input.GetAxisRaw("Horizontal");
        //Vector2 direction = pushVertical > 0f ? Vector2.up
        //    : pushVertical < 0f ? Vector2.down
        //    : pushHorizontal > 0f ? Vector2.right
        //    : pushHorizontal < 0f ? Vector2.left
        //    : Vector2.zero;
        //foundCrate = null;
        //if (direction != Vector2.zero)
        //{
        //    foundCrate = Physics2D.OverlapCircle((Vector2)transform.position + direction / 2, checkRadius, crate);
        //}
        //if (foundCrate is not null)
        //{
        //    foundCrate.gameObject.GetComponent<CrateMove>().Push(direction);
        //}
    }
}
