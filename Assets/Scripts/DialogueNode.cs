using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("ID")]
    public string nodeID;

    [Header("Персонаж")]
    public string speakerName;

    [Header("Візуал")]
    public Sprite background;

    [Header("Текст")]
    [TextArea(3, 10)]
    public string dialogueText;

    [Header("Варіанти вибору")]
    public List<Choice> choices;

    [Header("Лінійний перехід")]
    public DialogueNode nextLinearNode;
}
[System.Serializable]
public class Choice
{
    public string answerText;
    public DialogueNode nextNode;
}