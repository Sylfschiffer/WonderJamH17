﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableAnonymous : PlayableChar {

    Transform viseur;
    public static int ID;
    // Use this for initialization
    public override void Start()
    {
        melWeapon = new Katana();
        rangWeapon = new Uzi();
        viseur = transform.GetChild(0);
        nextShot = 0;
        PlayerIdNumber = ID;
        base.Start();
    }

    protected override void UseSpecial()
    {
        // Effects of the special attack
    }

    protected override void RangeAttack()
    {
        if (nextShot == 0)
        {
            nextShot = 0.08f;
            Vector2 direction = viseur.position - transform.position;
            direction.Normalize();
            Debug.Log(direction.x + " " + direction.y);
            GameObject bullet = (GameObject)Instantiate(Resources.Load("Prefabs/UziBullet"));
            bullet.transform.localScale /= 2;

            Transform bulletSpawn = transform;
            if(viseur.transform.position.x - gameObject.transform.position.x > 0)
            {
                bulletSpawn.position = new Vector3(bulletSpawn.position.x + 140, bulletSpawn.position.y, 0);
            }else if(viseur.transform.position.x - gameObject.transform.position.x < 0)
            {
                bulletSpawn.position = new Vector3(bulletSpawn.position.x - 140, bulletSpawn.position.y, 0);
            }

            //bullet.transform.position = bulletSpawn.position;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 12;
            bullet.GetComponent<Bullet>().damage = this.damage;
            bullet.GetComponent<SpriteRenderer>().color = ColorSystem.mainColors[PlayerIdNumber];
        }
        if(nextShot > 0)
        {
            nextShot -= Time.deltaTime;
        }
        if(nextShot < 0)
        {
            nextShot = 0;
        }
    }

    public static void UpdateID(int id)
    {
        ID = id;
    }


    protected override void MeleeAttack()
    {
        meleeAttack = 60;
        int direction = -1;
        if (GetComponent<SpriteRenderer>().flipX)
            direction = 1;
        GameObject hitBox = (GameObject)Instantiate(Resources.Load("Prefabs/SlashKatana"), new Vector2(transform.position.x + direction * 0.3f, transform.position.y - 0.25f), Quaternion.identity);
        hitBox.GetComponent<Bullet>().damage = meleeDamage;
        hitBox.GetComponent<Bullet>().timeToLive = 1f;
    }
}
