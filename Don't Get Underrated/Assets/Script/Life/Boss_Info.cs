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
    protected GameObject DisAppear_Effect_1; // 상위

    [SerializeField]
    protected GameObject DisAppear_Effect_2; // 상위

    [SerializeField]
    protected TextMeshProUGUI WarningText; // 상위

    protected IEnumerator phase; // 상위

    protected ArrayList Pattern_Total; // 상위

    public float speed = 15; // 둘다
    public float rotateSpeed = 200f; // 상위

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
   
    
   
    protected IEnumerator Position_Curve(Vector3 start_location, Vector3 standard_location, Vector3 last_location, float standard_time_ratio, AnimationCurve curve)
    {
        float percent = 0;

        float Length = Vector3.Distance(start_location, last_location);
        float journeyTime = standard_time_ratio * Vector3.Distance(standard_location, last_location) / Length; // 3차원 좌표 (-7, -4, 0), (7, 0, 0) 사이를 선형보간하는 시간을 1초로 기준을 두고 계산

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
        yield return null; // 깔끔한 정지를 위해 한 프레임 넘겨줌
    }

}
