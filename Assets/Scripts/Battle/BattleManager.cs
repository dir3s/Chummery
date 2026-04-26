using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;


    public bool playerTurn = true;

    private bool canDodge = false;
    private bool dodged = false;

    public Unit player;
    public Unit enemy;

    private Vector3 playerStartPos;
    private Vector3 enemyStartPos;

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
        playerTurn = true;
        Debug.Log("Player Turn Started");
    }

    public void EndPlayerTurn()
    {
        playerTurn = false;
        Debug.Log("Enemy Turn Started");

        EnemyTurn();
    }

    void EnemyTurn()
    {
        Debug.Log("Enemy attacks!");
        StartCoroutine(EnemyAttackRoutine());
    }

    public void PlayerAttack()
    {
        Debug.Log("PlayerAttack called");
        StartCoroutine(PlayerAttackRoutine());
    }

    float EaseOutQuad(float t)
    {
        return t * (2f - t);
    }

    IEnumerator PlayerAttackRoutine()
    {
        playerTurn = false;
        UIManager.Instance.HideBattleUI();
        Vector3 startPos = player.transform.position;
        Vector3 attackPos = enemy.transform.position + new Vector3(-1f, 0, 0);

        float t = 0;


        while (t < 1)
        {
            t += Time.deltaTime;
            float eased = EaseOutQuad(t);

            player.transform.position =
                Vector3.Lerp(startPos, attackPos, eased);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);


        enemy.TakeDamage(20);

        CameraShake.Instance.Shake(0.15f, 0.2f);
        StartCoroutine(Shake(enemy.transform));


        player.currentMana += 2;
        player.currentMana = Mathf.Min(player.currentMana, player.maxMana);
        UIManager.Instance.RefreshUI();
        yield return new WaitForSeconds(0.3f);

        


        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;

            player.transform.position =
                Vector3.Lerp(attackPos, startPos, t);

            yield return null;
        }

        EndPlayerTurn();
    }

    IEnumerator EnemyAttackRoutine()
    {
        UIManager.Instance.HideBattleUI();

        playerTurn = false;

        Vector3 startPos = enemy.transform.position;
        Vector3 attackPos = player.transform.position + new Vector3(1f, 0, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            float eased = EaseOutQuad(t);

            enemy.transform.position =
                Vector3.Lerp(startPos, attackPos, eased);

            yield return null;
        }


        canDodge = true;
        dodged = false;

        Debug.Log("DODGE NOW!");

        yield return new WaitForSeconds(0.5f);

        canDodge = false;


        if (dodged)
        {
            Debug.Log("Player dodged!");

 
            yield return StartCoroutine(DodgeMove(player.transform));


            CameraShake.Instance.Shake(0.1f, 0.1f);
        }
        else
        {
            int damage = 15;
            player.TakeDamage(damage);

            CameraShake.Instance.Shake(0.15f, 0.15f);
            StartCoroutine(Shake(player.transform));
        }

        yield return new WaitForSeconds(0.3f);


        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;

            enemy.transform.position =
                Vector3.Lerp(attackPos, startPos, t);

            yield return null;
        }

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
    }

    private void Update()
    {
        if (canDodge && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            dodged = true;
            Debug.Log("DODGE!");
        }
    }
}