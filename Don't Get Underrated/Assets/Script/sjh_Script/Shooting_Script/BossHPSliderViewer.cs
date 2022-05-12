using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSliderViewer : MonoBehaviour
{
    // Start is called before the first frame update

    Boss_Info boss_info;
    Slider slider;

    bool OnUpdate = false;


    public void F_HPFull(Boss_Info boss_info)
    {
        this.boss_info = boss_info;
        slider = GetComponent<Slider>();
        slider.value = 0;
        StartCoroutine("I_HPFull");
    }
    IEnumerator I_HPFull()
    {
        //Debug.Log(hp_info.CurrentHP);
        while(slider.value < 1)
        {
            slider.value += Time.deltaTime / 2;
            yield return null;
        }
        OnUpdate = true;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
         if (OnUpdate)
            slider.value = boss_info.CurrentHP / boss_info.MaxHP;
       
    }
}