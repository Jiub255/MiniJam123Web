using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI spidersKilledText;
	[SerializeField]
	private TextMeshProUGUI timeText;

	public float timer = 0f;
	public int spidersKilled = 0;

    private void Awake()
    {
		spidersKilledText.text = "Spiders Killed: 0";
		timeText.text = "Time: 0:00";
    }

    private void OnEnable()
    {
		Bullet.onSpiderKilled += UpdateScore;
    }

    private void OnDisable()
    {
		Bullet.onSpiderKilled -= UpdateScore;
    }

    private void Update()
    {
		timer += Time.deltaTime;

		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer) % 60;
		int centiseconds = Mathf.FloorToInt((timer - Mathf.FloorToInt(timer)) * 100);

		string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);

		timeText.text = "Time: " + formattedTime;
    }

	public void UpdateScore()
    {
		spidersKilled++;
		spidersKilledText.text = "Spiders Killed: " + spidersKilled;
    }
}