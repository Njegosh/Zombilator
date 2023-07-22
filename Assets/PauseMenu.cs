using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineBrain cb;    

    public void Resume(){
        cb.enabled = true;
        this.gameObject.SetActive(false);
    }
    
    public void Quit(){
        SceneManager.LoadScene("MainMenu");
    }

}
