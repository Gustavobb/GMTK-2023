using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Rule
{
    public string className;
    public List<string> advantage;
}

public class RulesManager : MonoBehaviour
{
    public List<Rule> rules;

    public float ruleCooldown = 10f;
    private float ruleTimer;

    private void Start()
    {
        ruleTimer = Time.time + ruleCooldown;
    }

    private void Update()
    {
        if (Time.time >= ruleTimer)
        {
            UpdateRules();
            ruleTimer = Time.time + ruleCooldown;
        }
    }

    private void UpdateRules()
    {
        // Atualize as regras do jogo

        // Anima a UI
    }

    public bool DoesClassWin(string classA, string classB)
    {
        // Verifique se a classe A vence a classe B
        Rule ruleA = rules.Find(x => x.className == classA);
        return ruleA.advantage.Contains(classB);
    }
}
