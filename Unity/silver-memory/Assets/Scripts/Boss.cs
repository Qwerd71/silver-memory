using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    private bool activated = false;
    public int baseLife = 20;
    public int life = 20;
    private Animator animator;

    public float attackTimer;

    public GameObject rock;
    private bool coroutineStarted = false;
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        life = baseLife;
    }

    // Update is called once per frame
    void Update()
    {
        // Surement bouger cette condition dans gameManager, sinon le boss sera tout le temps affiché
        if(Vector3.Distance(this.transform.position,gameManager.player.gameObject.transform.position) < 100f && !activated)
        {
            if (!gameManager.player.carryingEle)
            {
                Debug.Log("Vous êtes arrivé au boss, il s'active");
                this.transform.position += new Vector3(-70, 0);
                activated = true;
                gameManager.BossScene();
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
        if(activated && life > 0 && !coroutineStarted)
        {
            coroutineStarted = true;
            int random = Random.Range(0, 3);
            if (0 == random)
            {
                //Debug.Log("attack 0");
                coroutineStarted = false;
            }
            else if(random == 1)
            {
                StartCoroutine(AttackMove1());
            }
            else if (random == 2)
            {
                StartCoroutine(AttackMove2());
            }
        }
        else if (life <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }
    private IEnumerator AttackMove1()
    {
        animator.SetTrigger("Attack1");
        //Debug.Log("1");
        for (int i = -2; i<3; i ++)
        {
            GameObject falling = Instantiate(rock, new Vector3(gameManager.player.transform.position.x + 7*i, this.transform.position.y + 7 * this.transform.localScale.y + Random.Range(-2,15), gameManager.player.transform.position.z), Quaternion.identity);
            Destroy(falling, 15f);
        }
        yield return new WaitForSeconds(attackTimer);
        coroutineStarted = false;
    }
    private IEnumerator AttackMove2()
    {
        animator.SetTrigger("Attack2");
        for (int i = -2; i < 3; i++)
        {
            GameObject rising = Instantiate(rock, new Vector3(gameManager.player.transform.position.x + 7 *i, this.transform.position.y - 2 * this.transform.localScale.y - Random.Range(-2, 15), gameManager.player.transform.position.z), Quaternion.identity);
            Rigidbody rb = rising.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(9.81f * 4 * Vector3.up, ForceMode.Impulse);
            Destroy(rising, 15f);
        }
        yield return new WaitForSeconds(attackTimer);
        coroutineStarted = false;
    }
}
