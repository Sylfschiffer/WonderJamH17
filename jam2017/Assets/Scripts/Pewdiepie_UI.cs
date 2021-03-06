﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum SelectedAttack
{
    Soldier = 0,
    Robot = 1,
    Juggernaut = 2,
    Sniper = 3,
    Missile = 4
}

public enum SelectedSpawnPosition
{
    Left = 1,
    Right = 3,
    Bottom = 2
}

public class Pewdiepie_UI : MonoBehaviour {

    public GameObject[] unit_icons;
    public Text myMoney;

    public float[] cooldownValue;
    private float[] cooldownCounter = { 1f, 1f, 1f, 1f, 1f };
    //private bool[] attackAllowed = { true, true, true, true, true, };

    private long money;
    private bool raiseMoney;
    private SelectedAttack selectedAttack;
    private bool isFlipped = false;
    bool nukeActivated;

	// Use this for initialization
	void Start () {
        selectedAttack = SelectedAttack.Soldier;
        money = 1250;
        raiseMoney = true;
        StartCoroutine(AddMoney());
    }
	
    IEnumerator AddMoney()
    {
        if (!nukeActivated)
        {
            int moneyFactor = 10;

            while (raiseMoney)
            {
                yield return new WaitForSeconds(1f);
                if (Hack.beingHacked)
                    continue;
                money += moneyFactor;
                moneyFactor += 2;
            }
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
        // Je m'excuse

        #region SerieDeIf1
            if (Hack.beingHacked)
            {
                money = 0;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedAttack = SelectedAttack.Soldier;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedAttack = SelectedAttack.Robot;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedAttack = SelectedAttack.Juggernaut;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectedAttack = SelectedAttack.Sniper;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selectedAttack = SelectedAttack.Missile;
            }
            if(Input.GetKeyDown(KeyCode.Space) && nukeActivated)
            {
                GetComponent<AudioSource>().Play();
                StartCoroutine(Nuke());
            }
        #endregion

        #region cooldownRestoreRegion
        LoadCooldownUnderOne((int)SelectedAttack.Soldier);
        LoadCooldownUnderOne((int)SelectedAttack.Robot);
        LoadCooldownUnderOne((int)SelectedAttack.Juggernaut);
        LoadCooldownUnderOne((int)SelectedAttack.Sniper);
        LoadCooldownUnderOne((int)SelectedAttack.Missile);
        #endregion

        foreach (GameObject g in unit_icons)
            g.GetComponent<Image>().color = Color.white;
        unit_icons[(int)selectedAttack].GetComponent<Image>().color = Color.yellow;

        if (!nukeActivated)
        {
            myMoney.text = money + " $";
        }

        #region SerieDeIf2
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isFlipped = true;
            Vector2 position = GameObject.Find("LeftSpawnPosition").transform.position;
            SpawnEnemy(position, SelectedSpawnPosition.Left);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2 position = GameObject.Find("RightSpawnPosition").transform.position;
            SpawnEnemy(position, SelectedSpawnPosition.Right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2 position = GameObject.Find("BottomSpawnPosition").transform.position;
            SpawnEnemy(position, SelectedSpawnPosition.Bottom);
        }
        #endregion
    }

    private void LoadCooldownUnderOne(int unit)
    {
        if(cooldownCounter[unit]<1)
        {
            cooldownCounter[unit] += Time.deltaTime * cooldownValue[unit];
            unit_icons[unit].GetComponent<Image>().fillAmount = cooldownCounter[unit];
        }
    }

    private void ActivateCooldown(int unit)
    {
        cooldownCounter[unit] = 0;
        unit_icons[unit].GetComponent<Image>().fillAmount = cooldownCounter[unit];

    }
    private void SpawnEnemy(Vector2 spawnPosition, SelectedSpawnPosition ssp)
    {
        if (Dialogue_pewdiepie.isActive)
            return;

        GameObject spawnedAttack;

        switch (selectedAttack)
        {
            case SelectedAttack.Soldier:
                if (money >= 150 && cooldownCounter[(int)SelectedAttack.Soldier] >= 1f)
                {
                        spawnedAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Soldier"));
                        spawnedAttack.transform.position = spawnPosition;
                        spawnedAttack.GetComponent<MovableEnemy>().type = TypeMoveableEnemy.Soldier;
                        spawnedAttack.GetComponent<SpriteRenderer>().flipX = isFlipped;
                        money -= spawnedAttack.GetComponent<MovableEnemy>().spawnCost;
                        ActivateCooldown((int)SelectedAttack.Soldier);
                }
                else
                {
                    //Gerer feedback
                }
                break;

            case SelectedAttack.Robot:
                if (money >= 350 && cooldownCounter[(int)SelectedAttack.Robot] >= 1f)
                {
                    spawnedAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Robot"));
                    spawnedAttack.transform.position = spawnPosition;
                    spawnedAttack.GetComponent<MovableEnemy>().type = TypeMoveableEnemy.Robot;
                    money -= spawnedAttack.GetComponent<MovableEnemy>().spawnCost;
                    ActivateCooldown((int)SelectedAttack.Robot);
                }
                else
                {
                    //Gerer feedback
                }

                break;

            case SelectedAttack.Juggernaut:
                if (money >= 500 && cooldownCounter[(int)SelectedAttack.Juggernaut] >= 1f)
                {
                    spawnedAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Juggernaut"));
                    spawnedAttack.transform.position = spawnPosition;
                    spawnedAttack.GetComponent<MovableEnemy>().SetSelectedSpawnPosition(ssp);
                    money -= spawnedAttack.GetComponent<MovableEnemy>().spawnCost;
                    ActivateCooldown((int)SelectedAttack.Juggernaut);
                }
                else
                {
                    //Gerer feedback
                }
                
                break;

            case SelectedAttack.Sniper:
                if (money >= 750 && cooldownCounter[(int)SelectedAttack.Sniper] >= 1f)
                {
                    spawnedAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Sniper"));
                    spawnedAttack.transform.position = spawnPosition;
                    spawnedAttack.GetComponent<MovableEnemy>().SetSelectedSpawnPosition(ssp);
                    spawnedAttack.GetComponent<SpriteRenderer>().flipX = isFlipped;
                    money -= spawnedAttack.GetComponent<MovableEnemy>().spawnCost;
                    ActivateCooldown((int)SelectedAttack.Sniper);
                }
                else
                {
                    //Gerer feedback
                }

                break;

            case SelectedAttack.Missile:
                if (money >= 1000 && cooldownCounter[(int)SelectedAttack.Missile] >= 1f)
                {
                    GetComponentInParent<Missile>().SelectLocation((int)ssp);
                    GetComponentInParent<Missile>().missileIsTriggered = true;
                    money -= 1000;
                    ActivateCooldown((int)SelectedAttack.Missile);
                }
                else
                {
                    //Gerer feedback
                }
                
                break;
        }
    }

    public void ActivateNuke()
    {
        nukeActivated = true;
        money = 0;
    }

    IEnumerator Nuke()
    {
        Image nuke_flash = GameObject.Find("Flash").GetComponent<Image>();
        for(int i = 0; i < 300; i++)
        {
            yield return new WaitForSeconds(0.005f);
            Camera.main.orthographicSize *= 0.998f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.Range(-0.1f, 0.1f), Camera.main.transform.position.y + Random.Range(-0.1f, 0.1f), Camera.main.transform.position.z);
            nuke_flash.color = new Color(nuke_flash.color.r, nuke_flash.color.g, nuke_flash.color.b, i * 0.005f);
        }
        SceneManager.LoadScene("End_Dictator");
    }
}
