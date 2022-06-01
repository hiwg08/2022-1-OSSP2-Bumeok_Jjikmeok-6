using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    [SerializeField]
    float pursuitSpeed;

    [SerializeField]
    float wanderSpeed;

    [SerializeField]
    float directionChangeInterval;

    [SerializeField]
    bool followPlayer;

    IEnumerator move;

    IEnumerator wander_routine;

    Animator animator;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector3 endPosition;
    int randomAngle = 0;
    float currentSpeed;
    // Start is called before the first frame update

    void Init_Start()
    {
        currentSpeed = wanderSpeed;
        if (TryGetComponent (out Rigidbody2D RB) && TryGetComponent (out SpriteRenderer SR))
        {
            rb = RB;
            spriteRenderer = SR;
        }
        if (TryGetComponent(out Animator A))
            animator = A;
    }
    public Color Get_Color()
    {
        return spriteRenderer.color;
    }

    private void Awake()
    {
        Init_Start();
    }
    void Start()
    {
        //animator = GetComponent<Animator>(); // TBD
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
    }
    // Student�� �ڷ�ƾ ���� : WanderRoutine -> Move
    //                         Change_Lerp_Color (�� �߿���)
    //                          
    public void Disappear()
    {
        StartCoroutine(Change_Color_Lerp(Color.white, Color.clear, 1.5f, 0.1f, null));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(rb.position, endPosition, Color.red); // Showing the direction and distance of an object
        rb.velocity = Vector3.zero;
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            if (move != null)
                StopCoroutine(move);

            move = Move(rb, currentSpeed);

            StartCoroutine(move);
            yield return YieldInstructionCache.WaitForSeconds(directionChangeInterval); // �� �ڵ� ����
        }
    }

    void ChooseNewEndpoint() // Setting a destination of an object
    {
        randomAngle = Random.Range(0, 361);
        endPosition = Vector3FromAngle(randomAngle);
    }
    public void Start_Move()
    {
        spriteRenderer.color = Color.white;
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
        animator.SetBool("Electric", false);
    }
    public void Stop_Move()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.95f);
        if (move != null)
            StopCoroutine(move);
        if (wander_routine != null)
            StopCoroutine(wander_routine);
        animator.SetBool("Electric", true);
    }
    public void When_Fever_End()
    {
        Stop_Move();
        StopAllCoroutines();
        Init_Start();
        Start_Move();
    }

    Vector3 Vector3FromAngle(int inputAngleDegrees) // Changing the degree into the radian
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        int randNum = Random.Range(-13, 14);

        return new Vector3(randNum * Mathf.Cos(inputAngleRadians) + transform.position.x, transform.position.y, 0);
    }
    public Color get_Color()
    {
        return spriteRenderer.color;
    }
    public void Stop_Coroutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed) // Acutual movement of an object according to the value of an endPosition
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        int key;
        while (remainingDistance > float.Epsilon)
        {
            if (rigidBodyToMove != null) // Checking whether an object has rigidbody2D
            {
                
                if (endPosition.x - rigidBodyToMove.position.x > 0)
                    key = -1;
                else
                    key = 1;
                transform.localScale = new Vector3(-key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        //animator.SetBool("isWalking",false);
    }
    public IEnumerator Change_Color_Lerp(Color Origin_C, Color Change_C, float time_persist, float Wait_Second, GameObject Effect)
    {
        if (Effect != null)
            Instantiate(Effect, transform.position, Quaternion.identity);
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
            yield return null;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }
}