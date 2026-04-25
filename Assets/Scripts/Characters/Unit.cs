using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP = 100;
    public int currentHP;

    public int maxMana = 10;
    public int currentMana;

    public HPBar hpBar;

    public Sprite idleSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();



        currentHP = maxHP;
        currentMana = maxMana;

        if (hpBar != null)
            hpBar.SetMaxHP(maxHP);
    }

    public void SetSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
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