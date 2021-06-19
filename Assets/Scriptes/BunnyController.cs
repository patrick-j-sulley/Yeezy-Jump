using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BunnyController : MonoBehaviour
{

    private Rigidbody2D myRigidBody;
    private Animator myAnim;
    public float yeezyJumpForce = 500f;
    private float yeezyHurtTime = -1;
    private Collider2D myCollider;
    public Text scoreText;
    private float startTime;
    private int jumpsLeft = 2;
    public AudioSource jumpSfx;
    public AudioSource deathSfx;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }

        if (yeezyHurtTime == -1)
        {

            if ((Input.GetButtonUp("Jump") || Input.GetButtonUp("Fire1")) && jumpsLeft > 0)
            {
                if (myRigidBody.velocity.y < 0)
                {

                    myRigidBody.velocity = Vector2.zero;

                }
                if (jumpsLeft == 1)
                {

                    myRigidBody.AddForce(transform.up * yeezyJumpForce * 0.75f);

                }
                else
                {

                    myRigidBody.AddForce(transform.up * yeezyJumpForce);

                }
                jumpsLeft--;

                jumpSfx.Play();
            }

            myAnim.SetFloat("vVelocity", Mathf.Abs(myRigidBody.velocity.y));
            scoreText.text = (Time.time - startTime).ToString("0.0");

        }
        else
        {

            if (Time.time > yeezyHurtTime + 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {


            foreach (PrefabSpawner spawner in FindObjectsOfType<PrefabSpawner>())
            {
                spawner.enabled = false;
            }

            foreach (MoveLeft moveLefter in FindObjectsOfType<MoveLeft>())
            {
                moveLefter.enabled = false;
            }


            yeezyHurtTime = Time.time;
            myAnim.SetBool("YeezyHurt", true);
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.AddForce(transform.up * yeezyJumpForce);
            myCollider.enabled = false;

            deathSfx.Play();

            float currentBestScore = PlayerPrefs.GetFloat("BestScore", 0);
            float currentScore = Time.time - startTime;

            if (currentScore > currentBestScore)
            {

                PlayerPrefs.SetFloat("BestScore", currentScore);

            }
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            jumpsLeft = 2;


        }




    }
    } 
