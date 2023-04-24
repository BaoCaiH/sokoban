using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSink : MonoBehaviour
{
    [SerializeField] private bool sinkable = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject landing;
    [SerializeField] private GameObject crate;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private AudioSource sfxCrateSink;

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

        // Get the artificial-relative distance of the top from the object center
        sinkDistance = top.transform.position.y - transform.position.y;
        remainDistance = sinkDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (sinkable && !isSinking && !sunk && Vector2.Distance(transform.position, landing.transform.position) < .1f)
        {
            isSinking = true;
            landingSprite.enabled = false;
            transform.GetComponent<CrateMove>().movable = false;

            crateSprite.sortingOrder = 0;
            //topSprite.sortingOrder = 1;

            sfxCrateSink.Play();
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
            crateCollider.enabled = false;
            obstacleCollider.enabled = false;
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
