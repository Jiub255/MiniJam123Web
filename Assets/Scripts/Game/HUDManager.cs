using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI spidersKilledText;
	[SerializeField]
	private TextMeshProUGUI timeText;
	[SerializeField]
	private TextMeshProUGUI winText;
	[SerializeField]
	private TextMeshProUGUI deathText;

	[SerializeField]
	private GameObject hudCanvas;
	[SerializeField]
	private GameObject winCanvas;
	[SerializeField]
	private GameObject deathCanvas;

	[HideInInspector]
	public float timer = 0f;
	[HideInInspector]
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
		Movement.onBossLevel += OpenBossHealthHUD;
		BossHealth.onWin += OpenWinScreen;
		Movement.onDeath += OpenDeathScreen;
		Venom.onDeath += OpenDeathScreen;
		EnemyMovement.onDeath += OpenDeathScreen;
		BossSpiderMovement.onDeath += OpenDeathScreen;
    }

    private void OnDisable()
    {
		Bullet.onSpiderKilled -= UpdateScore;
		Movement.onBossLevel -= OpenBossHealthHUD;
		BossHealth.onWin -= OpenWinScreen;
		Movement.onDeath -= OpenDeathScreen;
		Venom.onDeath -= OpenDeathScreen;
		EnemyMovement.onDeath -= OpenDeathScreen;
		BossSpiderMovement.onDeath -= OpenDeathScreen;
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
        }
    }

    private void FixedUpdate()
    {
		UpdateBossHealth();

		timer += Time.fixedDeltaTime;

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

	private void OpenDeathScreen(string deathMessage)
    {
		hudCanvas.SetActive(false);
		deathCanvas.SetActive(true);
		deathText.text = "You died!\n" + deathMessage;
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
		return Mathf.RoundToInt(1000 * (spidersKilled + 1) / timer);
    }
}