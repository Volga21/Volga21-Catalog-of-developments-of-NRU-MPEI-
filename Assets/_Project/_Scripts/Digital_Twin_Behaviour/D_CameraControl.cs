using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CameraControl : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private float speedRotationHorizontal;
    [SerializeField] private float speedRotationVertical;

    [SerializeField] private float yaw;
    [SerializeField] private float pitch;

    private float _horizontalDirection;
    private float _verticlaDirection;

    // Update is called once per frame
    void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _verticlaDirection = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(transform.position.x + (_horizontalDirection * _speed), transform.position.y + +(_verticlaDirection * _speed), transform.position.z);

        if (Input.GetMouseButton(2))
        {
            yaw += speedRotationHorizontal * Input.GetAxis("Mouse X");
            pitch -= speedRotationVertical * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
}
