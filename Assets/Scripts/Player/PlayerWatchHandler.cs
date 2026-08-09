using System;
using System.Collections.Generic;
using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshPro))]
[RequireComponent(typeof(AudioSource))]
public class PlayerWatchHandler : Singleton<PlayerWatchHandler>
{
    [Serializable]
    private struct AlarmTimestamp
    {
        public int Hour;
        public int Minute;

        public static int Comparer(AlarmTimestamp a, AlarmTimestamp b)
        {
            if (a.Hour > b.Hour)
            {
                return -1;
            }

            if (b.Hour > a.Hour)
            {
                return 1;
            }

            if (a.Minute > b.Minute)
            {
                return -1;
            }

            if (b.Minute > a.Minute)
            {
                return 1;
            }
            return 0;
        }
    }

    [SerializeField]
    private List<AlarmTimestamp> alarms;
    [SerializeField]
    private float alarmTextFadeTime;
    [SerializeField]
    private float alarmDisableDelay;

    private TMPro.TextMeshPro text;
    private int hour;
    private int minute;
    private List<AlarmTimestamp>.Enumerator alarmsEnumerator;
    private bool checkAlarms;
    private AudioSource source;
    private bool alarmActive;
    private bool alarmDisabling;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
        alarms.Sort(AlarmTimestamp.Comparer);
        alarmsEnumerator = alarms.GetEnumerator();
        checkAlarms = alarmsEnumerator.MoveNext();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float shiftNormlizedTime = GameManager.Instance.shiftTime / GameManager.Instance.ShiftDuration;
        int watchTime = (int)((1 - shiftNormlizedTime) * 8f * 60f);
        hour = watchTime / 60 % 24;
        minute = watchTime % 60;
        text.text = $"{hour.ToString().PadLeft(2, '0')}\n{minute.ToString().PadLeft(2, '0')}";

        if (alarmActive && !alarmDisabling)
        {
            if (PlayerWatchWatcher.Instance.CanCheckTime())
            {
                if (!PlayerWatchWatcher.Instance.Watching)
                {
                    PlayerWatchWatcher.Instance.CheckTime();
                }
                DisableAlarm();
            }
        }

        if (!checkAlarms)
        {
            return;
        }

        if (hour <= alarmsEnumerator.Current.Hour && minute <= alarmsEnumerator.Current.Minute)
        {
            checkAlarms = alarmsEnumerator.MoveNext();

            if (!alarmActive && !alarmDisabling)
            {
                source.Play();
                text.DOFade(0, alarmTextFadeTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                alarmActive = true;
            }
        }
    }

    public void DisableAlarm()
    {
        alarmDisabling = true;
        this.CallDelayed(alarmDisableDelay, DisableAlarmInternal);
    }

    private void DisableAlarmInternal()
    {
        alarmActive = false;
        alarmDisabling = false;
        source.Stop();
        text.DOKill();
        text.alpha = 1;
    }
}
