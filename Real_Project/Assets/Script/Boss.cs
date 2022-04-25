using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BossState { Phase01 = 0, Phase02, Phase03 }

public class Boss : HP_Info
{
    // Start is called before the first frame update

    [SerializeField]
    float body_damage = 0.7f;

    [SerializeField]
    AnimationCurve curve;


    [SerializeField]
    GameObject Die_Explosion;

    float[,] Boss_Random_Move = new float[9, 3] { { 7, 2, 0 }, { -7, 2, 0 }, { -7, -2, 0 }, { 7, -2, 0 }, { 3.5f, 1, 0 },
    { -3.5f, 1, 0 }, { -3.5f, -1, 0 }, { 3.5f, 1, 0 }, { 0, 0, 0 }};

    PlayerControl playerControl;

    float boomDelay = 0.5f;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject[] Boss_Weapon;

    new private void Awake()
    {
        base.Awake();
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        CurrentHP = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().TakeDamage(body_damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        StartCoroutine("Hit");
        if (CurrentHP <= 0)
        {
            OnDie();
        }
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public void OnDie()
    {
        playerControl.Score += 10000;
        Instantiate(Die_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Phase_Start()
    {
        StartCoroutine("Repeat_Phase");
    }
    IEnumerator Repeat_Phase()
    {
        while(true)
        {
            yield return StartCoroutine("Phase01");
            yield return StartCoroutine("Phase02");
        }
    }
    IEnumerator Phase01()
    {
        yield return StartCoroutine(Boss_Move(Boss_Random_Move));
        
        Instantiate(Boss_Weapon[0], transform.position, Quaternion.identity);

        GameObject L2 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -60)));
        L2.GetComponent<Movement2D>().MoveTo(new Vector3(1, -0.5714f, 0));

        GameObject L3 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
        L3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

        GameObject L4 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, 120)));
        L4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));

        yield return new WaitForSeconds(2f);

        GameObject L5 = Instantiate(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L6 = Instantiate(Boss_Weapon[2], new Vector3(0, -4, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L7 = Instantiate(Boss_Weapon[3], new Vector3(-8, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));
        GameObject L8 = Instantiate(Boss_Weapon[3], new Vector3(6.6f, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));
        yield return new WaitForSeconds(2.5f);
        Destroy(L5); Destroy(L6); Destroy(L7); Destroy(L8);

        while (true)
        {
            transform.position += Vector3.right * (Time.deltaTime * 8f);
            yield return null;

            if (transform.position.x >= 7)
                yield break;
        }
    }
    IEnumerator Phase02()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);

            GameObject C2 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C2.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));

            GameObject C3 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

            GameObject C4 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.2857f, 0));

            GameObject C5 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C5.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.2857f, 0));

            yield return new WaitForSeconds(0.15f);
        }
        yield break;
    }
    IEnumerator Boss_Move(float[,] Boss_Move_float)
    {
        for (int i = 0; i < 9; i++)
        {
            float current = 0;
            float percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / boomDelay;

                transform.position = Vector3.Lerp(transform.position, new Vector3(Boss_Move_float[i, 0], Boss_Move_float[i, 1], Boss_Move_float[i, 2]), curve.Evaluate(percent));
                yield return null;
            }
        }
        yield break;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
