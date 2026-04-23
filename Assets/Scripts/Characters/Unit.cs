using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP = 100;
    public int currentHP;

    public HPBar hpBar;



    private void Start()
    {
        currentHP = maxHP;

        if (hpBar != null)
            hpBar.SetMaxHP(maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (hpBar != null)
            hpBar.SetHP(currentHP);

        Debug.Log(unitName + " took " + damage + " damage. HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(unitName + " is dead!");

        if (unitName == "Player")
        {
            Debug.Log("GAME OVER");
        }
        else
        {
            Debug.Log("YOU WIN!");
        }

        gameObject.SetActive(false);
    }
}