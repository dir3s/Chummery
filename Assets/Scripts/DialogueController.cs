using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [Header("Початкова нода")]
    public DialogueNode firstNode;

    [Header("Посилання на UI")]
    public Image backgroundDisplay;
    public TextMeshProUGUI textDisplay;
    public Transform choiceRoot;
    public GameObject buttonPrefab;

    private DialogueNode currentNode;
    private bool isTransitioning = false;

    void Start()
    {
        // Виправляємо баг "подвійної анімації":
        // Завантажуємо контент першої ноди миттєво без запуску корутини переходу.
        // SceneTransitionManager сам зробить плавний вхід у сцену.
        if (firstNode != null)
        {
            UpdateDialogueContent(firstNode);
        }
    }

    void Update()
    {
        if (isTransitioning) return;

        if (currentNode != null && currentNode.choices.Count == 0 && currentNode.nextLinearNode != null)
        {
            bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

            if (mouseClicked || spacePressed)
            {
                DisplayNode(currentNode.nextLinearNode);
            }
        }
    }

    public void DisplayNode(DialogueNode node)
    {
        if (node == null || isTransitioning) return;

        // Для всіх наступних переходів використовуємо анімацію
        StartCoroutine(TransitionToNode(node));
    }

    private IEnumerator TransitionToNode(DialogueNode node)
    {
        isTransitioning = true;

        if (SceneTransitionManager.Instance != null)
        {
            yield return StartCoroutine(SceneTransitionManager.Instance.PerformTransition(1f));
        }

        UpdateDialogueContent(node);

        if (SceneTransitionManager.Instance != null)
        {
            yield return StartCoroutine(SceneTransitionManager.Instance.PerformTransition(0f));
        }

        isTransitioning = false;
    }

    private void UpdateDialogueContent(DialogueNode node)
    {
        currentNode = node;
        textDisplay.text = node.dialogueText;
        if (node.background != null) backgroundDisplay.sprite = node.background;

        foreach (Transform child in choiceRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (Choice choice in node.choices)
        {
            GameObject btnObj = Instantiate(buttonPrefab, choiceRoot);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.answerText;
            btnObj.GetComponent<Button>().onClick.AddListener(() => DisplayNode(choice.nextNode));
        }
    }
}