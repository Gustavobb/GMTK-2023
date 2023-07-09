using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Animator fade;

    public void CallPlayButton(){
        StartCoroutine("PlayButton");
    }

    IEnumerator PlayButton()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(1);
    }

    public void CallMenuButton(){
        StartCoroutine("MenuButton");
    }
    IEnumerator MenuButton()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }

    public void CallGameOver(){
        StartCoroutine("GameOver");
    }

    IEnumerator GameOver()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CallNextLevel(){
        StartCoroutine("NextLevel");
    }

    IEnumerator NextLevel()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
