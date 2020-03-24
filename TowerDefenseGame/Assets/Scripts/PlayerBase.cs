using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int health = 100;
    [SerializeField] protected Text healthText;

    public int Health => health;
    public int Defense = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (healthText == null)
        {
            throw new ArgumentNullException(nameof(healthText));
        }
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageValue)
    {
        health = health - (damageValue - Defense);
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = $"{health} / {maxHealth}";
    }
}