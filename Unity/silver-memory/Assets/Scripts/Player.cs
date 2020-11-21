using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CharacterController controller = null;

    public GameManager gameManager;

    public bool carryingEle = false;
    public Transform carryingJoint; 
    private GameObject ptitEle;
    private float speed;
    public float normalSpeed;
    private Animator animator;

    Vector3 velocity;
    public float gravity = -9.81f;
    private float jumpHeight;
    public float normalJump;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public GameObject projectile;

    public int life = 10;
    private void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Firing();
        Action2();
        CheckPoint();
        if(life <= 0)
        {
            Death();
        }
    }
    private void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
        if (carryingEle)
        {
            speed = normalSpeed /3;
            jumpHeight = normalJump /2;
        }
        else
        {
            speed = normalSpeed;
            jumpHeight = normalJump;
        }

        if (direction.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, direction.x * 90, 0f);
            animator.SetBool("Moving", true);
            controller.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Moving",false);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Firing()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject firedProjectile = Instantiate(projectile, this.transform.position + this.transform.localScale.z / 3*this.transform.forward + 0.8f *  this.transform.localScale.y * Vector3.up, Quaternion.identity);
            firedProjectile.GetComponent<Rigidbody>().AddForce(70f * this.transform.forward.normalized, ForceMode.Impulse);
            Destroy(firedProjectile, 10);
        }
    }
    private void Action2()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            int layerMask = ~(1<< 9 | 1<<10);
            if (carryingEle)
            {
                Debug.Log("Dropping ele");
                animator.SetBool("Carrying", false);
                ptitEle.transform.localPosition = Vector3.forward;
                ptitEle.transform.parent = null;
                //ptitEle.transform.position -= ptitEle.transform.up - 1.5f* this.transform.forward;
                this.carryingEle = false;
            }
            else if (Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition),20f, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                GameObject actionCheck = hit.collider.gameObject;
                Debug.Log(hit.collider.name +" "+ Vector3.Distance(this.transform.position, actionCheck.transform.position));
                if (Vector3.Distance(this.transform.position, actionCheck.transform.position) < 20f)
                {
                    if (!carryingEle && actionCheck != null)
                    {
                        if (actionCheck.tag.StartsWith("Action"))
                        {
                            actionCheck.GetComponent<Actionable>().actioned = true;
                        }
                        else if (actionCheck.tag.StartsWith("Projectile"))
                        {
                            Destroy(actionCheck);
                        }
                        else if (actionCheck.tag.StartsWith("Ele"))
                        {
                            animator.SetBool("Carrying", true);
                            Debug.Log("Grabbing elephant");
                            actionCheck.transform.parent = carryingJoint; //this.transform.position + this.transform.up + new Vector3(0,actionCheck.transform.position.y);
                            ptitEle = actionCheck;
                            //ptitEle.transform.localPosition= Vector3.up;
                            ptitEle.transform.localRotation = Quaternion.identity;
                            ptitEle.transform.localPosition = Vector3.zero;
                            this.carryingEle = true; ;
                        }
                    }
                }
            }
        }
    }
    private void CheckPoint()
    {
        if (Input.GetButton("Fire3") && gameManager.lastCheckpoint != null)
        {
            this.transform.position = gameManager.lastCheckpoint.position;
        }
    }
    private void Death()
    {
        Debug.Log("U R dead");
        this.gameObject.SetActive(false);
    }
}
