using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float damage = 1f;
    float bulletLife = 3;
    public string color;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        rb.AddForce(transform.up * bulletSpeed);
        bulletLifeTracker();
    }

    void bulletLifeTracker()
    {
        if (bulletLife <= 0)
        {
            Destroy(gameObject);
        }
        bulletLife -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (!(collision.gameObject.GetComponent<Bullet>().color == color))
            {
                Destroy(gameObject);

            }
        }
        catch (Exception e) { }
        try
        {
            if (!(collision.gameObject.GetComponent<TowerCode>().color == color))
            {
                Destroy(gameObject);

            }
        }
        catch (Exception e) { }
        try
        {
            if (!(collision.gameObject.GetComponent<EnemyCode>().color == color))
            {
                Destroy(gameObject);

            }
        }
        catch (Exception e) { }
        try
        {
            if (!(collision.gameObject.GetComponent<Controler>().color == color))
            {
                Destroy(gameObject);

            }
        }
        catch (Exception e) { }
    }

}
