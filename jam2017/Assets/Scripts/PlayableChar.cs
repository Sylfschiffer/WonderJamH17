﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayableChar : MonoBehaviour
{
    // Character Attributes
    public int healthPoints;
    public float movSpeed;
    public float dashLength;

    protected Melee melWeapon;
    protected Guns rangWeapon;
    protected float fireRate;
    protected float nextShot;
    protected Animator myAnimator;
    public int damage;
    public int meleeDamage;
    Transform viseur;
    public bool falling;
    public int PlayerIdNumber;
    
    private int maxHealth;
    protected int meleeAttack;

    //enum Controls {Attack = "R2" ,Melee = "R1" ,SpecialAttack = "L1" };

    // Useful Functions
    public virtual void Start()
    {
        maxHealth = healthPoints;
        myAnimator = GetComponent<Animator>();
        meleeAttack = 0;
        PlayerIdNumber = 1; //TODELETE
        if(PlayerIdNumber == 0)
        {
            Destroy(this.gameObject);
        }
    }
    protected virtual void SpecialAttack(){ }
    protected virtual void RangeAttack() { }
    protected virtual void MeleeAttack() { }
    protected virtual void Dash() { }
    public void Update()
    {
        if (meleeAttack > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            meleeAttack--;
            return;
        }
        viseur = transform.GetChild(0);
        float xRight = Input.GetAxis("RightAxisXPlayer"+PlayerIdNumber);
        float yRight = Input.GetAxis("RightAxisYPlayer"+PlayerIdNumber) *-1;
        if (xRight != 0 || yRight != 0)
        {
            viseur.position = (new Vector2(xRight, yRight)).normalized;
            viseur.localPosition = Vector2.ClampMagnitude(viseur.position, 2);
        }
        
        if(xRight > 0.2)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(xRight < -0.2)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetButtonDown("R1Player"+PlayerIdNumber))
        {
            myAnimator.SetTrigger("meleeAttack");
            myAnimator.SetBool("isWalking", false);
            MeleeAttack();
            return;
        }
        if(Input.GetAxis("R2Player"+PlayerIdNumber)<-0.8)
        {
            myAnimator.SetBool("isShooting", true);
            RangeAttack();
        }
        else myAnimator.SetBool("isShooting", false);

        float xLeft = Input.GetAxis("LeftAxisXPlayer" + PlayerIdNumber);
        float yLeft = Input.GetAxis("LeftAxisYPlayer" + PlayerIdNumber) * -1;
        Vector2 velocity = new Vector2(xLeft, yLeft);

        if (xLeft <0)
        {
            //this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
            
            myAnimator.SetBool("isWalking", true);
        }
        else if(xLeft > 0)
        {
            myAnimator.SetBool("isWalking", true);
            //this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        else  myAnimator.SetBool("isWalking", false);

        if (falling)
        {
            if (xLeft == 0 && yLeft == 0)
            {

            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = (velocity * movSpeed) / 3;
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = velocity * movSpeed;
        }

    }

    protected virtual void UseSpecial()
    {
        // Effects of the special attack
    }

    public void SetHp(int hp)
    {
        if (healthPoints + hp > maxHealth)
        {
            healthPoints = maxHealth;
        } else
        {
            Debug.Log("can you plz" + hp);
            healthPoints += hp;
        }
    }

}
