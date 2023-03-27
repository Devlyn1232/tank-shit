using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

        //bools
    private bool Sprinting = false;
    private bool Crouching = false;
    private bool Sliding = false;
    public bool geppo = false;
    private bool Climbing = false;
    public bool SBTrue = true;
    //movement variables
    public float sprintSpeed = 12;
    public float sSpeed = 5f;
    public float speed;          
    public float speedCap = 20f;                           
    public float crouchSpeed = 3;
    public float slideSpeed;
    public float jumpHeight = 3f;
    public float climb = 1f;
    public float speedMul;
    public int extraJumps = 0;
    public int maxJumps = 3;
    public float friction = 0.1f;
    float m_FieldOfView;
    //misic variables
    private bool SSlideing = false;
    public float force = 1f;
    public float gravity = -1f;
    public bool coolDown = false;
    private float timer = 10f;
    private float timer2 = 0.1f;
    public Camera cam;
    private GameObject wayPoint;
    private Vector3 wayPointPos;


    //ground variables
    public LayerMask groundMask; 
    public Transform groundCheck;
    public float groundDistance = 0.5f;

    //public LayerMask ledgeMask;
    public Transform wallCheck;
    public float wallDistance = 0.5f;

    public Transform headCheck;
    public float headDistance = 0.5f;

    Vector3 velocity;
    public bool isGrounded;
    bool canClimb;
    bool wallAbove;
    //bool cantjump = false;
    bool jumpButtonPrev = false;
    //scrypt

    void Start()
    {
        wayPoint = GameObject.Find("wayPoint");
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
        }
        if (timer2 <= 0)
        {
            timer2 = 0.1f;
        }
        if (timer <= 0)
        {
            timer = 20f;
            SBTrue = true;
        }
        //somethin for sliding
        wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);

        //creating floor checker
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f;
        }

        //creating wall checker
        canClimb = Physics.CheckSphere(wallCheck.position, wallDistance, groundMask);

        //creating non headhiter
        wallAbove = Physics.CheckSphere(headCheck.position, headDistance, groundMask);

        //movement keys
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // fixing jump
        bool jumpButtonNext = Input.GetButton("Jump");
        bool jump = jumpButtonNext && !jumpButtonPrev;
        jumpButtonPrev = jumpButtonNext;
        // creating jump function
        //if (canClimb)
        //{
        //    extraJumps = 0;
        //}
        if ((isGrounded))
        {
            extraJumps = 0;
            if (jump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        //extra jump
        if ((maxJumps > extraJumps) && jump)
        {
            print(extraJumps);
            if (!isGrounded && !canClimb)
            {
                extraJumps += 1;
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //reseting jump number
            print(extraJumps);
            if (maxJumps <= extraJumps)
            {
                print("out of jumps");
            }
        }
        //geppo
        if (geppo && jump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    

        //climbing
        if (canClimb && Input.GetButton("Jump"))
        {
            Climbing = true;
        }
        else
        {
            Climbing = false;
        }
        //sprinting
        if (Input.GetButton("Sprint"))
        {
            Sprinting = true;
        }
        else 
        {
            Sprinting = false;
        }
        //crouching
        if (Input.GetButton("Crouch") || Input.GetKey(KeyCode.C))
        {
            Crouching = true;
        }
        else
        {
            if (wallAbove)
            {
                Crouching = true;
            }
            else
            {
                Crouching = false;
            }
        }
        if (Sprinting && Crouching && isGrounded)
        {
            Sliding = true;
        }
        else
        {
            Sliding = false;
        }
        //movement branches
        if(Sprinting || Crouching || Climbing)
        {
            //climbing mechanics
            if(Climbing)
            {
                velocity.y = Mathf.Sqrt(climb * -1f * gravity);
                if (speed >= 5f)
                {
                    speed -= Time.deltaTime*5;
                }
            }
            else
            //crouching mechanics
            if (Crouching)
            {

                
                transform.localScale = new Vector3(1f, 0.6f, 1f);
                
                if (isGrounded)
                {
                    if (speed >= crouchSpeed)
                    {
                        
                        speed -= Time.deltaTime*5;
                    }
                }
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            
            
            if  (Sprinting)
            {   
                if (isGrounded)
                {
                    if (speed <= sprintSpeed)
                    {
                        speed += Time.deltaTime*5;
                    }
                    if (speed >= sprintSpeed)
                    {
                        speed -= Time.deltaTime*5;
                    }   
                }
                else
                {
                    speed += Time.deltaTime/10;
                }
            }
            if (Sliding)
            {   
                if (isGrounded)
                {
                    if (speed <= 15)
                    {
                        speed -= (transform.position.y - wayPointPos.y);
                    }
                    //if (speed <= 20)
                    {
                        speed -= (transform.position.y - wayPointPos.y)*0.9f;
                    }
                    //if (speed <= 30)
                    {
                        speed -= (transform.position.y - wayPointPos.y);
                    }
                    if (speed <= sprintSpeed)
                    {
                        speed -= Time.deltaTime*10;
                    }
                    if (speed <= 1)
                    {
                        speed += Time.deltaTime*15;
                    }
                }
            }
        }
        else
        {
            if (isGrounded)
            {
                if (speed >= sSpeed)
                {
                    speed -= Time.deltaTime*30;
                }
                if (speed <= sSpeed -1)
                {
                    speed += Time.deltaTime*10;
                }
            }
            else
            {
                speed += 0.001f;
            }
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (speed >= speedCap)
        {
            speed -= 1;
        }
        cam.GetComponent<Camera>().fieldOfView = speed*2f+40;
        if (cam.GetComponent<Camera>().fieldOfView <= 20)
        {
            cam.GetComponent<Camera>().fieldOfView = 20;
        }



        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }   

    
//void OnGUI()
   // {
        //Set up the maximum and minimum values the Slider can return (you can change these)
        //float max, min;
        //max = 150.0f;
        //min = 20.0f;
        //This Slider changes the field of view of the Camera between the minimum and maximum values
        //m_FieldOfView = GUI.HorizontalSlider(new Rect(20, 20, 100, 40), m_FieldOfView, min, max);
  //  }

}
