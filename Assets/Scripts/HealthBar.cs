using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public bool isMonster;

    private int currentHealth;
    private int maxHealth;

    private void OnEnable()
    {
        if (!isMonster) {
          Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
          currentHealth = p.health;
          maxHealth = p.maxHealth;
        }
        else {
          foreach(GameObject monster in GameObject.FindGameObjectsWithTag("monster")) {
            if (monster.GetComponent<SpriteRenderer>().isVisible) {
              Monster m = monster.GetComponent<Monster>();
              currentHealth = m.health;
              maxHealth = m.maxHealth;
            }
          }
        }
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void SetHealth(int hp)
    {
        healthBar.value+=hp;
    }
}
