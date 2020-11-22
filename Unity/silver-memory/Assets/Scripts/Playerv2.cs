using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerv2: MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager gameManager;

    public bool carryingEle = false;
    public Transform carryingJoint;
    private GameObject ptitEle;
    private float speed;
    public float normalSpeed;
    private Animator animator;

    Vector3 velocity;
    private float jumpHeight;
    public float normalJump;
    public bool isGrounded;

    public GameObject projectile;
    private Rigidbody rb;

    public int life = 10;
    private float distToGround;
    private CapsuleCollider col;
    private CapsuleCollider initCol;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        initCol = col;
        distToGround = col.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Firing();
        Action2();

        if ((Input.GetButton("Fire3") && gameManager.lastCheckpoint != null) || life <= 0)
        {
            Death();
        }
    }
    private void LateUpdate()
    {
        if (life <= 0)
        {
            life = 1;
        }
    }

    private bool IsGrounded() {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void Moving()
    {
        float height = animator.GetFloat("Height");
        col.center = new Vector3(0, 0.6f * (1f - height) +  height * 1.2f, height * 0.3f);
        col.height = 1.3f * (1f - height) + height * 0.8f;
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;

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

        isGrounded = IsGrounded();

        if (isGrounded)
        {
            if (animator.GetBool("Jumping")) animator.SetBool("Jumping", false);
            rb.AddForce(new Vector3(horizontal * speed * Time.deltaTime, 0, 0), ForceMode.Impulse);
            animator.SetBool("Moving", Mathf.Abs(rb.velocity.x) > 0.1f);
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jump");
                animator.SetTrigger("Jump");
            }
            if(Mathf.Abs(horizontal) > 0.1f) transform.rotation = Quaternion.Euler(0f, direction.x * 90, 0f);
        }
        else
        {
            if (!animator.GetBool("Jumping")) animator.SetBool("Jumping", true);
        }
    }

    private void Firing()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void Action2()
    {
        Debug.Log("Action2");
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Fire2");
            int layerMask = ~(1 << 9 | 1 << 10 | 1 << 11);
            if (carryingEle)
            {
                Debug.Log("Dropping ele");
                animator.SetTrigger("Grab");
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
                            Debug.Log("Grabbing elephant");
                            ptitEle = actionCheck;
                            this.carryingEle = true;
                        }
                    }
                }
            }
        }
    }

    private void Death()
    {
        this.transform.position = new Vector3(gameManager.currentCheckpoint.transform.position.x, gameManager.currentCheckpoint.transform.position.y, this.transform.position.z);

        //this.life = 1;
    }

    public void SnapEle()
    {
        if (this.carryingEle)
        {
            ptitEle.transform.parent = carryingJoint;
            animator.SetBool("Carrying", true);
            ptitEle.transform.localRotation = Quaternion.identity;
            ptitEle.transform.localPosition = Vector3.zero;
            ptitEle.GetComponent<Rigidbody>().isKinematic = false;
            ptitEle.GetComponent<Rigidbody>().useGravity = false;
            ptitEle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            animator.SetBool("Carrying", false);
            ptitEle.transform.parent = null;
            ptitEle.transform.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
            ptitEle.GetComponent<Rigidbody>().useGravity = true;
            ptitEle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    public void Jump()
    {
        rb.AddForce(new Vector3(0, jumpHeight, 0));
    }
    public void Attack()
    {
        GameObject firedProjectile = Instantiate(projectile, this.transform.position + this.transform.localScale.z / 3 * this.transform.forward + 0.8f * this.transform.localScale.y * Vector3.up, Quaternion.identity);
        firedProjectile.GetComponent<Rigidbody>().AddForce(70f * this.transform.forward.normalized, ForceMode.Impulse);
        Destroy(firedProjectile, 10);
    }
}
