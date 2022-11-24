using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    private Vector3 startTouchPosition, endTouchPosition;
    public float speed;
    public float maxSpeed;
    public float speedR;
    public float speedL;
    public float jumpY;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public AudioSource coinCollect;
    bool start = true;
    //bool swipeAllowed = true;
    bool isGrounded=true;
    private Rigidbody RB;
    public ParticleSystem landingParticle;
    public ParticleSystem jumpParticle;
    public int tapCount;
    GameManager GM;
    private Touch touch;
    public float speedmodifier;
    public Vector3 coinPos;
    public Quaternion coinRot;
    bool jump = false;
    bool startParticle = false;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        speedmodifier = 0.02f;
        GM = GameManager.instance;
        
    }
    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        //if (swipeAllowed == true)
        //{
        //    SwipeCheck();
        //}

        SwipeCheck();


        void SwipeCheck()
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
               
                    if (touch.phase == TouchPhase.Moved)
                    {


                        transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speedmodifier, transform.position.y,
                            /*transform.position.z + touch.deltaPosition.y * speedmodifier*/transform.position.z);

                    }
                if(transform.position.x>6.37)
                {
                    transform.position = new Vector3(6.36f, transform.position.y, transform.position.z);
                }
                if (transform.position.x < -6.37)
                {
                    transform.position = new Vector3(-6.36f, transform.position.y, transform.position.z);
                }

            }
        }



        if (start == true)      
        {
         
            Invoke("GetReady", 4f);

            start = false;

        }

        if (startplayer == true)
        {
           if (isGrounded)
            {
                Vector3 move = new Vector3(0, 0, 1);
                move *= speed * Time.deltaTime;
                var movementPlane = new Vector3(RB.velocity.x, 0, RB.velocity.z);
                if (movementPlane.magnitude < speed)
                    RB.AddForce(move, ForceMode.VelocityChange);
                //RB.AddForce(0, 0, speed*Time.deltaTime,ForceMode.VelocityChange);
            }
            
            
            
            //-------------old Script for moving-------------

            //RB.AddForce(0, 0, speed*Time.deltaTime,ForceMode.VelocityChange);
            //if (RB.velocity.z > maxSpeed)
            //{
            //  RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, maxSpeed);
            //}
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);


        }


        if (jump == true && isGrounded==true)
        {
            //RB.AddRelativeForce(Vector3.up * jumpY);
            
            RB.velocity = Vector3.up * jumpY*Time.fixedDeltaTime;
            jump = false;
        }

        //------------Swipe Jumping--------------

        //void SwipeCheck()
        //{
        //    if (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //        startTouchPosition = Input.GetTouch(0).position;

        //    if (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        endTouchPosition = Input.GetTouch(0).position;
        //        if (endTouchPosition.y > startTouchPosition.y && RB.velocity.y == 0)
        //            jump = true;
        //        jumpParticle.Play();
        //    }
        //}

        

        if (isGrounded == false)
        {
            
            startParticle = true;
            // StartCoroutine(LandingEnDe());
           
        }

        //-------------------Better Jump--------------------



        if (RB.velocity.y < 0 /*&& isGrounded == false*/)
        {

            RB.AddRelativeForce(Vector3.up * (fallMultiplier - 1) * Time.fixedDeltaTime * Physics.gravity.y, ForceMode.Impulse);
            //RB.velocity += Vector3.up * Time.fixedDeltaTime * (fallMultiplier-1) * Physics.gravity.y;

        }
        //else if(RB.velocity.y > 0 && !jump)
        //{
        //    RB.velocity += Vector3.up * Time.fixedDeltaTime * (lowJumpMultiplier-1) * Physics.gravity.y;

        //}


        //-----------------------------------------------------




        if (GM.moveright == true )
        {

            //RB.AddForce(speedRL * Time.deltaTime, 0, 0);
            
            transform.Translate(Vector3.right * speedR * Time.deltaTime);
            //swipeAllowed = false;

        }
   
        if (GM.moveleft == true )
        {

            
            
            transform.Translate(Vector3.left* speedL * Time.deltaTime);
            //swipeAllowed = false;

        }
        //if(GM.moveright == false && GM.moveleft == false)
        //{
        //    swipeAllowed = true;
        //}
       
    }
    

     public void OnJump()
    {
        jump = true;
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            if (startParticle == true)
            {
                landingParticle.Play();
                startParticle = false;
            }
            isGrounded = true;
            

        }
        else
        {
            
            isGrounded = false;
            
            
        }
    }private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false;

          

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GM.gameAudio.PlayOneShot(GM.levelFinishAudio);
            GM.gameplayBGM.Pause();
            //GM.trailParticle.gameObject.SetActive(false);
            StartCoroutine(FinishDelay_());
        }
        if (other.CompareTag("Reset"))
        {

            //speed = 0;
            RB.isKinematic = true;
            //GM.trailParticle.gameObject.SetActive(false);
            GM.fireParticle.gameObject.SetActive(true);
            StartCoroutine(DeathDelay());
        }
        if (other.CompareTag("Coin"))
        {
            coinPos = transform.position; /*other.gameObject.transform.position;*/
            coinRot =transform.rotation;
            
            Destroy(other.gameObject);
            Instantiate(GM.plusOne.gameObject,new Vector3 (coinPos.x,coinPos.y+3.66f,coinPos.z+16.5f),coinRot);
           
           
            GM.buttonAudioSource.PlayOneShot(GM.coinSound);
            GM.CoinCounter();
        }
        if (other.CompareTag("Devil"))
        {
            
            GM.gameAudio.PlayOneShot(GM.devilSlash);
            //speed = 0;
            RB.isKinematic = true;
            //GM.trailParticle.gameObject.SetActive(false);
            GM.fireParticle.gameObject.SetActive(true);
            StartCoroutine(DeathDelay());

        }
        if(other.CompareTag("SlowMo"))
        {
            Time.timeScale = 0.3f;
            speed = 200;

            GM.fireParticle.gameObject.SetActive(true);
        }
        if(other.CompareTag("Obstacle"))
        {
            //speed = 0;
            RB.isKinematic = true;
            //GM.trailParticle.gameObject.SetActive(false);
            //GM.trailParticle.gameObject.SetActive(false);
            GM.fireParticle.gameObject.SetActive(true);
            StartCoroutine(DeathDelay());
            

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowMo"))
        {
            Time.timeScale = 1f;
            speed = 200;
        }
            
        

    }
    

    

    IEnumerator FinishDelay_()
    {

        //speed = 0;
        RB.isKinematic = true;
        yield return new WaitForSeconds(2f);
        GM.touchControlsP.gameObject.SetActive(false);
        GM.levelFinish.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + GameManager.instance.coinadd);



    }
    bool startplayer=false;
    void GetReady()
    {
        speed = 38f;
        startplayer = true;
       
    }
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2f);
        GM.gameAudio.PlayOneShot(GM.deathSound);
        GM.gameplayBGM.Pause();
        GM.touchControlsP.gameObject.SetActive(false);
        GM.deathP.gameObject.SetActive(true);
    }

}
