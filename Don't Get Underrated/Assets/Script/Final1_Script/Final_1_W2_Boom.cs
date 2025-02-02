using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_1_W2_Boom : Weapon_Player
{
    [SerializeField]
    AnimationCurve declineCurve;

    private new void Awake()
    {
        base.Awake();
        if (GameObject.Find("Weapon_Effect_Sound") && GameObject.Find("Weapon_Effect_Sound").TryGetComponent(out AudioSource AS))
            EffectSource = AS;
    }
    void Start()
    {
        StartCoroutine(MoveToCenter());
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = new Vector3(0, 7.7f, 0);
        Vector3 endPosition = Vector3.zero;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1.5f;
            transform.position = Vector3.Lerp(startPosition, endPosition, declineCurve.Evaluate(percent));
            yield return null;
        }
        StartCoroutine(OnBoom());
    }
    IEnumerator OnBoom() // ���� �������� (1) - ��ź�� ���
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] devil_weapon = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");

        foreach (var e in meteor)
            Destroy(e);

        foreach (var e in enemy)
        {
            if (e.TryGetComponent(out F1_Homming_Enemy f1))
                f1.OnDie();
        }
        foreach (var e in devil_weapon)
        {
            if (e.TryGetComponent(out Weapon_Devil WD))
                WD.Weak_Weapon();
        }
        if (boss != null)
        {
            if (boss.TryGetComponent(out Boss_Info B))
                B.TakeDamage(200.0f);
        }
        Instantiate(Explosion, Vector3.zero, Quaternion.identity);
        spriteRenderer.color = Color.clear;

        Effect_Sound_OneShot(0);
        yield return Camera_Shake_And_Wait(0.02f, 2f, true, false);
        Destroy(gameObject);
    }
}