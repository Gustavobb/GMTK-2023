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

    private void Start()
    {
        startTimer = Time.time + 3f;
        ruleTimer = Time.time + ruleCooldown;
        entityManager.paperPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
        entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
        entityManager.rockPointsTo = new List<EntityManager.Type>();
    }

    private void Update()
    {   
        if(onGame){
            if(ordered)fillTimer.fillAmount = Mathf.InverseLerp(0, ruleCooldown, ruleTimer-Time.time);
            else fillTimer.fillAmount = Mathf.InverseLerp(ruleCooldown, 0, ruleTimer-Time.time);
            if (Time.time >= ruleTimer)
            {
                ordered = !ordered;
                UpdateRules();
                ruleTimer = Time.time + ruleCooldown;
            }
        }
        else{
            countDownText.text = System.Math.Round(startTimer-Time.time).ToString();
            if (Time.time >= startTimer){
                onGame = true;
                countDownText.text = "";
                ruleTimer = Time.time + ruleCooldown;
            }
        }
    }

    private void UpdateRules()
    {
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
