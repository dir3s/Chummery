using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    // Статичне посилання, щоб звертатися до менеджера звідусіль
    public static SceneTransitionManager Instance;

    [Header("Components")]
    [SerializeField] private CanvasGroup faderCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private bool glitchEffect = true;

    private void Awake()
    {
        // Робимо об'єкт синглтоном
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(PerformTransition(0));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionSequence(sceneName));
    }

    private IEnumerator TransitionSequence(string sceneName)
    {
        yield return StartCoroutine(PerformTransition(1));

        SceneManager.LoadScene(sceneName);

        yield return StartCoroutine(PerformTransition(0));
    }

    public IEnumerator PerformTransition(float targetAlpha)
    {
        float startAlpha = faderCanvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeDuration;

            if (glitchEffect && targetAlpha > 0.5f) 
            {
                float noise = Random.Range(-0.1f, 0.1f);
                faderCanvasGroup.alpha = Mathf.Clamp01(Mathf.Lerp(startAlpha, targetAlpha, progress) + noise);
            }
            else
            {
                faderCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            }

            yield return null;
        }

        faderCanvasGroup.alpha = targetAlpha;
    }
}