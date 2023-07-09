using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Animator fade;
    public SoundManager soundManager;

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
        SoundManager.instance.Stop("TacTac");
        StartCoroutine("GameOver");
    }

    IEnumerator GameOver()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CallNextLevel(){
        SoundManager.instance.Stop("TacTac");
        SoundManager.instance.Play("Win");
        StartCoroutine("NextLevel");
    }

    IEnumerator NextLevel()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
