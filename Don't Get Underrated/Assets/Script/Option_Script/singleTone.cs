using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class singleTone
{
    private static singleTone instance;

    public static UnityWebRequest request;

    public static int main_stage_1_score, main_stage_2_score, main_stage_3_score, final_stage_1_score, final_stage_2_score;

    public static string id;

    public static singleTone Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new singleTone();
            }
            return instance;
        }
    }

    public singleTone()
    {

    }

}