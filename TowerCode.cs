using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCode : MonoBehaviour
{
    public GameObject shot;
    public int hp = 5;
    public float turnSpeed = 5f;
    public float shootRange = 20;
    public float shootCooldown = 0.5f;
    float cooldownLeft = 0;
    public string color;
    public float shotOffset;
    public Color col;

    
    public void teamDie(Color deadTeamCol)
    {
        if (this.col == deadTeamCol)
        {
            Destroy(gameObject);
        }
    }

    public void setColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = col;
    }

    EnemyCode[] GetEnemyCode()
    {
        Object[] g = FindObjectsOfType(typeof(EnemyCode));
        EnemyCode[] t = new EnemyCode[g.Length];
        for (int h = 0; h < g.Length; h++)
        {
            EnemyCode temp = (EnemyCode)g[h];
            if (!temp.color.Equals(this.color))
            {
                t[h] = temp;
            }
            else
            {
                t[h] = null;
            }
        }
        return t;
    }

    EnemyCode getNextTarget()
    {

        EnemyCode[] t = GetEnemyCode();
        EnemyCode nextTarget = null;
        float dist = Mathf.Infinity;
        for (int h = 0; h < t.Length; h++)
        {
            if (t[h] != null)
            {
                float d = Vector3.Distance(this.transform.position, t[h].transform.position);

                if (nextTarget == null || d < dist)
                {
                    nextTarget = t[h];
                    dist = d;
                }
            }
        }
        return nextTarget;
    }

    void pointTo(EnemyCode nextTarget)
    {
        Vector2 v = nextTarget.transform.position - this.transform.position;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    void shoot()
    {
        if (cooldownLeft <= 0)
        {
            Vector3 vec = transform.rotation * new Vector3(0, shotOffset, 0);
            Bullet b = shot.GetComponent<Bullet>();
            b.color = color;
            Instantiate(shot, transform.position + vec, transform.rotation);
            cooldownLeft = shootCooldown;
        }
        else
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    void checkForDeath()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        EnemyCode nextTarget = getNextTarget();

        if (nextTarget != null)
        {
            pointTo(nextTarget);
            shoot();
        }

        checkForDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.GetComponent<Bullet>().color == color))
        {
            hp--;
        }
    }

}
