using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

[System.Serializable]


public class RulesManager : MonoBehaviour
{
    public EntityManager entityManager;

    public Animator fluxoAnimator;

    private bool ordered;

    public Image fillTimer;

    public float ruleCooldown = 10f;
    private float ruleTimer;

    private void Start()
    {
        ruleTimer = Time.time + ruleCooldown;
        entityManager.rockPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
        entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Paper };
        entityManager.paperPointsTo = new List<EntityManager.Type>();
    }

    private void Update()
    {
        if(ordered)fillTimer.fillAmount = Mathf.InverseLerp(0, ruleCooldown, ruleTimer-Time.time);
        else fillTimer.fillAmount = Mathf.InverseLerp(ruleCooldown, 0, ruleTimer-Time.time);
        if (Time.time >= ruleTimer)
        {
            ordered = !ordered;
            UpdateRules();
            ruleTimer = Time.time + ruleCooldown;
        }
    }

    private void UpdateRules()
    {
        if (ordered){
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.rockPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Paper };
            entityManager.paperPointsTo = new List<EntityManager.Type> {};
        }
        else{
            fluxoAnimator.SetTrigger("ChangeOrder");
            entityManager.paperPointsTo = new List<EntityManager.Type> { EntityManager.Type.Scissors };
            entityManager.scissorsPointsTo = new List<EntityManager.Type> { EntityManager.Type.Rock };
            entityManager.rockPointsTo = new List<EntityManager.Type>();

        }
    }

}
