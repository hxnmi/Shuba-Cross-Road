using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
	[SerializeField] TMP_Text mainMenuText;
	
	void Start()
	{
		int highScore = PlayerPrefs.GetInt("HighScore");
		
		mainMenuText.text = "HighScore : " + highScore;
	}
}
	