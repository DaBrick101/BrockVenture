using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI healthText;

    public PlayerAi player; // Reference to your player script

    private float timer = 0f;
    private int killCount = 0;
    private bool isPlayerAlive = true;

    private void Start()
    {
        // Optionally, find the player if not assigned
        if (player == null)
            player = FindObjectOfType<PlayerAi>();
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
            UpdateHealthUI();
        }
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = $"{killCount} Kills";
    }

    public void PlayerDied()
    {
        isPlayerAlive = false;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);
        timerText.text = $"{minutes}:{seconds:00}:{milliseconds:00}";
    }

    private void UpdateHealthUI()
    {
        if (player != null)
            healthText.text = $"{Mathf.Max(0, (int)player.health)}/10";
    }
}
