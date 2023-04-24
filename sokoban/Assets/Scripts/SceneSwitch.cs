using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] int layerIndex;
    [SerializeField] string nextScene;
    [SerializeField] GameObject crates;

    private bool loading = false;
    private List<BoxCollider2D> objectiveCrates = new();
    private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim = transform.GetComponent<Animator>();
        foreach (BoxCollider2D collider in crates.GetComponentsInChildren<BoxCollider2D>())
        {
            if (collider.gameObject.layer == layerIndex)
            {
                Debug.Log(collider);
                objectiveCrates.Add(collider);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!loading && RemainingCrate() == 0)
        {
            Loading();
        }
    }

    private int RemainingCrate()
    {
        int cnt = 0;
        foreach (BoxCollider2D collider in objectiveCrates)
        {
            cnt += collider.enabled == true ? 1 : 0;
        }
        return cnt;
    }

    private void Loading()
    {
        loading = true;
        anim.SetTrigger("out");
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
