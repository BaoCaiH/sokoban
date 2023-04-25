using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSelect : MonoBehaviour
{
    [SerializeField] string sceneName = "PuzzleSelection";
    [SerializeField] CrateSink sinker;
    [SerializeField] GameObject sceneManager;

    private bool chosen = false;
    private SceneSwitch switcher;

    // Start is called before the first frame update
    void Start()
    {
        switcher = sceneManager.GetComponent<SceneSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!chosen && sinker.Sunk())
        {
            chosen = true;
            switcher.ChooseScene(sceneName);
            switcher.Loading();
        }
    }
}
