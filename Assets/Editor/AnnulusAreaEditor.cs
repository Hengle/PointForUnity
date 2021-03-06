﻿using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AnnulusArea),true)]
public class AnnulusAreaEditor : Editor
{
    protected AnnulusArea Target;
    protected Matrix4x4 mOld;
    protected Color colorOld;
    private float scaleValue;

    protected void OnEnable()
    {
        Target = target as AnnulusArea;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script");
        GetProperties();
        serializedObject.ApplyModifiedProperties();
        SceneView.RepaintAll();
    }

    protected void GetProperties()
    {
        Target.MinRadius = EditorGUILayout.FloatField("MinRadius", Target.MinRadius);
        Target.MaxRadius = EditorGUILayout.FloatField("MaxRadius", Target.MaxRadius);
    }

    protected void OnSceneGUI()
    {
        mOld = Handles.matrix;
        colorOld = Handles.color;

        DrawAnnulusArea();
        DrawMinMaxScaleHandle();

        Handles.color = colorOld;
        Handles.matrix = mOld;
    }

    /// <summary>
    /// 绘制环形区域
    /// </summary>
    private void DrawAnnulusArea()
    {
        Handles.color = Target.appearance.outerColor;
        Handles.DrawSolidArc(Target.transform.position, Target.transform.up, Target.transform.right, -Target.angle, Target.MaxRadius);
        Handles.color = Target.appearance.innerColor;
        Handles.DrawSolidArc(Target.transform.position, Target.transform.up, Target.transform.right, -Target.angle, Target.MinRadius);
    }

    /// <summary>
    /// 绘制内外缩放控制
    /// </summary>
    protected void DrawMinMaxScaleHandle()
    {
        scaleValue = DrawScaleHandle(Target.MinRadius, Target.appearance.innerEdgeColor, -Target.transform.right);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Scale Min Radius");
            Target.MinRadius = scaleValue;
            Repaint();
        }

        scaleValue = DrawScaleHandle(Target.MaxRadius, Target.appearance.outerEdgeColor, -Target.transform.forward);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Scale Max Radius");
            Target.MaxRadius = scaleValue;
            Repaint();
        }
    }

    /// <summary>
    /// 绘制缩放控制
    /// </summary>
    /// <param name="radius">半径</param>
    /// <param name="color">颜色</param>
    /// <param name="direction">方向</param>
    /// <returns>返回缩放值</returns>
    protected float DrawScaleHandle(float radius, Color color, Vector3 direction)
    {
        Handles.color = color;
        Handles.CircleHandleCap(0, Target.transform.position, Quaternion.LookRotation(Target.transform.up), radius, EventType.Repaint);
        EditorGUI.BeginChangeCheck();
        if (radius <= 0)
            radius = 0.1f;
        return Handles.ScaleSlider(radius, Target.transform.position, direction, Target.transform.rotation, HandleUtility.GetHandleSize(Target.transform.position), 0.5f);
    }
}
