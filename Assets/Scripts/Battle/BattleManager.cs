using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Dialogue Nodes")]
    public string winNodeID;
    public string loseNodeID;
    public string sceneNameAfterBattle;

    private bool battleEnded = false;

    public bool playerTurn = true;

    private bool canDodge = false;
    private bool dodged = false;

    public Unit player;
    public Unit enemy;

    private Vector3 playerStartPos;
    private Vector3 enemyStartPos;

    [SerializeField] private Sprite idleSpriteEnemy;
    [SerializeField] private Sprite idleSpritePlayer;

    [SerializeField] private Sprite dodgeSprite;
    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Sprite hitSpritePlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerStartPos = player.transform.position;
        enemyStartPos = enemy.transform.position;

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (battleEnded) return;

        playerTurn = true;
        Debug.Log("Player Turn Started");
    }

    public void EndPlayerTurn()
    {
        if (battleEnded) return;

        playerTurn = false;
        Debug.Log("Enemy Turn Started");

        EnemyTurn();
    }

    void EnemyTurn()
    {
        if (battleEnded) return;

        Debug.Log("Enemy attacks!");
        StartCoroutine(EnemyAttackRoutine());
    }

    public void PlayerAttack()
    {
        if (battleEnded) return;

        Debug.Log("PlayerAttack called");
        StartCoroutine(PlayerAttackRoutine());
    }

    float EaseOutQuad(float t)
    {
        return t * (2f - t);
    }

    IEnumerator PlayerAttackRoutine()
    {
        if (battleEnded) yield break;

        playerTurn = false;
        UIManager.Instance.HideBattleUI();

        Vector3 startPos = player.transform.position;
        Vector3 attackPos = enemy.transform.position + new Vector3(-1f, 0, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPos, attackPos, EaseOutQuad(t));
            yield return null;
        }

        player.SetSprite(hitSpritePlayer);
        yield return new WaitForSeconds(0.2f);

        enemy.TakeDamage(20);

        // ✅ WIN CHECK
        if (enemy.currentHP <= 0 && !battleEnded)
        {
            battleEnded = true;

            SaveLastNode.Save(winNodeID);
            SceneManager.LoadScene(sceneNameAfterBattle);
            yield break;
        }

        CameraShake.Instance.Shake(0.15f, 0.2f);
        StartCoroutine(Shake(enemy.transform));

        player.currentMana = Mathf.Min(player.currentMana + 2, player.maxMana);
        UIManager.Instance.RefreshUI();

        yield return new WaitForSeconds(0.3f);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            player.transform.position = Vector3.Lerp(attackPos, startPos, t);
            yield return null;
        }

        player.SetSprite(idleSpritePlayer);
        EndPlayerTurn();
    }

    IEnumerator EnemyAttackRoutine()
    {
        if (battleEnded) yield break;

        UIManager.Instance.HideBattleUI();

        Vector3 startPos = enemy.transform.position;
        Vector3 attackPos = player.transform.position + new Vector3(1f, 0, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            enemy.transform.position = Vector3.Lerp(startPos, attackPos, EaseOutQuad(t));
            yield return null;
        }

        canDodge = true;
        dodged = false;

        Debug.Log("DODGE NOW!");
        enemy.SetSprite(hitSprite);

        yield return new WaitForSeconds(0.5f);

        canDodge = false;

        if (dodged)
        {
            yield return StartCoroutine(DodgeMove(player.transform));
            CameraShake.Instance.Shake(0.1f, 0.1f);
        }
        else
        {
            player.TakeDamage(15);

            // ❌ LOSE CHECK
            if (player.currentHP <= 0 && !battleEnded)
            {
                battleEnded = true;

                SaveLastNode.Save(loseNodeID);
                SceneManager.LoadScene(sceneNameAfterBattle);
                yield break;
            }

            CameraShake.Instance.Shake(0.15f, 0.15f);
            StartCoroutine(Shake(player.transform));
        }

        yield return new WaitForSeconds(0.3f);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            enemy.transform.position = Vector3.Lerp(attackPos, startPos, t);
            yield return null;
        }

        enemy.SetSprite(idleSpriteEnemy);
        UIManager.Instance.ShowBattleUI();

        StartPlayerTurn();
    }

    public IEnumerator Shake(Transform target)
    {
        Vector3 originalPos = target.position;

        float duration = 0.2f;
        float magnitude = 0.2f;

        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            target.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = originalPos;
    }

    IEnumerator DodgeMove(Transform target)
    {
        player.SetSprite(dodgeSprite);

        Vector3 startPos = target.position;
        Vector3 dodgePos = startPos + new Vector3(-2f, -1f, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            target.position = Vector3.Lerp(startPos, dodgePos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 4f;
            target.position = Vector3.Lerp(dodgePos, startPos, t);
            yield return null;
        }

        player.SetSprite(idleSpritePlayer);
    }

    private void Update()
    {
        if (battleEnded) return;

        if (canDodge && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            dodged = true;
            Debug.Log("DODGE!");
        }
    }
}