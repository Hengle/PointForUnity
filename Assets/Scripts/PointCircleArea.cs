﻿using UnityEngine;

public class PointCircleArea : PointAreaBase
{
    private float radius = 1f;
    public float Radius { get { return radius; } set { radius = Mathf.Max(0, value); } }

    public override Vector3 GetRandomPositionInArea()
    {
        return Random.insideUnitCircle * radius;
    }

    public override Vector3 GetRandomPositionInEgde()
    {
        float x = Random.value;
        return new Vector3(x * radius, Mathf.Sqrt(1 - x * x) * radius, 0);
    }

    public override PointBase GetRandomPointInArea()
    {
        Vector3 pos = GetRandomPositionInArea();
        return new PointBase(pos, GetAngleByPosition(pos));
    }

    public override PointBase GetRandomPointInEgde()
    {
        Vector3 pos = GetRandomPositionInEgde();
        return new PointBase(pos, GetAngleByPosition(pos));
    }
}