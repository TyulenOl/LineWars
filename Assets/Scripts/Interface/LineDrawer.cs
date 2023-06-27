using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Graph;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer lineSpriteRenderer;
    [SerializeField] private Transform firstTransform;
    [SerializeField] private Transform secondTransform;
    private void Update()
    {
        DrawLine(firstTransform, secondTransform);
    }

    public void DrawLine(Transform firstNodeTransform, Transform secondNodeTransform)
    {
        var positionFirst = firstNodeTransform.position;
        var positionSecond = secondNodeTransform.position;
        var distance = Vector3.Distance(positionFirst, positionSecond);
        lineSpriteRenderer.size = new Vector2(distance,lineSpriteRenderer.size.y);
        var center = positionFirst;
        var newSecondNodePosition = positionSecond - center;
        var radian = Mathf.Atan2(newSecondNodePosition.y , newSecondNodePosition.x) * 180 / Math.PI;
        lineSpriteRenderer.transform.rotation = Quaternion.Euler(0,0,(float)radian);
        lineSpriteRenderer.transform.position = (positionFirst + positionSecond) / 2;
    }
}
