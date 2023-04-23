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

    private bool isSink = false;
    private float sinkDistance;
    private Vector2 crateMovePoint;
    private Vector2 topMovePoint;
    private Transform crateTransform;
    private Transform topTransform;
    private Transform landingTransform;
    private SpriteRenderer crateSprite;
    private SpriteRenderer topSprite;
    private SpriteRenderer landingSprite;
    private Collider2D crateCollider;
    private Collider2D obstacleCollider;

    // Start is called before the first frame update
    void Start()
    {
        crateTransform = crate.transform;
        topTransform = top.transform;
        landingTransform = landing.transform;
        crateSprite = crate.GetComponent<SpriteRenderer>();
        topSprite = top.GetComponent<SpriteRenderer>();
        landingSprite = landing.GetComponent<SpriteRenderer>();
        crateCollider = transform.GetComponent<Collider2D>();
        obstacleCollider = obstacle.GetComponent<Collider2D>();

        sinkDistance = topTransform.position.y;
        crateMovePoint = (Vector2)landingTransform.position + new Vector2(0f, -sinkDistance);
        topMovePoint = landingTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, landingTransform.position) < .1f)
        {
            landingSprite.enabled = false;
            crateSprite.sortingOrder = 0;
            topSprite.sortingOrder = 1;
            isSink = true;
        }

        if (isSink)
        {
            Sink(crateTransform, crateMovePoint);
            Sink(topTransform, topMovePoint);
        }
    }

    private void Sink(Transform obj, Vector2 dest)
    {
        if (Vector2.Distance(obj.position, dest) > 0f)
        {
            obj.position = Vector2.MoveTowards(
                obj.position,
                dest,
                speed * Time.deltaTime
            );
        }
        else
        {
            crateSprite.sortingOrder--;
            topSprite.sortingOrder--;
            isSink = false;
            crateCollider.enabled = false;
            obstacleCollider.enabled = false;
        }
    }
}
