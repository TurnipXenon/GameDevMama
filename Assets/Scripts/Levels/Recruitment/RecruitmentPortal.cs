using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecruitmentPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        TopDownCharacter2D mainCharacter = other.GetComponent<TopDownCharacter2D>();
        if (mainCharacter != null && mainCharacter.playerData.teamMemberList.Count > 0)
        {
            SceneManager.LoadScene("Scope");
        }
    }
}
