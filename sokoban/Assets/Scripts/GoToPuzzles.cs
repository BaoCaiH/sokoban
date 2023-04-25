using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToPuzzles : MonoBehaviour
{
	[SerializeField] string puzzleSelectSceneName = "PuzzleSelection";
	[SerializeField] GameObject sceneManager;

	private SceneSwitch switcher;

	public Button puzzleButton;

	void Start()
	{
		switcher = sceneManager.GetComponent<SceneSwitch>();
		Button btn = puzzleButton.GetComponent<Button>();
		btn.onClick.AddListener(Go);
	}

	private void Go()
	{
		switcher.ChooseScene(puzzleSelectSceneName);
		switcher.Loading();
	}
}
