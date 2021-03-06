﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayableChar : MonoBehaviour
{
    // Character Attributes
    public int healthPoints;
    public int SP;
    public float movSpeed;
    public float dashLength;
    public Text score;
    public Slider SPSlider;
    public Slider hpSlider;
    private int point;
    private SpriteRenderer charRenderer;
    private bool isInvincible;

    protected Melee melWeapon;
    protected Guns rangWeapon;
    public float fireRate;
    protected float nextShot;
    protected Animator myAnimator;
    public int damage;
    public int meleeDamage;
    Transform viseur;
    public bool falling;
    public int PlayerIdNumber;

    public Collider2D regularCollider;
    public Collider2D flippedCollider;
    public Collider2D RegularFeet;
    public Collider2D flippedFeet;

    public float bulletSpeed;

    private int maxHealth;
    protected int meleeAttack;

    //enum Controls {Attack = "R2" ,Melee = "R1" ,SpecialAttack = "L1" };

    // Useful Functions
    public virtual void Awake()
    {
        if (PlayerIdNumber == 0)
        {
            Destroy(this.gameObject);
            return;
        }
        score = GameObject.Find("Score" + PlayerIdNumber).GetComponent<Text>();
        hpSlider = GameObject.Find("Player" + PlayerIdNumber).GetComponent<Slider>();
        SPSlider = GameObject.Find("SpecialBar" + PlayerIdNumber).GetComponent<Slider>();
        charRenderer = GetComponent<SpriteRenderer>();
        point = 0;
        SP = 0;
        score.text = "Score  :  " + point;
        maxHealth = healthPoints;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
        SPSlider.maxValue = 100;
        SPSlider.value = 0;

        myAnimator = GetComponent<Animator>();
        meleeAttack = 0;
    }

    public void Start()
    {
        transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = ColorSystem.mainColors[PlayerIdNumber];
        GetComponentInChildren<ParticleSystem>().startColor = ColorSystem.mainColors[PlayerIdNumber];
    }
    protected virtual void SpecialAttack(){ }
    protected virtual void RangeAttack() { }
    protected virtual void MeleeAttack() { }
    protected virtual void Dash() { }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player in collision with " + other.name);
        Bullet bulletScript;
        float KnockBackRate = 0f;
        if (bulletScript = other.GetComponent<Bullet>())
        {
            KnockBackRate = other.GetComponent<Bullet>().damage / 5f;
        }

        Vector3 bulletVelocity = other.GetComponent<Rigidbody2D>().velocity;
        
        if(other.tag == "PlayerBullet")
        {
            Debug.Log(this.name+" vs "+other.gameObject.GetComponent<Bullet>().Creator.name);
            if(this.name != other.gameObject.GetComponent<Bullet>().Creator.name)
            {
                Debug.Log("The tag is actually PlayerBullet");
                transform.position += new Vector3(bulletVelocity.x * KnockBackRate * Time.deltaTime, bulletVelocity.y * KnockBackRate * Time.deltaTime, 0.0f);
                Destroy(other.gameObject);
            }
            
        }
    }

    public void Update()
    {
        hpSlider.value = Mathf.MoveTowards(hpSlider.value, healthPoints, 2f);
        SPSlider.value = Mathf.MoveTowards(SPSlider.value, SP, 2f);
        if (SP == 100)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            SP_Blinker.StartBlinking(PlayerIdNumber - 1);
        }
        if (PlayerIdNumber == 0)
        {
            PlayerIdNumber = 1;
        }
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
            flippedCollider.gameObject.SetActive(true);
            regularCollider.gameObject.SetActive(false);
            flippedFeet.gameObject.SetActive(true);
            RegularFeet.gameObject.SetActive(false);
        }
        else if(xRight < -0.2)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            flippedCollider.gameObject.SetActive(false);
            regularCollider.gameObject.SetActive(true);
            flippedFeet.gameObject.SetActive(false);
            RegularFeet.gameObject.SetActive(true);
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

        if(Input.GetAxis("L2Player"+PlayerIdNumber)>0.8 && SP == 100)
        {
            UseSpecial();
            SP = 0;
        }


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
        GetComponentInChildren<ParticleSystem>().Stop();
        SP_Blinker.EndBlinking(PlayerIdNumber - 1);
    }

    public void SetHp(int value)
    {
        if (healthPoints + value > maxHealth)
        {
            healthPoints = maxHealth;
            hpSlider.value = hpSlider.maxValue;
        } else
        {
            if((!isInvincible && value < 0) || value > 0)
            healthPoints += value;
            if (value < 0)
            {
                isInvincible = true;
                StartCoroutine(FlashDamage(-value));
            }
            if (healthPoints <= 0)
            {
                GetComponent<FadeOut>().enabled = true;
                Destroy(this);
            }
        }
    }

    IEnumerator FlashDamage(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if(i%2 == 0)
            {
                charRenderer.color = Color.red;
            }
            else
            {
                charRenderer.color = Color.white;
            }
            yield return new WaitForSeconds(0.05f);
        }
        isInvincible = false;
        charRenderer.color = Color.white;
    }

    public void UpdatePoint(int pts)
    {
        Debug.Log(pts);
        this.point += pts;
        SP += pts;
        if(SP > 100)
        {
            SP = 100;
        }
        score.text = "Score  :  " + point;
    }

}
