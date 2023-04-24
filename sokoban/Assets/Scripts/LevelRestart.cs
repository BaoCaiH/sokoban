using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
	public Button restartButton;

	void Start()
	{
		Button btn = restartButton.GetComponent<Button>();
		btn.onClick.AddListener(Restart);
	}

	private void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
