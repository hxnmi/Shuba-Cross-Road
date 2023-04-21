using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
	[SerializeField] TMP_Text gameOverText;
	
	public void UpdateText(int score)
	{
		int highScore;
		if (score>PlayerPrefs.GetInt("HighScore"))
		{
			highScore = score;
			SetHighScore(score);
		}
		else
			highScore = LoadHighScore();
		
		gameOverText.text = "Game Over!\nScore: " + score + "\nYour High Score: " + highScore;
	}
	
	public void SetHighScore(int score)
	{
		PlayerPrefs.SetInt("HighScore", score);
	}
	
	public int LoadHighScore()
	{
		var score = PlayerPrefs.GetInt("HighScore");
		return score;
	}
}
