using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int health = 100;
    [SerializeField] protected GameObject healthText;

    public int Health => health;
    public int Defense = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (healthText == null)
        {
            throw new ArgumentNullException(nameof(healthText));
        }
        var meshRenderererererer = healthText.GetComponent<MeshRenderer>();
        meshRenderererererer.sortingOrder = 100;

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
        var textMesh = healthText.GetComponent<TextMesh>();
        textMesh.text = $"{health} / {maxHealth}";
    }
}