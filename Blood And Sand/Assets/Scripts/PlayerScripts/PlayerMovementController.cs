using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject rayOrigin;
    //variables for animation
    public Animator PlayerAnimator;
    public bool isMoving = false;

    private PhotonView PV;
    private CinemachineFreeLook CFL;
    public CharacterController controller;
    public Camera ray_camera;
    public GameObject p_camera;
    //public Transform cam;
	public float moveSpeed = 70f;
    public float turnSmoothTime = 0.1f;
    // Jumping stuff
    public float jumpHeight = 2.0f;
    public float jumpForwardForce = 5.0f;
    public float gravityValue = -9.81f;
    private bool hasJumped = false;
    private Vector3 playerVelocity;
    // Player falling stuff
    private bool isFalling = false;
    private bool setFallingStats = false;
    private Vector3 fallDir;
    private bool ifSprint = false;
    
    float horizontal; 
    float vertical;

    float turnSmoothVelocity;
    public Vector3 moveDir;
    
    private GameObject netController;

    // For highliting
    public GameObject eToInteract;
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
        netController = GameObject.Find("TheReaper");
        if(netController){
            netController.GetComponent<deathScript>().playerObjects.Add(gameObject);
        }
        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {

            // RAYTRACING for Player to interact with things (items, etc).
            RaycastHit hit;
            Ray pointing = ray_camera.ScreenPointToRay(Input.mousePosition);
            GameObject obj = null;
            if(Physics.Raycast(pointing, out hit, 20)){
                    //Debug.DrawRay(pointing.origin, pointing.direction * 20);
                    if(hit.collider.tag == "Interact"){
                        //hit.collider.gameObject.GetComponent<Interact>().isHighlightOn(true);
                        // Have 'e' button pop up to display controls
                        obj = hit.collider.gameObject; 
                        eToInteract.SetActive(true);
                        //gameObject.transform.Find("Canvas_SS_Overlay/'E' to interact").gameObject.SetActive(true);
                        if(Input.GetKeyDown("e")){ 
                            hit.collider.gameObject.GetComponent<Interact>().interactFunction(this.gameObject);
                        }
                    }
                    else if(hit.collider.tag != "Interact"){
                        eToInteract.SetActive(false);
                    }
                    if (obj == null){
                        eToInteract.SetActive(false);
                        //gameObject.transform.Find("Canvas_SS_Overlay/'E' to interact").gameObject.SetActive(false);
                    }
            }
            else{
                eToInteract.SetActive(false);
            }
            // Movement input
            if(!isPaused){
                horizontal = Input.GetAxisRaw("Horizontal");
                PlayerAnimator.SetFloat("moving sideways", horizontal);
                vertical = Input.GetAxisRaw("Vertical");
                PlayerAnimator.SetFloat("moving forward", vertical);

            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


            /* 
             * Making sure the Vector3 property for Jumping is set to 0 grounded and/or below 0
             * using the custom controller.isGrounded function instead of our custom one, 
             * or else the player will stop mid-air at the distance we set for the raytracing.
             *
             * I'm also setting the velocity of 'x' and 'z' of the player back to zero so the player doesn't
             * move forward on it's own without user input.
            */
            if (controller.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                playerVelocity.x = 0f;
                playerVelocity.z = 0f;                

                // Set hasJumped back to false to allow for more jumping;
                hasJumped = false;
                // Since the player has landed, he is no longer falling ;)
                isFalling = false;
            } 

            
            if (Input.GetMouseButton(0)) {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                    float targetAngle1 = p_camera.transform.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0f, targetAngle1, 0f);
                } 
                // else {
                //     float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                //     transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                // }
            /********************************************************************************************
            *      - [PLAYER MOVEMENT] -                                                                *
            * Making sure player is currently moving. If direction.magnitude == 0, then no movement.    *
            *********************************************************************************************
            */
            if (direction.magnitude >= 0.1f)
            {
                isMoving = true;
                PlayerAnimator.SetBool("moving?", isMoving);
                // If player is moving, adjust movement based off of camera direction.
                // Set targetAngle to be the angle and direction at which the camera is pointing.
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                // [OLD ATTEMPT] --> float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                /*
                * Use angle instead of target angle in order to smooth the turning, but
                * I'm leaving it off intentionally for now.
                * I'm thinking we can do the smoothing with animations.
                */
                if (Input.GetMouseButton(0)) {
                    float targetAngle1 = p_camera.transform.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0f, targetAngle1, 0f);
                } else {
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
                // [OLD ATTEMPT] --> transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                /*
                * Using Move instead of SimpleMove because we've implemented Jumping for our character,
                * and SimpleMove ignores the 'y'-axis, therefore not allowing us to make the character jump.
                * I've also added the " * Time.deltaTime" at the end of "controller.Move" function, (I'm not sure
                * why, but without it I've ran into many unexplainable errors).
                */
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // Sprinting
                if(Input.GetKey(KeyCode.LeftShift) && IsGrounded() && (!isPaused)) { 
                    controller.Move(moveDir.normalized * moveSpeed * 2f * Time.deltaTime);
                }
                // If not sprinting, then move normally.
                else { 
                    /*
                    * This long line of code is meant to check if the player is falling but hasn't jumped.
                    * It also has a bool check if the player had sprinted or not, this so that when
                    * forward falling momentum is applied it matches with the movement speed before
                    * the player started falling.
                    */
                    if(!IsGrounded() && !hasJumped || (Input.GetKey(KeyCode.LeftShift) && !IsGrounded() && !hasJumped)){
                        if(Input.GetKey(KeyCode.LeftShift))
                            ifSprint = true;
                        isFalling = true;
                    }
                    // Move normally.
                    else if(IsGrounded() && (!isPaused)){
                        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                    }
                }
            } 
            else
            {
                isMoving = false;
                PlayerAnimator.SetBool("moving?", isMoving);
            }



            /************************************************************************************************************      
            *        - [PLAYER FALLING] -                                                                               *
            * This part of the code makes sure there is "natural" momentum applied to a player when they begin falling. *
            * Falling, in this case, means to fall with directional momentum without the player pressing Jump prior     *
            * to finding themselves in this situation.                                                                  *
            *************************************************************************************************************
            */
            if(isFalling){
                // This bool makes sure that the direction is only set once, so the player cannot change directions mid-air.
                if(!setFallingStats){
                    // Set target direction.
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                    // Checks to see if the player was sprinting before they fell.
                    if(ifSprint)
                        fallDir += Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * 2 * jumpForwardForce;
                    else
                        fallDir += Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * jumpForwardForce;
                    // This bool makes sure that the direction is only set once, so the player cannot change directions mid-air.
                    setFallingStats = true;
                }
                
            }
            // Reset all the falling stats back to 0 or false so as to not cause conflicts with anything else.
            else{
                fallDir.x = 0;
                fallDir.z = 0;
                setFallingStats = false;
            }
            // Apply the forward momentum when falling.
            controller.Move(fallDir * Time.deltaTime);

            
            
            /****************************************************************************
            *        - [PLAYER JUMPING] -                                               *
            * Need to check if sprint key is being held down as the player is jumping   *
            * to adjust the forward push when they jump.                                *
            *****************************************************************************
            */
            if(IsGrounded() && Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Jump") && !hasJumped && !isPaused){
                /*
                * Since I've changed our controller movement function from "SimpleMove" to "Move", this meant that
                * once you've stopped inputting movement and you'd be falling, you're character would stop
                * any movement on the 'x' and 'z'-axis and would plument straight down.
                *
                * So because of this, I've had to manually introduce a "push" in the direction that the
                * player was facing initially as they pressed the "jump" button.
                */
                // Set hasJumped equal to true before anything else is done, so as to prevent multiple jumps.
                hasJumped = true;
                // 1. Grab the direction of the initial direction the player is facing.
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                // 2. Add force to that direction. 
                playerVelocity += Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * 2 * jumpForwardForce;
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            
            }
            // Jumping WITHOUT holding sprint key.
            else if(IsGrounded() && Input.GetButtonDown("Jump") && !hasJumped && !isPaused){
                // Set hasJumped equal to true before anything else is done, so as to prevent multiple jumps.
                hasJumped = true;
                // 1. Grab the direction of the initial direction the player is facing.
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + p_camera.transform.eulerAngles.y;
                // 2. Add force to that direction. 
                playerVelocity += Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * jumpForwardForce;
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            // Make sure player falls back down after launch.
            playerVelocity.y += gravityValue * Time.deltaTime;
            // This "controller.Move" function controls the player "Jump" and "Push forward while Jumping".
            controller.Move(playerVelocity * Time.deltaTime);
        }

        // Player falling off edge with momentum
        

    }

    /********************************************************************************
    *       - [CHECK IF PLAYER GROUNDED] -                                          *
    * Character controller input is unreliable and gives off false-positives,       *
    * so I made our own way of checking if the player is grounded using raycasts    *
    *********************************************************************************
    */

    bool IsGrounded()
    {
        // Bit shifting from the first layer mask (1. Transparent FX) to the player mask (9. Players).
        int layerMask = 1 << 9;
        // I'm inverting the layer mask so that the raycast will hit everything except the player.
        layerMask = ~layerMask;
        /*
        * Switched from Raycast to Spherecast to get a wider area to detect for collision.
        * Spherecasting downard (-transform.up), at a short distance (1.5f), while using the
        * newly made Layer mask (layerMask) to check if something is right under the player
        * and that the player is touching it.
        */
        RaycastHit hit;
        // [OLD ATTEMPT] --> if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, layerMask)){
        if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 1.5f, layerMask)){
            return true;
        }
        else{
            return false;
        }
    }

    // Used to attempt to debug and imitate the shape of the Spherecast. Will be disabled to avoid confusion.
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 0.5f);
    // }
}
