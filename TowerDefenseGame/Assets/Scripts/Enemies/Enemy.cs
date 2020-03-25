using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Inspector variables
    [SerializeField] private int health = 100;
    [SerializeField] private int defense = 0;
    #endregion

    #region Properties
    public int Defense { set { defense = value; } }

    #endregion

    #region MonoBehavior Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public void TakeDamage(int damage)
    {
        // 10 * 10 / (10 - 5) = 20
        // 10 
        var actualDamage = damage * damage / (damage - defense);

        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
