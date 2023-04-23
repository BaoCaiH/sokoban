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
    private float remainDistance;
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

        sinkDistance = top.transform.position.y - transform.position.y;
        Debug.Log(sinkDistance);
        remainDistance = sinkDistance;
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

            crateSprite.sortingOrder = 0;
            //topSprite.sortingOrder = 1;
        }

        if (isSinking)
        {
            Sinking();
            sunk = remainDistance == 0f;
            isSinking = !sunk;
        }
        if (sunk)
        {
            crateSprite.sortingOrder = -1;
            topSprite.sortingOrder = 0;
        }
    }

    private void Sinking()
    {
        float offset = Mathf.Clamp(speed * Time.deltaTime, 0, remainDistance);
        crate.transform.position -= new Vector3(0f, offset, 0f);
        top.transform.position -= new Vector3(0f, offset, 0f);
        remainDistance -= offset;
    }
}
