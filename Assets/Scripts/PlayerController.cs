using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField] private float speed; //Movement Speed
    [SerializeField] Text timerText;
    [SerializeField] private Text winText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private int shotSpeed; //Speed at which bullets travel
    [SerializeField] private GameObject bulletPrefab; //For bullet instantiation
    [SerializeField] private float fireDelay; //Minimum time inbetween bullet shots

    private string BIGTAG = "Pickup"; //To check if player was hit by a pickup when it collides with something
    private string LILTAG = "LilPickup"; //To check if player was hit by a pickup when it collides with something
    private string SCENE = "SampleScene"; //Current scene for restarting the game
    private bool gameOverStatus = false; //bool to track if the game has ended
    private float lastFire; //variable for checking if enough time has passed since the last shot to fire again
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //Setting up the UI elements for the game start
        timerText.text = "Timer: 60";
        winText.text = "";
        restartButton.gameObject.SetActive(false);
        gameOverText.text = "";
    }

    float timer = 0.0f;
    void Update()
    {
        //If game not over
        if(!gameOverStatus)
        {
            //timer that counts up from 0 to 60
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            //if 60 seconds have passed
            if (seconds == 0 && timer > 60)
            {
                //End game, display congratulations message and restart button
                gameOverStatus = true;
                winText.text = "You Win!";
                restartButton.gameObject.SetActive(true);
                //This was the only way I could get the timer to display 0 instead of looping back to 60 at the end
                seconds = 60;
            }
            //Display time left
            timerText.text = "Timer: "+ (60 - seconds).ToString();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If game not over, take input and apply physics
        if (!gameOverStatus)
        {
            //Get movement input
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            //Get shooting input
            float shootHorizontal = Input.GetAxisRaw("ShootHorizontal");
            float shootVertical = Input.GetAxisRaw("ShootVertical");

            Vector2 shootVector = new Vector2(shootHorizontal, shootVertical);
            Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

            //If player is pressing the shoot button, and [fireDelay] time has passed since lastFire
            if (shootVector != Vector2.zero && Time.time > lastFire + fireDelay)
            {
                //Spawn bullet prefab from player
                GameObject bullet = Instantiate(bulletPrefab, shootVector + new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                //Give bullet velocity in direction player input
                bullet.GetComponent<Rigidbody2D>().velocity = shootVector * shotSpeed;
                //save fire time to lastFire
                lastFire = Time.time;
            }

            //Set player character's rigidbody velocity to the movement input from player (normalized so diagonal movement is not fast) * speed
            rb2d.velocity = (movement.normalized * speed);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag(TAG))
    //     {
    //         other.gameObject.SetActive(false);
    //         count++;
    //         countText.text = "Count: "+ count.ToString();
    //         if (count >= 12)
    //         {
    //             winText.text = "You Win!";
    //             restartButton.gameObject.SetActive(true);
    //         }
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //If collision object is a Pickup object
        if (other.gameObject.CompareTag(BIGTAG) || other.gameObject.CompareTag(LILTAG))
        {
            //End the game, display loss message and restart button
            gameOverStatus = true;
            gameOverText.text = "Game Over";
            restartButton.gameObject.SetActive(true);
        }
    }

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene(SCENE);
    }
}
