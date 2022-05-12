using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss_Info : Life
{
    // Start is called before the first frame update

    [SerializeField]
    float maxHP = 100;

    [SerializeField]
    protected GameObject DisAppear_Effect_1; // ����

    [SerializeField]
    protected GameObject DisAppear_Effect_2; // ����

    [SerializeField]
    protected TextMeshProUGUI WarningText; // ����

    protected IEnumerator phase; // ����

    protected ArrayList Pattern_Total; // ����

    public float speed = 15; // �Ѵ�
    public float rotateSpeed = 200f; // ����

    private float currentHP;
    public float CurrentHP
    {
        set { currentHP = value; }
        get { return currentHP; }
    }
    public float MaxHP => maxHP;
    protected virtual new void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        Pattern_Total = new ArrayList();
    }
    public override void TakeDamage(float damage)
    {
        CurrentHP -= damage;
    }
    public override void OnDie()
    {
        Destroy(gameObject);
    }
   
    protected IEnumerator Warning(string warning_message, float time_ratio)
    {
        WarningText.text = warning_message;
        while (WarningText.color.a < 1.0f)
        {
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, WarningText.color.a + Time.deltaTime * time_ratio);
            yield return null;
        }
        while (WarningText.color.a > 0.0f)
        {
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, WarningText.color.a - Time.deltaTime * time_ratio);
            yield return null;
        }
    }
    protected IEnumerator Rotate_Bullet(float rot_Speed, float rot_Radius, float ratio, int Launch_Num, GameObject Bullet)
    {
        float percent = 0;
        int Index = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * ratio);
            transform.Rotate(Vector3.forward * rot_Speed * rot_Radius * Time.deltaTime);
            Index++;
            if (Index >= Launch_Num)
            {
                Index = 0;
                GameObject T1 = Instantiate(Bullet);
                T1.transform.position = transform.position;
                T1.transform.rotation = transform.rotation;
            }
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return YieldInstructionCache.WaitForEndOfFrame;
    }
    protected IEnumerator Circle_Move(int Degree, int is_ClockWise_And_Speed, float Start_Degree, float x, float y, float start_x, float start_y, float ratio)
    {
        // ����, �ð�/�ݽð� ����, x�� ��, y�� ��

        for (int th = 0; th < Degree; th++)
        {

            float rad = Mathf.Deg2Rad * (is_ClockWise_And_Speed * th + Start_Degree);

            float rad_x = x * Mathf.Sin(rad);
            float rad_y = y * Mathf.Cos(rad);

            transform.position = new Vector3(start_x + rad_x, start_y + rad_y, 0);
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * ratio);
        }
        yield return null;
    }
    
   
    protected IEnumerator Position_Curve(Vector3 start_location, Vector3 standard_location, Vector3 last_location, float standard_time_ratio, AnimationCurve curve)
    {
        float percent = 0;

        float Length = Vector3.Distance(start_location, last_location);
        float journeyTime = standard_time_ratio * Vector3.Distance(standard_location, last_location) / Length; // 3���� ��ǥ (-7, -4, 0), (7, 0, 0) ���̸� ���������ϴ� �ð��� 1�ʷ� ������ �ΰ� ���

        while (true)
        {
            if (percent / journeyTime >= 1)
                break;
            percent += Time.deltaTime;
            Vector3 center = (start_location + last_location) * 0.5f;
            center -= new Vector3(0, -7f, 0);
            Vector3 riseRelCenter = start_location - center;
            Vector3 setRelCenter = last_location - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent / journeyTime));

            transform.position += center;
            yield return null;
        }
        yield return null; // ����� ������ ���� �� ������ �Ѱ���
    }
    protected IEnumerator Size_Change(float size_ratio, int inc_or_dec)

    {
        while (true)
        {
            transform.localScale = new Vector3(transform.localScale.x + (Time.deltaTime * size_ratio * inc_or_dec), transform.localScale.y + (Time.deltaTime * size_ratio * inc_or_dec), 0);
            yield return null;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}