﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using noissimEnvironment.LobbyScene;
using System;

public class PlayerLobbyController : MonoBehaviour
{
    [SerializeField]
    private GameObject alertText;
    [SerializeField]
    private GameObject NPCName;

    [Space(10)]
    [Header("Player Data")]
    [SerializeField]
    private TextMeshProUGUI playerLevel;
    [SerializeField]
    private TextMeshProUGUI playerEXPProgess;
    [SerializeField]
    private TextMeshProUGUI playerRuby;

    protected CharacterLevel characterLvl;
    protected ProgressSerial progressSerial;
    [SerializeField]
    protected GameObject uiControllerGameObject;
    private UIController uiController;

    private void Start()
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);

        characterLvl = new CharacterLevel();
        progressSerial = ProgressSerial.getInstance();
    }

    private void Awake()
    {
        uiController = uiControllerGameObject.GetComponent<UIController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DungeonEntrance")
        {
            alertText.SetActive(true);
        }
        else if (collision.gameObject.tag == "NPC")
        {
            NPCName.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DungeonEntrance")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameplayCounter.getInstance().restartCounter();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else if (collision.gameObject.tag == "NPC")
        {
            // Load Skill Panel
            if (Input.GetKeyDown(KeyCode.F))
            {
                NPCName.SetActive(false);
                OnSkillPanelClick();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);
    }

    public void OnSkillPanelClick()
    {
        uiController.displaySkillPanel();
    }

    private void Update()
    {
        double level = characterLvl.calculateLevel();

        int levelDisplayText = (int)level;
        playerLevel.SetText(levelDisplayText.ToString());
        double percentage = level - levelDisplayText;
        int percentageDisplayText = (int) (percentage * 100f);
        playerEXPProgess.SetText(percentageDisplayText.ToString() + "%/100%");

        long ruby = progressSerial.RubyToSave;
        playerRuby.SetText(ruby.ToString());
    }
}

