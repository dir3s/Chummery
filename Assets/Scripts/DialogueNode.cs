using UnityEngine;
using System.Collections.Generic;

// Це дозволить нам створювати нові кадри через меню правої кнопки миші
[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Візуал")]
    public Sprite background; // Картинка фону

    [Header("Текст")]
    [TextArea(3, 10)]
    public string dialogueText; // Основний текст кадру

    [Header("Варіанти вибору")]
    public List<Choice> choices; // Список кнопок-відповідей

    [Header("Лінійний перехід")]
    public DialogueNode nextLinearNode;
}

[System.Serializable]
public class Choice
{
    public string answerText;      // Що написано на кнопці
    public DialogueNode nextNode; // Куди ми перейдемо після натискання
}