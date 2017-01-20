
using UnityEngine;


public class CameraController : MonoBehaviour
{
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
 
    public GameObject player;
    private CharacterController car;

    private Vector3 m_TargetAngles;
    private Vector3 m_FollowAngles;
    private Vector3 m_FollowVelocity;
    private Vector3 initialOffset;
    private Vector3 currentOffset;
    private Vector3 offset1;
    private Vector3 offset;

    private Quaternion m_OriginalRotation;

    public Transform target;
    
    public float angularSpeed;
    [SerializeField]
    [HideInInspector]
  
    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 15f;
    float startY;


    private void Start()
    {
        m_OriginalRotation = transform.localRotation;
        startY = target.position.y;
        offset = transform.position - player.transform.position;
        offset1 = new Vector3(target.position.x - 12, target.position.y + 1, target.position.z - 11);
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

    void LateUpdate()
    {
        offset1 = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up) * offset1;
        transform.position = target.position + offset1;
        transform.LookAt(target.position);
    }


    private void Update()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
        

        // read input from mouse or mobile controls
        float inputH;
        float inputV;
        if (relative)
        {
            inputH = Input.GetAxis("Mouse X");
            inputV = Input.GetAxis("Mouse Y");

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
        car.GetComponent<CharacterController>().transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, m_FollowAngles.y, 0);
        car.GetComponent<CharacterController>().SimpleMove(new Vector3(0, Input.mousePosition.y, 0));
    }
}

