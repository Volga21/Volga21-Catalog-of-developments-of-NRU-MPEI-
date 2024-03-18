using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    public GameObject descPnl;
    //public Spawner spawner;
    [SerializeField]
    private float scale = 0.1f;
    [SerializeField]
    private float mouseSensativity = 3.0f;
    private float rotationY;
    private float rotationX;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform defaultTarget;

    public CollectionUIHandler CollectionUI;

    [SerializeField]
    private float distanceToTarget = 3.0f;

    [SerializeField]
    private float distanceToTargetMin = 2.0f;
    [SerializeField]
    private float distanceToTargetMax = 6.0f;
    [SerializeField]
    private float groundOffset = 0.2f;

    private Vector3 currentRotation;
    private Vector3 smoothVel = Vector3.zero;
    [SerializeField]
    private float smoothTime = 3.0f;

    private void Start()
    {
    }
    private void Update()
    {
        if (target)
        {

            if (!UiHandler.IsPointerOverUIElement())
            {
                if (Input.touchCount == 2)
                {
                    Zoom(Input.GetTouch(0), Input.GetTouch(1));
                }
                else if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
                {
                    CameraMove();
                }
                else
                {
                    if (Input.mouseScrollDelta.magnitude > 0)
                    {
                        distanceToTarget -= Input.mouseScrollDelta.y * scale;
                    }
                }

                if (distanceToTarget < distanceToTargetMin)
                    distanceToTarget = distanceToTargetMin;
                else if (distanceToTarget > distanceToTargetMax)
                    distanceToTarget = distanceToTargetMax;
                transform.position = target.position - transform.forward * distanceToTarget;
                if (transform.position.y < groundOffset)
                {
                    transform.position = new Vector3(transform.position.x, groundOffset, transform.position.z);
                }
            }
        }
        else
            ChangeTarget();
    }
    private void CameraMove()
    {
        float mouseX=0, mouseY=0;

#if UNITY_EDITOR_WIN
        mouseX = Input.GetAxis("Mouse X") * mouseSensativity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensativity;
#elif UNITY_ANDROID
        mouseX = Input.GetTouch(0).deltaPosition.x/mouseSensativity;
        mouseY = Input.GetTouch(0).deltaPosition.y/mouseSensativity;

#endif


        rotationX -= mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -25, 40);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVel, smoothTime);

        transform.localEulerAngles = currentRotation;
    }
    private void Zoom(Touch t1, Touch t2)
    {

        Touch touchZero = t1;
        Touch touchOne = t2;

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagDif = prevTouchDeltaMag - touchDeltaMag;

        distanceToTarget += deltaMagDif * scale * 0.05f;

    }
    [ContextMenu("ChangeTarget")]

    public void ChangeTarget()
    {
        try
        {
            target = Spawner.Instance.objectModel.transform;
        }
        catch
        {
            target=defaultTarget;
        }
    }
}
