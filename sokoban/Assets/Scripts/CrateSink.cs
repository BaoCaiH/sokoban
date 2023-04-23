using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSink : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject landing;
    [SerializeField] private GameObject crate;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject obstacle;

    private bool isSinking = false;
    private bool sunk = false;
    private float sinkDistance;
    private Vector2 crateMovePoint;
    private Vector2 topMovePoint;
    private SpriteRenderer crateSprite;
    private SpriteRenderer topSprite;
    private SpriteRenderer landingSprite;
    private Collider2D crateCollider;
    private Collider2D obstacleCollider;

    // Start is called before the first frame update
    void Start()
    {
        crateSprite = crate.GetComponent<SpriteRenderer>();
        topSprite = top.GetComponent<SpriteRenderer>();
        landingSprite = landing.GetComponent<SpriteRenderer>();
        crateCollider = transform.GetComponent<Collider2D>();
        obstacleCollider = obstacle.GetComponent<Collider2D>();

        sinkDistance = top.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isSinking && !sunk && Vector2.Distance(transform.position, landing.transform.position) < .1f)
        {
            isSinking = true;
            landingSprite.enabled = false;
            crateCollider.enabled = false;
            obstacleCollider.enabled = false;
            crateMovePoint = (Vector2)crate.transform.position - new Vector2(0f, sinkDistance);
            topMovePoint = landing.transform.position; // + new Vector2(0f, -sinkDistance);

            crateSprite.sortingOrder = 0;
            //topSprite.sortingOrder = 1;
        }

        if (isSinking)
        {
            Debug.Log("Crate " + crate.transform.position + " move to: " + crateMovePoint);
            Sink(crate, crateMovePoint, -1);
            Sink(top, topMovePoint);
            sunk = Vector2.Distance(crate.transform.position, crateMovePoint) < .001f
                && Vector2.Distance(top.transform.position, topMovePoint) < .001f;
            isSinking = !sunk;

        }
    }

    private void Sink(GameObject obj, Vector2 dest, int order = 0)
    {
        float dist = Vector2.Distance(obj.transform.position, dest);
        if (dist > 0f)
        {
            obj.transform.position = Vector2.MoveTowards(
                obj.transform.position,
                dest,
                Mathf.Clamp(speed * Time.deltaTime, -dist, dist)
            );
        }
        else
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }
}
