using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    private const string HealthStringStart = "";

    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int health = 100;
    [SerializeField] protected int defense = 0;
    [SerializeField] protected Text healthText;

    public int Health => health;
    public int Defense => defense;

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
        health = health - damageValue - defense;
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = $"{health} / {maxHealth}";
    }
}