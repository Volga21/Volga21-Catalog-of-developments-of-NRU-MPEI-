using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class DT_InfoPanel : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] public RectTransform cursorTransform;
    [SerializeField] public RectTransform windowTransform;

    public bool isDebug = false;

    public UILineRenderer lineRenderer;

    [SerializeField] List<Vector2> positions = new List<Vector2>();

    private float timer;

    private void Start()
    {
        positions[0] = cursorTransform.localPosition;
        positions[1] = windowTransform.localPosition;
        lineRenderer.Points = positions.ToArray();
    }
    private void Update()
    {
        // positions[0] = cursorTransform.localPosition;
        // positions[1] = windowTransform.localPosition;
        // lineRenderer.Points = positions.ToArray();
        transform.position = Camera.main.WorldToScreenPoint(Target.position);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            positions[0] = cursorTransform.localPosition;
            positions[1] = windowTransform.localPosition;
            lineRenderer.Points = positions.ToArray();
        }
    }

}
