using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI spidersKilledText;
	[SerializeField]
	private TextMeshProUGUI timeText;

	[SerializeField]
	private GameObject hudCanvas;
	[SerializeField]
	private GameObject winCanvas;
	[SerializeField]
	private TextMeshProUGUI winText;

	public float timer = 0f;
	public int spidersKilled = 0;

	[SerializeField]
	private Image healthBarImage;
	private BossHealth bossHealth;
	[SerializeField]
	private GameObject bossHealthPanel;

    private void Awake()
    {
		spidersKilledText.text = "0";
		timeText.text = "0:00";

		StartCoroutine(GetBossReference());
    }

	private IEnumerator GetBossReference()
    {
		yield return new WaitForSeconds(1f);

		bossHealth = FindObjectOfType<BossHealth>();
    }

    private void OnEnable()
    {
		Bullet.onSpiderKilled += UpdateScore;
		//Bullet.onHitBoss += UpdateBossHealth;
		BossHealth.onWin += OpenWinScreen;
		Movement.onBossLevel += OpenBossHealthHUD;
    }

    private void OnDisable()
    {
		Bullet.onSpiderKilled -= UpdateScore;
		//Bullet.onHitBoss -= UpdateBossHealth;
		BossHealth.onWin -= OpenWinScreen;
		Movement.onBossLevel -= OpenBossHealthHUD;
	}

    private void Start()
    {
		healthBarImage.fillAmount = 1f;
    }

    private void OpenBossHealthHUD()
    {
		bossHealthPanel.SetActive(true);
    }

    private void UpdateBossHealth()
    {
		if (bossHealth != null)
        {
			healthBarImage.fillAmount = Mathf.Clamp((float)bossHealth.health / (float)bossHealth.maxHealth, 0f, 1f);
			Debug.Log("Fill amount: " + healthBarImage.fillAmount.ToString());
        }
    }

    private void Update()
    {
		UpdateBossHealth();

		timer += Time.deltaTime;

		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer) % 60;
		int centiseconds = Mathf.FloorToInt((timer - Mathf.FloorToInt(timer)) * 100);

		string formattedTime = new string("");

		if (minutes > 0)
        {
			formattedTime = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, centiseconds);
        }
        else
        {
			formattedTime = string.Format("{0}:{1:00}", seconds, centiseconds);
		}

		timeText.text = formattedTime;
    }

	public void UpdateScore()
    {
		spidersKilled++;
		spidersKilledText.text = spidersKilled.ToString();
    }

	private void OpenWinScreen()
    {
		hudCanvas.SetActive(false);
		winCanvas.SetActive(true);
		winText.text = "You Win!\nScore: " + CalculateScore();
		Time.timeScale = 0f;
    }

	// Call from button on win screen UI
	public void StartNewGame()
    {
		hudCanvas?.SetActive(true);
		winCanvas?.SetActive(false);
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public int CalculateScore()
    {
		return Mathf.RoundToInt(100 * (spidersKilled + 1) / timer);
    }
}