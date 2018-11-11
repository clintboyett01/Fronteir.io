using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCode : MonoBehaviour
{
    public GameObject tower;
    public GameObject shot;
    public float turnSpeed = 2;
    public float shootCooldown = 3;
    private float cooldownLeft = 0;
    public float shootRange = 10;
    public float toCloseRange = 2;
    public float speed;
    public string color;
    public float shotOffset;
    public int hp = 10;
    public float towerPlaceCooldown = 10;
    float towerCooldown = 0;
    Color32 c;
    TowerCode[] myTeam;

    void Start()
    {
        setColor();
    }

    void setColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        c = getRandomColor();
        sr.color = c;
        color = c.ToString();
    }

    Color32 getRandomColor()
    {
        Color32 temp = new Color32();
        int  randNum = (Random.Range(0, 255)/50)*50;
        
        switch (Random.Range(1, 6))
        {
            case 1: temp.r = (byte)randNum; temp.b = 0xFF; temp.g = 0x0; temp.a = 0xFF; break;
            case 2: temp.r = 0x0; temp.b = (byte)randNum; temp.g = 0xFF; temp.a = 0xFF; break;
            case 3: temp.r = 0xFF; temp.b = 0x0; temp.g = (byte)randNum; temp.a = 0xFF; break;
            case 4: temp.r = (byte)randNum; temp.b = 0x0; temp.g = 0xFF; temp.a = 0xFF; break;
            case 5: temp.r = 0xFF; temp.b = (byte)randNum; temp.g = 0x0; temp.a = 0xFF; break;
            case 6: temp.r = 0x0; temp.b = 0xFF; temp.g = (byte)randNum; temp.a = 0xFF; break;
        }

        Object[] g = FindObjectsOfType(typeof(EnemyCode));
        Object[] g2 = FindObjectsOfType(typeof(Controler));
        for (int c = 0; c < g.Length; c++)
        {
            EnemyCode e = (EnemyCode)g[c];
            if (temp.ToString().Equals(e.color))
            {

                return getRandomColor();
            }
        }
        for (int c = 0; c < g2.Length; c++)
        {
            Controler e = (Controler)g2[c];
            if (temp.ToString().Equals(e.color))
            {
                return getRandomColor();
            }
        }

        return temp;

    }
    
    TowerCode[] GetTowerCode()
    {
        Object[] g = FindObjectsOfType(typeof(TowerCode));
        TowerCode[] t = new TowerCode[g.Length];
        for (int h = 0; h < g.Length; h++)
        {
            TowerCode temp = (TowerCode)g[h];
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

    TowerCode getNextTarget()
    {
        TowerCode[] t = GetTowerCode();
        TowerCode nextTarget = null;
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

    void MoveTo(TowerCode nextTarget)
    {
        Vector2 v = nextTarget.transform.position - this.transform.position;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextTarget.transform.position) < toCloseRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextTarget.transform.position, -speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, nextTarget.transform.position) > shootRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextTarget.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = this.transform.position;
        }
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

    void buildTower()
    {
        if (towerCooldown <= 0)
        {

            Transform tra = GetComponent<Transform>();
            float x = transform.position.x;
            float y = transform.position.y;
            tra.transform.position.Set(x, y, 0);

            TowerCode tc = tower.GetComponent<TowerCode>();
            tc.color = color;
            tc.col = c;
            tc.setColor();
            Instantiate(tower, this.transform.position, this.transform.rotation);
            towerCooldown = towerPlaceCooldown;
        }
        else
        {
            towerCooldown -= Time.deltaTime;
        }
    }

    void checkForDeath()
    {
        if (hp <= 0)
        {
            killTeam();
            Destroy(gameObject);
        }
    }

    void killTeam()
    {
        Object[] g = FindObjectsOfType(typeof(TowerCode));
        for (int h = 0; h < g.Length; h++)
        {
            TowerCode temp = (TowerCode)g[h];
            temp.teamDie(c);
        }
    }

    void FixedUpdate()
    {
        TowerCode nextTarget = getNextTarget();
        if (!(nextTarget == null))
        {
            MoveTo(nextTarget);
            shoot();
        }
        buildTower();
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
