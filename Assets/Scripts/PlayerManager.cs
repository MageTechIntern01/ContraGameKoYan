using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    public float resetX;
    public float resetY;
    public GameObject bulletRight;
    public GameObject bulletLeft;
    public GameObject bulletRightUp;
    public GameObject bulletLeftUp;
    public GameObject bulletRightDown;
    public GameObject bulletLeftDown;
    public GameObject bulletUp;
    public GameObject bulletDown;
    public GameObject bulletF;
    public GameObject bulletFLeft;
    public int shootingType;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    public float runningSpeed;
    private float speed;
    public float jumpSpeed;
    private Transform firePosition;
    private Transform firePositionW;
    private Transform firePositionS;
    private Transform firePositionWd;
    private Transform firePositionF;
    private float fireRate = 3f;
    float timeToFire = 0;
    public bool facingRight;
    private bool isDead = false;
    public int health;
    private bool readyToFall;
    private bool onWater;
    private bool onGround;
    private bool jump = true;
    public float gravity;
    private bool isVisible;
    public bool horizontalLevel;

    [Space]

    [Header("Mobile Controls")]
    
    [Space]

    // The reference for the mobile joystick.
    public Joystick joystick = null;

    [Tooltip("FOR MOBILE JOYSTICK SENSITIVITY. Can be adjusted through inspector. Capped at 1.")]
    [Range(0.1f, 1f)]
    public float mobileJoyStickSensitivity = 0.2f;

    // Bool if the player is holding or let gone of the shooting button.
    private bool isHoldingShootButton = false;
    
    // Bool if the player is holding or let gone of the crouching button.
    private bool isHoldingCrouchButton = false;



    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        firePositionW = transform.Find("FirePointOnlyW");
        firePositionS = transform.Find("FirePointS");
        firePositionWd = transform.Find("FirePointW");
        firePositionF = transform.Find("SpinningFirePoint");
        firePosition = transform.Find("FirePoint");
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        facingRight = true;
        isVisible = true;
       

    }

    // Update is called once per frame
    void Update() {

        if (!isDead) { 
            Move(speed);
            Flip();
        }

        if (Input.GetKey(KeyCode.A))
        {
            speed = -runningSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            speed = runningSpeed;
        }
        else
        {
            speed = 0;
        }

        // Mobile Controls
        if (joystick != null)
        {

            // If the player's joystick is pointing towards the RIGHT (positive) direction, set it to positive running speed. 
            if (joystick.Horizontal >= mobileJoyStickSensitivity)
            {
                speed = runningSpeed;
            }

            // If the player's joystick is pointing towards the LEFT (negative) direction, set it to negative running speed.
            else if (joystick.Horizontal <= -mobileJoyStickSensitivity)
            {
                speed = -runningSpeed;
            }

        }
        

        if (Input.GetKeyDown(KeyCode.Space) && !jump && !Input.GetKey(KeyCode.S) && !isDead
            && rigidbody2d.velocity.y == 0)
        {
            
            jump = true;
            rigidbody2d.AddForce(new Vector2(rigidbody2d.velocity.x, jumpSpeed));

            animator.SetBool("Ground", false);
        }

       

        if (readyToFall && Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) && !jump)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - gravity);
            jump = true;
            animator.SetBool("Ground", false);
           
        }



        ///////////////
        if (Input.GetKey("w"))
        {
            animator.SetBool("Up", true);
        }
        else
            animator.SetBool("Up", false);

        if (Input.GetKey("s"))
        {
            animator.SetBool("Down", true);
        }
        else
            animator.SetBool("Down", false);

        if (Input.GetKey("z") && !onWater && onGround)
        {
            animator.SetBool("Crouch", true);
            speed = 0;
          
        }
        else
            animator.SetBool("Crouch", false);

        // For mobile crouching.
        if (isHoldingCrouchButton && !onWater && onGround)
            speed = 0;
        
        // Sets the "Crouching" either TRUE or FALSE depending if the player is holding the crouching button or not.
        animator.SetBool("Crouch", isHoldingCrouchButton);
      
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2d.velocity.x));

        if (Input.GetKey("k") && Time.time > timeToFire && !isDead)
        {
            timeToFire = Time.time + 1 / fireRate;
            Fire();
            
        }

        if (Input.GetKey("k")){
            animator.SetBool("Shooting", true);
            isVisible = true;
        }

        else
            animator.SetBool("Shooting", false);

        // For mobile shooting.
        if (isHoldingShootButton && Time.time > timeToFire && !isDead)
        {
            timeToFire = Time.time + 1 / fireRate;
            MobileFire();
        }

        if (isHoldingShootButton)
            isVisible = true;

        // Sets the "Shooting" either TRUE or FALSE depending if the player is holding the shooting button or not.
        animator.SetBool("Shooting", isHoldingShootButton);

        if (onWater)
        {
            animator.SetBool("Water", true);
        }
        else
            animator.SetBool("Water", false);
    }

    private void Move(float speed)
    {
        rigidbody2d.velocity = new Vector3(speed, rigidbody2d.velocity.y, 0);
    }

    // Firing function for desktops.
    private void Fire()
    {
        if (shootingType == 0)
        {
            bool anyKey = false;


            if (Input.GetKey("w") && !Input.GetKey("d") && !Input.GetKey("a"))
            {
                Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                anyKey = true;
            }

            else if (Input.GetKey("s") && !Input.GetKey("d") && !Input.GetKey("a"))
            {
                Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                anyKey = true;
            }
            if (facingRight)
            {


                if (Input.GetKey("w") && Input.GetKey("d"))
                {
                    Instantiate(bulletRightUp, firePositionWd.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (Input.GetKey("s") && Input.GetKey("d"))
                {
                    Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (!anyKey)
                {
                    Instantiate(bulletRight, firePosition.position, Quaternion.identity);
                }
            }
            else
            {

                if (Input.GetKey("w") && Input.GetKey("a"))
                {
                    Instantiate(bulletLeftUp, firePositionWd.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (Input.GetKey("s") && Input.GetKey("a"))
                {
                    Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (!anyKey)
                {
                    Instantiate(bulletLeft, firePosition.position, Quaternion.identity);
                }
            }
        }
        if (shootingType == 1)
        {
          
            ////////////////////////////////////////////////////////
            bool anyKey = false;


            if (Input.GetKey("w") && !Input.GetKey("d") && !Input.GetKey("a"))
            {
                Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                Instantiate(bulletRightUp, firePositionW.position, Quaternion.identity);
                Instantiate(bulletLeftUp, firePositionW.position, Quaternion.identity);
                anyKey = true;
            }

            else if (Input.GetKey("s") && !Input.GetKey("d") && !Input.GetKey("a"))
            {
                Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                anyKey = true;
            }
            if (facingRight)
            {


                if (Input.GetKey("w") && Input.GetKey("d"))
                {
                    Instantiate(bulletRightUp, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletRight, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (Input.GetKey("s") && Input.GetKey("d"))
                {
                    Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletRight, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletDown, firePositionS.position, Quaternion.identity);

                    anyKey = true;
                }
                else if (!anyKey)
                {
                    Instantiate(bulletRight, firePosition.position, Quaternion.identity);
                    Instantiate(bulletRightUp, firePosition.position, Quaternion.identity);
                    Instantiate(bulletRightDown, firePosition.position, Quaternion.identity);
                }
            }
            else
            {

                if (Input.GetKey("w") && Input.GetKey("a"))
                {
                    Instantiate(bulletLeftUp, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletLeft, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                    
                    anyKey = true;
                }
                else if (Input.GetKey("s") && Input.GetKey("a"))
                {
                    Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletLeft, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (!anyKey)
                {
                    Instantiate(bulletLeft, firePosition.position, Quaternion.identity);
                    Instantiate(bulletLeftUp, firePosition.position, Quaternion.identity);
                    Instantiate(bulletLeftDown, firePosition.position, Quaternion.identity);
                }
            }
        }
        if(shootingType == 2)
        {
            if (facingRight)
            {
                Instantiate(bulletF, firePositionF.position, Quaternion.identity);
            }
            else
            {
                Instantiate(bulletFLeft, firePositionF.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GROUND" || collision.gameObject.tag == "Water" ||
            collision.gameObject.tag == "NoFalling") ;
        {
            jump = false;
            animator.SetBool("Ground", true);
        }
        
        if (collision.gameObject.tag == "GROUND" || collision.gameObject.tag == "NoFalling")
        {
            readyToFall = true;
            onGround = true;
            onWater = false;
        }

        if (collision.gameObject.tag == "Water" && readyToFall || collision.gameObject.tag == "NoFalling")
        {
            readyToFall = false;
        }

        if (collision.gameObject.tag == "Water")
        {
            onWater = true;
            onGround = false;
        }

        if((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "DeathLayer" ||
            collision.gameObject.tag == "EnemiesBullet") && isVisible)
        {
            GetComponent<AudioSource>().Play();
            animator.SetBool("Dead", true);
            boxCollider.isTrigger = true;
            circleCollider.isTrigger = true;
            rigidbody2d.simulated = false;
            isVisible = false;
            isDead = true;
            health--;
        
            if(health == 0)
            {

                Invoke("GameOver", 2f);
                

            }
            if(health !=0)
            {
                Invoke("RestartOne", 1f);
                Invoke("Restart", 2f);
                Invoke("MakeVisible", 5f);
                
            }
        }
        if(collision.gameObject.tag == "PowerUpB")
        {
            shootingType = 0;
        }
        if (collision.gameObject.tag == "PowerUpF")
        {
            shootingType = 1;
        }
        if (collision.gameObject.tag == "PowerUpM")
        {
            shootingType = 2;
        }


    }
  
    
    private void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            
        }
    }

    private void Restart()
    {
        animator.SetBool("Dead", false);
        boxCollider.isTrigger = false;
        circleCollider.isTrigger = false;
        rigidbody2d.simulated = true;

    }

    private void RestartOne()
    {
        if (horizontalLevel)
        {
            transform.position = new Vector3(transform.position.x - resetX, -0.8f, 0);
        }
        else
        {
             transform.position = new Vector3(transform.position.x - resetX, transform.position.y + resetY, 0);
        }
        isDead = false;
        animator.SetBool("Dead", false);
        
    }
    private void MakeVisible()
    {
        isVisible = true;
    }
    
    private void GameOver()
    {
        SceneManager.LoadScene(3);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            GetComponent<PlayerManager>().enabled = false;
        }
    }

    // An new function to handle the jumping button in mobiles.
    public void MobileJump()
    {

        if (!jump && !isDead && rigidbody2d.velocity.y == 0)
        {
            
            jump = true;
            rigidbody2d.AddForce(new Vector2(rigidbody2d.velocity.x, jumpSpeed));

            animator.SetBool("Ground", false);
        }

        if (readyToFall && !jump)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - gravity);
            jump = true;
            animator.SetBool("Ground", false);
           
        }

    }

    // A new function to handle the firing button in mobiles.
    // Player can shoot in 6 directions. (Above, Below, Topright, Topleft, Bottomright, Bottomleft).
    private void MobileFire()
    {
        if (shootingType == 0)
        {
            bool anyKey = false;

            // Fire Above
            if (joystick.Vertical >= mobileJoyStickSensitivity)
            {
                Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                anyKey = true;
            }

            // Fire Below
            else if (joystick.Vertical <= -mobileJoyStickSensitivity)
            {
                Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                anyKey = true;
            }

            if (facingRight)
            {

                // Fire Topright (Above) && (Right)
                if ((joystick.Vertical >= mobileJoyStickSensitivity) && (joystick.Horizontal >= mobileJoyStickSensitivity))
                {
                    Instantiate(bulletRightUp, firePositionWd.position, Quaternion.identity);
                    anyKey = true;
                }

                // Fire Bottomright (Bottom) && (Right)
                else if ((joystick.Vertical <= -mobileJoyStickSensitivity) && (joystick.Horizontal >= mobileJoyStickSensitivity))
                {
                    Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }

                else if (!anyKey)
                {
                    Instantiate(bulletRight, firePosition.position, Quaternion.identity);
                }

            }

            else
            {

                // Fire Topleft (Above) && (Left)
                if ((joystick.Vertical >= mobileJoyStickSensitivity) && (joystick.Horizontal <= -mobileJoyStickSensitivity))
                {
                    Instantiate(bulletLeftUp, firePositionWd.position, Quaternion.identity);
                    anyKey = true;
                }

                // Fire Bottomleft (Bottom) && (Left)
                else if ((joystick.Vertical <= -mobileJoyStickSensitivity) && (joystick.Horizontal <= -mobileJoyStickSensitivity))
                {
                    Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }
                else if (!anyKey)
                {
                    Instantiate(bulletLeft, firePosition.position, Quaternion.identity);
                }
            }
        }

        if (shootingType == 1)
        {
          
            ////////////////////////////////////////////////////////
            bool anyKey = false;


            // Fire Topright (Above) && (Right)
            if ((joystick.Vertical >= mobileJoyStickSensitivity) && (joystick.Horizontal >= mobileJoyStickSensitivity))
            {
                Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                Instantiate(bulletRightUp, firePositionW.position, Quaternion.identity);
                Instantiate(bulletLeftUp, firePositionW.position, Quaternion.identity);
                anyKey = true;
            }

            // Fire Below
            else if (joystick.Vertical <= -mobileJoyStickSensitivity)
            {
                Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                anyKey = true;
            }

            if (facingRight)
            {


                // Fire Topright (Above) && (Right)
                if ((joystick.Vertical >= mobileJoyStickSensitivity) && (joystick.Horizontal >= mobileJoyStickSensitivity))
                {
                    Instantiate(bulletRightUp, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletRight, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                    anyKey = true;
                }
                
                // Fire Bottomright (Bottom) && (Right)
                else if ((joystick.Vertical <= -mobileJoyStickSensitivity) && (joystick.Horizontal >= mobileJoyStickSensitivity))
                {
                    Instantiate(bulletRightDown, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletRight, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletDown, firePositionS.position, Quaternion.identity);

                    anyKey = true;
                }

                else if (!anyKey)
                {
                    Instantiate(bulletRight, firePosition.position, Quaternion.identity);
                    Instantiate(bulletRightUp, firePosition.position, Quaternion.identity);
                    Instantiate(bulletRightDown, firePosition.position, Quaternion.identity);
                }
            }
            else
            {

                // Fire Topleft (Above) && (Left)
                if ((joystick.Vertical >= mobileJoyStickSensitivity) && (joystick.Horizontal <= -mobileJoyStickSensitivity))
                {
                    Instantiate(bulletLeftUp, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletLeft, firePositionWd.position, Quaternion.identity);
                    Instantiate(bulletUp, firePositionW.position, Quaternion.identity);
                    
                    anyKey = true;
                }

                // Fire Bottomleft (Bottom) && (Left)
                else if ((joystick.Vertical <= -mobileJoyStickSensitivity) && (joystick.Horizontal <= -mobileJoyStickSensitivity))
                {
                    Instantiate(bulletLeftDown, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletLeft, firePositionS.position, Quaternion.identity);
                    Instantiate(bulletDown, firePositionS.position, Quaternion.identity);
                    anyKey = true;
                }
                
                else if (!anyKey)
                {
                    Instantiate(bulletLeft, firePosition.position, Quaternion.identity);
                    Instantiate(bulletLeftUp, firePosition.position, Quaternion.identity);
                    Instantiate(bulletLeftDown, firePosition.position, Quaternion.identity);
                }
            }
        }
        
        if(shootingType == 2)
        {
            if (facingRight)
            {
                Instantiate(bulletF, firePositionF.position, Quaternion.identity);
            }
            else
            {
                Instantiate(bulletFLeft, firePositionF.position, Quaternion.identity);
            }
        }
    }
    
    // Enables the shooting once pressed and holding.
    public void OnHoldingShootingButton() => isHoldingShootButton = true;

    // Disables the shooting once released.
    public void OnReleasedShootingButton() => isHoldingShootButton = false;

    // Enables the crouching once pressed and holding.
    public void OnHoldingCrouchingButton() => isHoldingCrouchButton = true;

    // Disables the crouching once released.
    public void OnReleasedCrouchingButton() => isHoldingCrouchButton = false;



}
   


