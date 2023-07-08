using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public void CallPlayButton(){
        StartCoroutine("PlayButton");
    }

    IEnumerator PlayButton()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(1);
    }

}
