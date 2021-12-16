using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    private CinemachineFreeLook cineCam;
    public Joystick movementJoystick;
    public Joystick cameraJoystick;
    public PhotonView photonView;

    private Vector3 velocity;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [SerializeField] private float gravity;

    
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }

    private void Move()
    {       

        //Y-Axis
        float horizontal = movementJoystick.Horizontal;
       
        //Z-Axis
        float vertical = movementJoystick.Vertical;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cineCam.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (direction.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        }
        cineCam.m_XAxis.Value += cameraJoystick.Horizontal * 100 * Time.deltaTime;
        cineCam.m_YAxis.Value += -cameraJoystick.Vertical * Time.deltaTime;

        //Calculate Gravity
        velocity.y += gravity * Time.deltaTime;
        //Apply gravity
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetJoysticks(GameObject camera)
    {
        Joystick[] tempJoystickList = camera.GetComponentsInChildren<Joystick>();
        foreach (Joystick temp in tempJoystickList)
        {
            if (temp.tag == "Joystick Movement")
                movementJoystick = temp;
            else if (temp.tag == "Joystick Camera")
                cameraJoystick = temp;
        }
        cineCam = camera.GetComponentInChildren<CinemachineFreeLook>();
        cineCam.LookAt = GameObject.Find("Head").transform;
        cineCam.Follow = transform;
    }
}