using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{

    public float speed = 3f;
    float buildCoolDown = 0f;
    public float BuildCoolDown = 10;
    float shootCooldown = 0;
    public float ShootCooldown = 0.5f;
    public float turnSensitivity = -2f;
    public GameObject tower;
    public GameObject shot;
    public int hp = 10;
    public float shotOffset;
    public string color;
    Color32 c;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        getColor();
    }

    void getColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        c = new Color32();
        int randNum = (Random.Range(0, 255) / 50) * 50;

        switch (Random.Range(1, 6))
        {
            case 1: c.r = (byte)randNum; c.b = 0xFF; c.g = 0x0; c.a = 0xFF; break;
            case 2: c.r = 0x0; c.b = (byte)randNum; c.g = 0xFF; c.a = 0xFF; break;
            case 3: c.r = 0xFF; c.b = 0x0; c.g = (byte)randNum; c.a = 0xFF; break;
            case 4: c.r = (byte)randNum; c.b = 0x0; c.g = 0xFF; c.a = 0xFF; break;
            case 5: c.r = 0xFF; c.b = (byte)randNum; c.g = 0x0; c.a = 0xFF; break;
            case 6: c.r = 0x0; c.b = 0xFF; c.g = (byte)randNum; c.a = 0xFF; break;
        }
        sr.color = c;
        color = c.ToString();
    }

    void move()
    {
        if (Input.GetButton("Vertical"))
        {
            rb.AddForce(transform.up * speed * Input.GetAxis("Vertical"));
        }
        rb.AddTorque(Input.GetAxis("Horizontal") * turnSensitivity);
    }

    void shoot()
    {
        if (Input.GetButton("Fire1") && shootCooldown <= 0)
        {
            Vector3 vec = transform.rotation * new Vector3(0, shotOffset, 0);
            Bullet b = shot.GetComponent<Bullet>();
            b.color = color;
            Instantiate(shot, transform.position + vec, transform.rotation);
            shootCooldown = ShootCooldown;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    void placeTower()
    {
        if (Input.GetButton("Fire3") && buildCoolDown <= 0)
        {
            Transform t = GetComponent<Transform>();
            float x = rb.transform.position.x;
            float y = rb.transform.position.y;
            t.transform.position.Set(x, y, 0);

            TowerCode tc = tower.GetComponent<TowerCode>();
            tc.color = color;
            tc.col = c;
            tc.setColor();
            Instantiate(tower, this.transform.position, this.transform.rotation);
            buildCoolDown = BuildCoolDown;
        }
        else
        {
            buildCoolDown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        move();

        shoot();

        placeTower();

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.GetComponent<Bullet>().color == color))
        {
            hp--;
        }
    }

}
