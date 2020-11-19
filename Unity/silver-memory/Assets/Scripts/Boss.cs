using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    private bool activated = false;
    public int life = 20;

    public GameObject rock;
    private bool coroutineStarted = false;

    // Update is called once per frame
    void Update()
    {
        // Surement bouger cette condition dans gameManager, sinon le boss sera tout le temps affiché
        if(Vector3.Distance(this.transform.position,gameManager.player.gameObject.transform.position) < 10f && ! gameManager.player.carryingEle && !activated)
        {
            Debug.Log("Vous êtes arrivé au boss, il s'active");
            activated = true;
        }
        if(activated && life > 0 && !coroutineStarted)
        {
            coroutineStarted = true;
            int random = Random.Range(0, 3);
            if (0 == random)
            {
                Debug.Log("attack 0");
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
            Destroy(this.gameObject);
        }
    }
    private IEnumerator AttackMove1()
    {
        GameObject falling = Instantiate(rock, new Vector3(gameManager.player.transform.position.x, 10, 0),Quaternion.identity);
        Destroy(falling, 5f);
        yield return new WaitForSeconds(0.5f);
        coroutineStarted = false;
    }
    private IEnumerator AttackMove2()
    {
        GameObject rising = Instantiate(rock, new Vector3(gameManager.player.transform.position.x, -10, 0), Quaternion.identity);
        Rigidbody rb = rising.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(9.81f * Vector3.up, ForceMode.Impulse);
        Destroy(rising, 5f);
        yield return new WaitForSeconds(0.5f);
        coroutineStarted = false;
    }
}
