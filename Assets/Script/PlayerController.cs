using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 10f;
    public float gravity = -14f;
    public int PlayerHealth = 100;

    private Vector3 gravityVector;

    //GroundCheck
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundLayer;

    public bool isGrounded = false;
    public float jumpSpeed = 5f;

    //UI
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    void Start()
    {
        characterController= GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
        GroundCheck();
        JumpAndGravity(); 
    }

    void MovePlayer()
    {
        Vector3 moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        characterController.Move(moveVector * speed * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void JumpAndGravity()
    {
        gravityVector.y += gravity * Time.deltaTime;

        characterController.Move(gravityVector * Time.deltaTime);

        if (isGrounded && gravityVector.y < 0)
        {
            gravityVector.y = -3f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            gravityVector.y = jumpSpeed;   
        }
    }

    public void PlayerTakeDamage(int damageAmount)
    {
        PlayerHealth -= damageAmount;
        healthSlider.value -= damageAmount;
        HealthTextUpdate();
        if (PlayerHealth <= 0)
        {
            PlayerDeath();
            healthSlider.value = 0;
            HealthTextUpdate();
        }
    }

    public void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HealthTextUpdate()
    {
        healthText.text = PlayerHealth.ToString();
    }

}
