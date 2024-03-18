using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// класс составной части модели разработки
/// </summary>
public class MovablePart : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;
    public bool isSplitting = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    private void Start()
    {
        targetPosition = transform.position + direction;
        startPosition = transform.position;
    }
    private void Update()
    {
        if (isSplitting)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 1);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * 1);
        }
    }

    public void Split(bool s)
    {
        isSplitting = s;
    }
}