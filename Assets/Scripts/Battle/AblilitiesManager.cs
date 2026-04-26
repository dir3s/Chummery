using UnityEngine;
using TMPro;
using System.Collections;
public class AbilityManager : MonoBehaviour
{
    public BattleManager battle;

    public CanvasGroup abilitiesGroup;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject abilitiesPanel;

    public GameObject attackButton;
    public GameObject endTurnButton;
    public GameObject abilitiesButton;
    public GameObject ManaBar_BG;

    private bool isOpen = false;

    public Ability[] abilities;

    private void Start()
    {
        DisableAbilities();
    }

    public void EnableAbilities()
    {
        abilitiesPanel.SetActive(true);
        abilitiesGroup.interactable = true;
        abilitiesGroup.blocksRaycasts = true;
        abilitiesGroup.alpha = 1f;
    }

    public void DisableAbilities()
    {

        abilitiesGroup.interactable = false;
        abilitiesGroup.blocksRaycasts = false;
        abilitiesGroup.alpha = 0.3f;
        abilitiesPanel.SetActive(false);
    }

   

    public void ShowAbilityInfo(int index)
    {
        nameText.text = abilities[index].name;
        descriptionText.text = abilities[index].description;
    }


    public void UseAbility(int index)
    {
        StartCoroutine(UseAbilityRoutine(index));
    }

    IEnumerator UseAbilityRoutine(int index)
    {
        UIManager.Instance.HideBattleUI();
        var player = BattleManager.Instance.player;
        var enemy = BattleManager.Instance.enemy;
        Ability ability = abilities[index];

        if (battle.player.currentMana < ability.manaCost)
        {
            Debug.Log("Not enough mana!");
            UIManager.Instance.ShowBattleUI();
            yield break;
        }

        Vector3 startPos = player.transform.position;
        Vector3 attackPos = enemy.transform.position + new Vector3(-1f, 0, 0);


        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            float eased = t * (2f - t);

            player.transform.position =
                Vector3.Lerp(startPos, attackPos, eased);

            yield return null;
        }

        player.SetSprite(ability.abilitySprite);

        yield return new WaitForSeconds(0.2f);


        battle.player.currentMana -= ability.manaCost;
        UIManager.Instance.RefreshUI();

        enemy.TakeDamage(ability.damage);

        CameraShake.Instance.Shake(0.15f, 0.2f);
        StartCoroutine(BattleManager.Instance.Shake(enemy.transform));

        yield return new WaitForSeconds(0.3f);


        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;

            player.transform.position =
                Vector3.Lerp(attackPos, startPos, t);

            yield return null;
        }


        player.SetSprite(player.idleSprite);
        yield return new WaitForSeconds(0.3f);
        UIManager.Instance.ShowBattleUI();
        DisableAbilities();
        CloseAbilities();
        
        battle.EndPlayerTurn();
    }




    public void HighlightButton(GameObject button)
    {
        button.transform.localScale = Vector3.one * 1.1f;
    }

    public void UnhighlightButton(GameObject button)
    {
        button.transform.localScale = Vector3.one;
    }


    public void ToggleAbilities()
    {
        if (isOpen)
        {
            CloseAbilities();
        }
        else
        {
            OpenAbilities();
        }
    }

    public void OpenAbilities()
    {
        abilitiesPanel.SetActive(true);
        abilitiesGroup.interactable = true;
        abilitiesGroup.blocksRaycasts = true;
        abilitiesGroup.alpha = 1f;
        isOpen = true;

        attackButton.SetActive(false);
        endTurnButton.SetActive(false);
        abilitiesButton.SetActive(false);
        ManaBar_BG.SetActive(false);
    }

    public void CloseAbilities()
    {
        abilitiesPanel.SetActive(false);
        abilitiesGroup.interactable = false;
        abilitiesGroup.blocksRaycasts = false;
        abilitiesGroup.alpha = 0.3f;

        attackButton.SetActive(true);
        endTurnButton.SetActive(true);
        abilitiesButton.SetActive(true);
        ManaBar_BG.SetActive(true);

        isOpen = false;
    }
}