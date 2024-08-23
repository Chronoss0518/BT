using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimer : MonoBehaviour
{
    public ChUnity.Common.Event.AlarmEvent alarm = null;
    public ChUnity.Common.CountDownEvent countDown = null;

    public TMPro.TextMeshProUGUI text = null;

    // Update is called once per frame
    void Update()
    {
        if (alarm == null) return;
        if (text == null) return;
        if (countDown == null) return;

        if(countDown.getNowCount > 0)
        {
            text.text = "You Can Fire";
            return;
        }

        int nowTime = alarm.nowMiliseconds;

        nowTime = (nowTime / 10);

        text.text = "Reload Time \n" + (alarm.alarmSeconds - (float)(nowTime / 100.0f)).ToString("0.00");

    }
}
