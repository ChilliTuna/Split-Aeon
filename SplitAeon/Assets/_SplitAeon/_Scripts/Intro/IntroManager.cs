using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

    public float introLength;

    public string sceneName;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > introLength)
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}
