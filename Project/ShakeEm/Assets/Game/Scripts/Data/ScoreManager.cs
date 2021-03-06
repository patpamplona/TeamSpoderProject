﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	private static ScoreManager _instance = null;
	public static ScoreManager Instance
	{
		get
		{
			return _instance;
		}
	}

	[SerializeField] private Text scoreText;

	private int score = 0;

	void Awake()
	{
		_instance = this;
		Reset();
	}

	void OnDestroy()
	{
		_instance = null;
	}

	// Use this for initialization
	void Start () 
	{
		scoreText.text = "0";
	}

	public void AddScore(int moreScore)
	{
		score += moreScore;
		score = Mathf.Clamp(score, 0, score);

		scoreText.text = DisplayUtils.ConvertToThousands(score);
	}

	public void Reset()
	{
		score = 0;
		scoreText.text = "0";
	}
}
