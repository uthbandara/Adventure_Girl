using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void Quit(){

        SceneManager.LoadScene("GameMenu");
    }

    public void Retry(){

        SceneManager.LoadScene("Level_1");
    }


}
