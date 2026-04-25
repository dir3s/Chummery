using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic; // Додано для роботи зі списками

public class DialogueController : MonoBehaviour
{
    [Header("Початкова нода")]
    public DialogueNode firstNode;

    [Header("Посилання на UI")]
    public Image backgroundDisplay;
    public TextMeshProUGUI textDisplay;
    public Transform choiceRoot;
    public GameObject buttonPrefab;

    [Header("Налаштування друку")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float punctuationPause = 0.5f; // Пауза після . ! ?

    private DialogueNode currentNode;
    private bool isTransitioning = false;
    private bool isTyping = false;
    private string fullText;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (firstNode != null) UpdateDialogueContent(firstNode);
    }

    void Update()
    {
        if (isTransitioning) return;

        bool inputPressed = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);

        if (inputPressed)
        {
            if (isTyping) FinishTyping();
            else if (currentNode != null && currentNode.choices.Count == 0 && currentNode.nextLinearNode != null)
            {
                DisplayNode(currentNode.nextLinearNode);
            }
        }
    }

    public void DisplayNode(DialogueNode node)
    {
        if (node == null || isTransitioning) return;
        StartCoroutine(TransitionToNode(node));
    }

    private IEnumerator TransitionToNode(DialogueNode node)
    {
        isTransitioning = true;
        if (SceneTransitionManager.Instance != null)
            yield return StartCoroutine(SceneTransitionManager.Instance.PerformTransition(1f));

        UpdateDialogueContent(node);

        if (SceneTransitionManager.Instance != null)
            yield return StartCoroutine(SceneTransitionManager.Instance.PerformTransition(0f));

        isTransitioning = false;
    }

    private void UpdateDialogueContent(DialogueNode node)
    {
        currentNode = node;
        fullText = node.dialogueText;
        if (node.background != null) backgroundDisplay.sprite = node.background;

        foreach (Transform child in choiceRoot) Destroy(child.gameObject);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(fullText));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        textDisplay.text = "";
        isTyping = true;

        for (int i = 0; i < sentence.Length; i++)
        {
            char letter = sentence[i];
            textDisplay.text += letter;

            // Логіка пауз після знаків припинання
            if (IsPunctuation(letter))
            {
                // Перевіряємо, чи це не "..." (якщо наступний символ теж знак — не паузимо)
                bool isEndOfPunctuation = (i + 1 >= sentence.Length) || !IsPunctuation(sentence[i + 1]);

                if (isEndOfPunctuation)
                {
                    yield return new WaitForSeconds(punctuationPause);
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
        CreateChoices();
    }

    // Метод для перевірки знаків припинання
    private bool IsPunctuation(char c)
    {
        return c == '.' || c == '!' || c == '?' || c == '…';
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        textDisplay.text = fullText;
        isTyping = false;
        CreateChoices();
    }

    private void CreateChoices()
    {
        if (choiceRoot.childCount > 0) return;
        foreach (Choice choice in currentNode.choices)
        {
            GameObject btnObj = Instantiate(buttonPrefab, choiceRoot);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.answerText;
            btnObj.GetComponent<Button>().onClick.AddListener(() => DisplayNode(choice.nextNode));
        }
    }
}