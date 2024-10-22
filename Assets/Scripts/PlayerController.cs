using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane=1;
    public float laneDistance=4;

    // public bool isGrounded;
    // public LayerMask groundLayer;
    // public Transform groundCheck;
    // private Vector3 velocity;

    public float jumpForce;
    public float Gravity=-20;

    public Animator animator;
    // private bool isSliding=false;

    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
        animator.SetBool("isGrounded", true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGameStarted)
            return;
            
        animator.SetBool("isGameStarted", true);

        if(forwardSpeed<maxSpeed)
            forwardSpeed+=0.1f*Time.deltaTime;

        
        direction.z=forwardSpeed;

        if(SwipeManager.swipeUp){
            if(controller.isGrounded){
               
                direction.y=-1;
                Jump();
                StartCoroutine(Ground());
            }
        }
        else{
            direction.y+=Gravity*Time.deltaTime;
        }
        
        if(SwipeManager.swipeDown){
            StartCoroutine(Slide());
        }

        if(SwipeManager.swipeRight){
            desiredLane++;
            if(desiredLane==3){
                desiredLane=2;
            }
        }

        if(SwipeManager.swipeLeft){
            desiredLane--;
            if(desiredLane==-1){
                desiredLane=0;
            }
        }

        Vector3 targetPosition=transform.position.z*transform.forward+transform.position.y*transform.up;
        
        if(desiredLane==0){
            targetPosition += Vector3.left * laneDistance;
        }
        else if(desiredLane==2){
            targetPosition += Vector3.right * laneDistance;
        }

        // transform.position=Vector3.Lerp(transform.position,targetPosition,70*Time.deltaTime);
        // transform.position=targetPosition;

        if(transform.position==targetPosition)
            return;
        Vector3 diff=targetPosition-transform.position;
        Vector3 moveDir=diff.normalized*25*Time.deltaTime;


        if(moveDir.sqrMagnitude<diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void FixedUpdate(){
        if(!PlayerManager.isGameStarted)
            return;
        controller.Move(direction*Time.fixedDeltaTime);
    }
    private void Jump(){
        direction.y=jumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.transform.tag=="Obstacle"){
            PlayerManager.gameOver=true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
    private IEnumerator Slide(){
        animator.SetBool("isSliding", true);
        controller.center=new Vector3(0,-0.5f,0);
        controller.height=1;
        yield return new WaitForSeconds(1.3f);
        controller.center=new Vector3(0,0,0);
        controller.height=2;
        animator.SetBool("isSliding", false);
    }
    private IEnumerator Ground(){
        animator.SetBool("isGrounded", false);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isGrounded", true);
    }
}
