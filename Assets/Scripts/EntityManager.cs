using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public enum Type
    {
        Rock,
        Paper,
        Scissors
    }

    public List<Entity> rockEntities = new List<Entity>();
    public List<Entity> paperEntities = new List<Entity>();
    public List<Entity> scissorsEntities = new List<Entity>();

    public List<Type> rockPointsTo = new List<Type>();
    public List<Type> paperPointsTo = new List<Type>();
    public List<Type> scissorsPointsTo = new List<Type>();

    public List<Type> rockPointsFrom = new List<Type>();
    public List<Type> paperPointsFrom = new List<Type>();
    public List<Type> scissorsPointsFrom = new List<Type>();
    public Vector3 weight;
    public float distanceToKill = 1f;

    private void HandlePointsFrom()
    {
        rockPointsFrom.Clear();
        paperPointsFrom.Clear();
        scissorsPointsFrom.Clear();

        foreach (Type type in rockPointsTo)
            PointsFromOperation(type, Type.Rock);

        foreach (Type type in paperPointsTo)
            PointsFromOperation(type, Type.Paper);

        foreach (Type type in scissorsPointsTo)
            PointsFromOperation(type, Type.Scissors);
    }

    private void PointsFromOperation(Type type, Type from)
    {
        switch (type)
        {
            case Type.Rock:
                rockPointsFrom.Add(from);
                break;
            case Type.Paper:
                paperPointsFrom.Add(from);
                break;
            case Type.Scissors:
                scissorsPointsFrom.Add(from);
                break;
        }
    }

    private void Update()
    {
        HandlePointsFrom();
    }
}
