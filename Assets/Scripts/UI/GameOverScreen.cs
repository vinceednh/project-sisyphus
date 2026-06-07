
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private InteractableManager interactableManager;
    [SerializeField] private Image fadeImage;

    private bool gameOverTriggered = false;

    private void Start()
    {
        if (healthBar == null) healthBar = FindAnyObjectByType<HealthBar>();
        if (interactableManager == null) interactableManager = FindAnyObjectByType<InteractableManager>();

        if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if (gameOverTriggered) return;

        if (healthBar != null && healthBar.CurrentHealth <= 0)
        {
            gameOverTriggered = true;
            StartCoroutine(FadeAndLoad(SceneManager.GetActiveScene().name));
        }
        else if (interactableManager != null && interactableManager.BeaconsActivated >= 5)
        {
            gameOverTriggered = true;
            StartCoroutine(FadeAndLoad("End"));
        }
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        float timer = 0;
        float fadeDuration = 1.5f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
