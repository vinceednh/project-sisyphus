using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private InteractableManager interactableManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button gameOverButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    private bool gameOverTriggered = false;

    private void Start()
    {
        // Find references if not assigned in inspector
        if (healthBar == null) healthBar = FindAnyObjectByType<HealthBar>();
        if (interactableManager == null) interactableManager = FindAnyObjectByType<InteractableManager>();

        // Hide game over screen initially
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Setup button listener
        if (gameOverButton != null)
            gameOverButton.onClick.AddListener(OnGameOverButtonClicked);
    }

    private void Update()
    {
        if (gameOverTriggered) return;

        // Check if health reached 0 (loss condition)
        if (healthBar != null && healthBar.CurrentHealth <= 0)
        {
            ShowGameOver("GAME OVER", "TRY AGAIN");
            gameOverTriggered = true;
        }
        // Check if all 5 beacons activated (win condition)
        else if (interactableManager != null && interactableManager.BeaconsActivated >= 5)
        {
            ShowGameOver("YOU WON", "PLAY AGAIN");
            gameOverTriggered = true;
        }
    }

    private void ShowGameOver(string text, string buttonLabel)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Fade background to black
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(0, 0, 0, 1f); // Fully opaque black
        }

        if (gameOverText != null)
            gameOverText.text = text;

        if (buttonText != null)
            buttonText.text = buttonLabel;

        // Enable cursor so player can click button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnGameOverButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
