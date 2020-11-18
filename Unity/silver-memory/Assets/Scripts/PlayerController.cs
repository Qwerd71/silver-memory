using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CharacterController controller = null;

    public GameManager gameManager;

    public bool carryingEle = false;
    private GameObject ptitEle;
    private float speed = 6f;

    Vector3 velocity;
    public float gravity = -9.81f;
    private float jumpHeight = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public GameObject projectile;

    public int life = 10;

    // Update is called once per frame
    void Update()
    {
        Moving();
        Firing();
        Action2();
        CheckPoint();
        if(life <= 0 || this.transform.position.y < -10)
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
            speed = 2f;
            jumpHeight = 0.7f;
        }
        else
        {
            speed = 6f;
            jumpHeight = 1.5f;
        }

        if (direction.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, direction.x * 90, 0f);
            controller.Move(direction * speed * Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Firing()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject firedProjectile = Instantiate(projectile, this.transform.position + this.transform.forward + 0.8f *  Vector3.up, Quaternion.identity);
            firedProjectile.GetComponent<Rigidbody>().AddForce(12f * this.transform.forward.normalized, ForceMode.Impulse);
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
                ptitEle.transform.parent = null;
                ptitEle.transform.position -= this.transform.up - 1.5f* this.transform.forward;
                this.carryingEle = false;
            }
            else if (Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition),2f, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                Debug.Log(hit.collider.name);
                GameObject actionCheck = hit.collider.gameObject;
                if (Vector3.Distance(this.transform.position, actionCheck.transform.position) < 2f)
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
                            Debug.Log("Grabbing elephant");
                            actionCheck.transform.parent = this.transform;
                            actionCheck.transform.position = this.transform.position + this.transform.up + new Vector3(0,actionCheck.transform.position.y);
                            ptitEle = actionCheck;
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
