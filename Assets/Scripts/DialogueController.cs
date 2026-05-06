using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    public DialogueNode firstNode;
    public Image backgroundDisplay;
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI speakerNameDisplay;
    public Transform choiceRoot;
    public GameObject buttonPrefab;

    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float punctuationPause = 0.5f;

    private DialogueNode currentNode;
    private bool isTransitioning = false;
    private bool isTyping = false;
    private string fullText;
    private Coroutine typingCoroutine;

    [SerializeField] private DialogueNode[] allNodes;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        Debug.Log("Nodes loaded: " + allNodes.Length);
    }

    void Start()
    {
        string savedID = SaveLastNode.Load();

        Debug.Log("SAVED ID: " + savedID);

        DialogueNode node = GetNodeByID(savedID);

        Debug.Log("FOUND NODE: " + (node != null ? node.nodeID : "NULL"));

        if (node != null)
        {
            UpdateDialogueContent(node);
            return;
        }

        Debug.Log("FALLBACK TO FIRST NODE");

        if (firstNode != null)
            UpdateDialogueContent(firstNode);
    }

    void Update()
    {
        if (isTransitioning || PauseMenu.isPaused) return;

        bool inputPressed = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);

        if (inputPressed)
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;

            if (isTyping) FinishTyping();
            else if (currentNode != null)
            {
                if (!string.IsNullOrEmpty(currentNode.nextSceneName))
                {
                    SceneTransitionManager.Instance.LoadScene(currentNode.nextSceneName);
                    isTransitioning = true;
                    return;
                }

                if (currentNode.choices.Count == 0 && currentNode.nextLinearNode != null)
                {
                    DisplayNode(currentNode.nextLinearNode);
                }
            }
        }
    }

    public float GetCurrentTypingSpeed() => typingSpeed;

    public void SetTypingSpeed(float value)
    {
        typingSpeed = value;
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

        SaveLastNode.Save(node.nodeID);


        if (DialogueSaveSystem.Instance != null)
        {
            DialogueSaveSystem.Instance.MarkVisited(node.nodeID);
            
        }

        if (speakerNameDisplay != null)
        {
            bool hasSpeaker = !string.IsNullOrEmpty(node.speakerName);
            speakerNameDisplay.text = hasSpeaker ? node.speakerName : "";
            speakerNameDisplay.transform.parent.gameObject.SetActive(hasSpeaker);
        }

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

            if (IsPunctuation(letter))
            {
                bool isEndOfPunctuation = (i + 1 >= sentence.Length) || !IsPunctuation(sentence[i + 1]);
                yield return new WaitForSeconds(isEndOfPunctuation ? punctuationPause : typingSpeed);
            }
            else yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        CreateChoices();
    }

    private bool IsPunctuation(char c) => c == '.' || c == '!' || c == '?' || c == '…';

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

    private DialogueNode GetNodeByID(string id)
    {
        foreach (var node in allNodes)
        {
            if (node.nodeID == id)
                return node;
        }
        return null;
    }
}