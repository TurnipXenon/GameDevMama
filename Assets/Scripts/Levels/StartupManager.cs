using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    public String nextScene = "Recruitment";

    public void OnInteract()
    {
        SceneManager.LoadScene(nextScene);
    }
}
