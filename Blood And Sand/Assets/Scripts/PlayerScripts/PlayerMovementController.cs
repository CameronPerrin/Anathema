using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private PhotonView PV;
    private CinemachineFreeLook CFL;
    public CharacterController controller;
    public GameObject p_camera;
    //public Transform cam;
	public float moveSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            p_camera = Camera.main.gameObject;
            CFL = transform.GetChild(0).GetComponent<CinemachineFreeLook>();
            //CFL.Follow = transform.GetChild(1);
            CFL.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            }
        }

    }
}
