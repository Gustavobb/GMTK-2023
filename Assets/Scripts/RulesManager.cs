using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Collections;
[System.Serializable]

public class RulesManager : MonoBehaviour
{
    public EntityManager entityManager;

    public Animator fluxoAnimator, fluxoAnimator2;
    public GameObject fluxo;

    private bool ordered;

    public Image fillTimer;

    public float ruleCooldown;
    private float ruleTimer, startTimer;
    public static bool onGame;
    public Text countDownText;
    private bool startTimerBool = false;

    [SerializeField] protected SoundManager soundManager;

    private void Start()
    {  
        onGame = false;
        startTimer = 3f;
        ruleTimer = Time.time + ruleCooldown;
        countDownText.text = "3";
        entityManager.paperPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
        entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
        entityManager.rockPointsTo = new List<EntityManager.Type>();
        soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(StartTimer());
    }

    private void Update()
    {   
        if (!startTimerBool) return;
        if(onGame)
        {
            if(ordered) fillTimer.fillAmount = Mathf.InverseLerp(0, ruleCooldown, ruleTimer-Time.time);
            else fillTimer.fillAmount = Mathf.InverseLerp(ruleCooldown, 0, ruleTimer-Time.time);
            if (Time.time >= ruleTimer)
            {
                ordered = !ordered;
                UpdateRules();
                ruleTimer = Time.time + ruleCooldown;
            }
        }
        else
        {
            startTimer -= Time.deltaTime;
            if (startTimer < 1) countDownText.text = "GO!";
            else countDownText.text = System.Math.Round(startTimer).ToString();
            if (startTimer <= 0)
            {
                onGame = true;
                countDownText.text = "";
                ruleTimer = Time.time + ruleCooldown;
            }
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        startTimerBool = true;
    }

    private void UpdateRules()
    {
        SoundManager.instance.Play("Change_flux");
        fluxoAnimator2.SetTrigger("Animate");
        if (ordered){
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.rockPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Paper };
            entityManager.paperPointsTo = new List<EntityManager.Type>();
        }
        else{
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.paperPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
            entityManager.rockPointsTo = new List<EntityManager.Type>();

        }
    }
}
