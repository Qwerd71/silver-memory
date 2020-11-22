using System;
using System.Collections;
using System.Collections.Generic;
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

        
    }
    private void FixedUpdate()
    {
        if ((Input.GetButton("Fire3") || life <= 0) && gameManager.currentCheckpoint != null)
        {
            Death();
        }
        if (life <= 0)
        {
            life = 1;
            gameManager.PlayerDeath();
        }
    }
    private void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (carryingEle)
        {
            speed = normalSpeed / 3;
            jumpHeight = normalJump / 2;
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
            animator.SetBool("Moving", false);
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
            animator.SetTrigger("Attack");
            GameObject firedProjectile = Instantiate(projectile, this.transform.position + (this.transform.localScale.z / 2 * this.transform.forward) + (0.8f * this.transform.localScale.y * Vector3.up), Quaternion.identity);
            firedProjectile.GetComponent<Rigidbody>().AddForce(70f * this.transform.forward.normalized, ForceMode.Impulse);
            Destroy(firedProjectile, 10);
        }
    }
    private void Action2()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            int layerMask = ~(1 << 9 | 1 << 10 | 1 << 11);
            if (carryingEle)
            {
                Debug.Log("Dropping ele");
                animator.SetBool("Carrying", false);
                ptitEle.GetComponent<Rigidbody>().useGravity = true;
                ptitEle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                ptitEle.transform.localPosition = Vector3.forward - Vector3.right;
                ptitEle.transform.parent = null;
                //ptitEle.transform.localRotation = Quaternion.identity;
                //ptitEle.transform.position -= ptitEle.transform.up - 1.5f* this.transform.forward;
                this.carryingEle = false;
            }
            else if (Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition), 20f, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                GameObject actionCheck = hit.collider.gameObject;
                Debug.Log(hit.collider.name + " " + Vector3.Distance(this.transform.position, actionCheck.transform.position));
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
                            animator.SetTrigger("Grab");
                            animator.SetBool("Carrying", true);
                            Debug.Log("Grabbing elephant");
                            actionCheck.transform.parent = carryingJoint; //this.transform.position + this.transform.up + new Vector3(0,actionCheck.transform.position.y);
                            ptitEle = actionCheck;
                            //ptitEle.transform.localPosition= Vector3.up;
                            ptitEle.transform.localRotation = Quaternion.identity;
                            ptitEle.transform.localPosition = Vector3.zero;
                            ptitEle.GetComponent<Rigidbody>().isKinematic = false;
                            ptitEle.GetComponent<Rigidbody>().useGravity = false;
                            ptitEle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            this.carryingEle = true;
                        }
                    }
                }
            }
        }
    }
    private void Death()
    {
        this.transform.position = new Vector3(gameManager.currentCheckpoint.transform.position.x, gameManager.currentCheckpoint.transform.position.y, this.transform.position.z); ;
    }
}
