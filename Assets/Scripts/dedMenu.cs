using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dedMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Resume(){
        SceneManager.LoadScene("Level1");
    }

    public void Quit(){
        SceneManager.LoadScene("MainMenu");
    }
}