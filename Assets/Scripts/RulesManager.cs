using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Collections;
[System.Serializable]

public class RulesManager : MonoBehaviour
{
    public EntityManager entityManager;

    public Animator fluxoAnimator, fluxoAnimator2, circleAnimator, UIAnimator;
    public GameObject fluxo;

    private bool ordered;

    public Image fillTimer;

    public float ruleCooldown;
    private float ruleTimer, startTimer, previousStartTimer;
    public static bool onGame;
    public Text countDownText;
    private bool startTimerBool = false;
    private bool animating = false;
    public int levelType;

    [SerializeField] protected SoundManager soundManager;

    private void Start()
    {  
        onGame = false;
        startTimer = 3f;
        ruleTimer = (float) System.Math.Round(Time.time + ruleCooldown);
        countDownText.text = "3";
        if(levelType == 1){
            entityManager.rockPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> ();
            entityManager.paperPointsTo = new List<EntityManager.Type>();

        }
        else if (levelType == 2){
            entityManager.rockPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type>();
            entityManager.paperPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
        }
        else{
            entityManager.rockPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Paper };
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        StartCoroutine(StartTimer());
    }

    private void Update()
    {   
        if (!startTimerBool) return;
        if(onGame)
        {
            if(ordered) fillTimer.fillAmount = Mathf.InverseLerp(0, ruleCooldown, ruleTimer-Time.time);
            else fillTimer.fillAmount = Mathf.InverseLerp(ruleCooldown, 0, ruleTimer-Time.time);
            float timeLeft = ruleTimer - Time.time;
            if (timeLeft < 2f && !animating)
            {
                SoundManager.instance.Play("TacTac");
                circleAnimator.SetTrigger("Pulse");
                UIAnimator.SetTrigger("Pulse");
                animating = true;
            }

            if (Time.time >= ruleTimer)
            {
                ordered = !ordered;
                UpdateRules();
                animating = false;
                ruleTimer = (float) System.Math.Round(Time.time + ruleCooldown);
            }
        }
        else
        {
            previousStartTimer = startTimer;
            startTimer -= Time.deltaTime;
            if (System.Math.Round(startTimer) != System.Math.Round(previousStartTimer)) SoundManager.instance.Play("Tic");
            if (startTimer <= .5f) countDownText.text = "GO!";
            else countDownText.text = System.Math.Round(startTimer).ToString();
            if (startTimer <= 0)
            {
                onGame = true;
                countDownText.text = "";
                ruleTimer = (float) System.Math.Round(Time.time + ruleCooldown);
            }
        }
    }

    public void GameOver()
    {
        SoundManager.instance.Stop("TacTac");
    }

    private IEnumerator StartTimer()
    {
        SoundManager.instance.Play("Tic");
        yield return new WaitForSeconds(1f);
        startTimerBool = true;
    }

    private void UpdateRules()
    {
        SoundManager.instance.Stop("TacTac");
        SoundManager.instance.Play("Change_flux");
        fluxoAnimator2.SetTrigger("Animate");
        circleAnimator.SetTrigger("Animate");
        UIAnimator.SetTrigger("Animate");

        if (levelType == 1 & ordered){
            fluxoAnimator.SetTrigger("ChangeOrder1");
            entityManager.rockPointsTo = new List<EntityManager.Type>();
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        else if (levelType == 1 & !ordered){
            fluxoAnimator.SetTrigger("ChangeOrder1");
            entityManager.rockPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> ();
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        else if (levelType == 2 & !ordered){
            fluxoAnimator.SetTrigger("ChangeOrder2");
            entityManager.rockPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type>();
            entityManager.paperPointsTo = new List<EntityManager.Type>{ EntityManager.Type.Scissors };
        }
        else if (levelType == 2 & ordered){
            fluxoAnimator.SetTrigger("ChangeOrder2");
            entityManager.rockPointsTo = new List<EntityManager.Type>();
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock, EntityManager.Type.Paper};
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        else if (!ordered)
        {
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.rockPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Paper };
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        else
        {
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.paperPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
            entityManager.rockPointsTo = new List<EntityManager.Type>();

        }
    }
}
