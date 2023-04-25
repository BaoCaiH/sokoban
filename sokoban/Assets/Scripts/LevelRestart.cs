using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
	[SerializeField] GameObject sceneManager;

	private SceneSwitch switcher;

	public Button restartButton;

	void Start()
	{
		switcher = sceneManager.GetComponent<SceneSwitch>();
		Button btn = restartButton.GetComponent<Button>();
		btn.onClick.AddListener(Restart);
	}

	private void Restart()
	{
		switcher.ChooseScene(SceneManager.GetActiveScene().name);
		switcher.Loading();
	}
}
