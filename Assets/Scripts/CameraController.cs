using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraController : MonoBehaviour {
    // A mouselook behaviour with constraints which operate relative to
    // this gameobject's initial rotation.
    // Only rotates around local X and Y.
    // Works in local coordinates, so if this object is parented
    // to another moving gameobject, its local constraints will
    // operate correctly
    // (Think: looking out the side window of a car, or a gun turret
    // on a moving spaceship with a limited angular range)
    // to have no constraints on an axis, set the rotationRange to 360 or greater.
    public Vector2 rotationRange = new Vector3(360, 360);
    public float rotationSpeed = 5;
    public float dampingTime = 0.2f;
    public bool autoZeroVerticalOnMobile = true;
    public bool autoZeroHorizontalOnMobile = false;
    public bool relative = true;
    private Vector3 offset;
    public GameObject player;
    private CharacterController car;

    private Vector3 m_TargetAngles;
    private Vector3 m_FollowAngles;
    private Vector3 m_FollowVelocity;
    private Quaternion m_OriginalRotation;

    public Transform target;
    public float angularSpeed;
    [SerializeField]
    [HideInInspector]
    private Vector3 initialOffset;
    private Vector3 currentOffset;


    private void Start()
    {
        m_OriginalRotation = transform.localRotation;
        offset = transform.position - player.transform.position;
        car = player.GetComponent<CharacterController>();
    }

    [ContextMenu("Set Current Offset")]
    private void SetCurrentOffset()
    {
        if (target == null)
        {
            return;
        }

        initialOffset = transform.position - target.position;
    }


    private void Update()
    {
        // we make initial calculations from the original local rotation
        transform.localRotation = m_OriginalRotation;
        transform.position = player.transform.position + offset;

        // read input from mouse or mobile controls
        float inputH;
        float inputV;
        if (relative)
        {
            inputH = CrossPlatformInputManager.GetAxis("Mouse X");
            inputV = CrossPlatformInputManager.GetAxis("Mouse Y");

            // with mouse input, we have direct control with no springback required.
            m_TargetAngles.y += inputH * rotationSpeed;
            m_TargetAngles.x += inputV * rotationSpeed;
     
        }
        else
        {
            inputH = Input.mousePosition.x;
            inputV = Input.mousePosition.y;

        }

        // smoothly interpolate current values to target angles
        m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, dampingTime);

        // update the actual gameobject's rotation
        //transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
        car.GetComponent<CharacterController>().transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, m_FollowAngles.y, 0);
        car.GetComponent<CharacterController>().SimpleMove(new Vector3(0, Input.mousePosition.y, 0));
    }
}

