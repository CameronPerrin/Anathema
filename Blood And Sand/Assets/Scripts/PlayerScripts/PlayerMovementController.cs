using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject rayOrigin;

    private PhotonView PV;
    private CinemachineFreeLook CFL;
    public CharacterController controller;
    public Camera ray_camera;
    public GameObject p_camera;
    //public Transform cam;
	public float moveSpeed = 70f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // for highliting
    private GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            ray_camera = Camera.main;
            p_camera = Camera.main.gameObject;
            CFL = transform.GetChild(0).GetComponent<CinemachineFreeLook>();
            //CFL.Follow = transform.GetChild(1);
            CFL.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine && !isPaused)
        {

            // RAYTRACING for Player to interact with things (items, etc).
            RaycastHit hit;
            Ray pointing = ray_camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(pointing, out hit, 20)){
                    Debug.DrawRay(pointing.origin, pointing.direction * 20);
                    if(hit.collider.tag == "Interact"){
                        hit.collider.gameObject.GetComponent<Interact>().isHighlightOn(true);
                        if(Input.GetKeyDown("e")){ 
                            hit.collider.gameObject.GetComponent<Interact>().interactFunction(this.gameObject);
                        }
                    }
            }
        

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //use angle instead of target angle in order to smooth the turning.
                // leaving it off intentionally for now
                //I'm thinking we can do the smoothing with animations.
                if (Input.GetMouseButton(0)) {
                    float targetAngle1 = p_camera.transform.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0f, targetAngle1, 0f);
                } else {
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
                //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if(Input.GetKey(KeyCode.LeftShift)) { // Sprinting
                    controller.SimpleMove(moveDir.normalized * moveSpeed * 2f);
                } else {
                    controller.SimpleMove(moveDir.normalized * moveSpeed);
                }
                

            }
            else{
                // Make sure character needs gravity to work before we call it                
                if(!controller.isGrounded){
                    // Gravity only works if movement is called
                    // so we're calling movement but we're making sure it's returning a 0 value so the player doesn't actually move!
                    controller.SimpleMove(transform.position*0);
                }
            }
        }

    }
}
