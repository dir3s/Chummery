using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool playerTurn = true;

    public Unit player;
    public Unit enemy;


    public Transform playerTransform;
    public Transform enemyTransform;

    private Vector3 playerStartPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerStartPos = playerTransform.position;

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















    IEnumerator PlayerAttackRoutine()
    {
        playerTurn = false;

        Vector3 attackPos = enemyTransform.position + new Vector3(-1f, 0, 0);


        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            playerTransform.position = Vector3.Lerp(playerStartPos, attackPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        int damage = 20;
        enemy.TakeDamage(damage);
        StartCoroutine(Shake(enemyTransform));

        yield return new WaitForSeconds(0.3f);


        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            playerTransform.position = Vector3.Lerp(attackPos, playerStartPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        EndPlayerTurn();
    }




    IEnumerator EnemyAttackRoutine()
    {
        playerTurn = false;

        Vector3 startPos = enemyTransform.position;
        Vector3 attackPos = playerTransform.position + new Vector3(1f, 0, 0);

        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            enemyTransform.position = Vector3.Lerp(startPos, attackPos, t);
            yield return null;
        }

       
        yield return new WaitForSeconds(0.2f);

        int damage = 15;
        player.TakeDamage(damage);

        StartCoroutine(Shake(playerTransform));

        yield return new WaitForSeconds(0.3f);

        
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            enemyTransform.position = Vector3.Lerp(attackPos, startPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        StartPlayerTurn();
    }








    IEnumerator Shake(Transform target)
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

}