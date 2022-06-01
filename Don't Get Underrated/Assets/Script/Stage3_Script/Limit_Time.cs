using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Limit_Time : MonoBehaviour
{
    TextMeshProUGUI Limit_Time_Text;
    Image image;

    ImageColor flashOn;

    IEnumerator flash_on;

    IEnumerator descent_time;

    [SerializeField]
    GameObject Time_Over_Text;

    [SerializeField]
    int Option_Limit_Time;

    int wow_Time;

    private void Awake()
    {
        Limit_Time_Text = GetComponent<TextMeshProUGUI>();
        wow_Time = 100;
        Limit_Time_Text.text = "���ѽð� : " + wow_Time;
        image = GameObject.Find("Flash_TimeOut").GetComponent<Image>();
        flashOn = image.GetComponent<ImageColor>();
    }

    private void Start()
    {
        descent_time = Descent_Time();
        StartCoroutine(descent_time);
    }

    IEnumerator Descent_Time()
    {
        while (true)
        {
            Debug.Log(wow_Time);
            if (wow_Time <= 10)
            {
                if (flash_on != null)
                    flashOn.StopCoroutine(flash_on);
                flash_on = flashOn.Change_Origin_BG(new Color(1, 0, 0, 0.5f), .5f);
                flashOn.StartCoroutine(flash_on);
            }
            if (wow_Time <= 0)
            {
                Player_Stage3 playerCtrl_Sarang;

                if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
                    playerCtrl_Sarang = PC_S;
                else
                    yield break;

                playerCtrl_Sarang.StopAllCoroutines();
                Debug.Log("�� ���ѽð�");
                playerCtrl_Sarang.Destroy_sliderClone();

                GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] q = GameObject.FindGameObjectsWithTag("Student");
                GameObject ww = GameObject.Find("Student_Gaze");
                GameObject rr = GameObject.Find("Targetting_Object");

                if (ww != null)
                    Destroy(ww);

                if (rr != null)
                    Destroy(rr);
                    
                foreach (var u in e)
                {
                    if (u.TryGetComponent(out Interrupt I1))
                    {
                        I1.Stop_Coroutine();
                        I1.Disappear();
                    }
                }
                
                foreach (var u in q)
                {
                    if (u.TryGetComponent(out Student S1))
                    {
                        S1.Stop_Coroutine();
                        S1.Disappear();
                    }
                }

                yield return YieldInstructionCache.WaitForSeconds(3f);
                Instantiate(Time_Over_Text, new Vector3(playerCtrl_Sarang.transform.position.x, playerCtrl_Sarang.transform.position.y + 2, 0), Quaternion.identity);
                yield break;
                // �� ��ȯ 
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            wow_Time -= 1;
        }
    }
    public void Stop_Time_Persist()
    {
        wow_Time += 1;
        if (descent_time != null)
            StopCoroutine(descent_time);
    }

    public void Final_Walk_Floor()
    {
        descent_time = Descent_Time();
        StartCoroutine(descent_time);
    }

    void Update()
    {
        Limit_Time_Text.text = "���ѽð� : " + wow_Time;
    }
}