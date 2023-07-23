using System.Collections.Generic;
using UnityEngine;

namespace AD.Utility
{
    public static class TimeExtension
    {
        public static GameObject CoroutineWorker = null;

        public static void Init()
        {
            if (CoroutineWorker != null)
            {
                CoroutineWorker.GetComponent<MonoBehaviour>().StopAllCoroutines();
                GameObject.Destroy(CoroutineWorker);
            }
            CoroutineWorker = new GameObject();
            CoroutineWorker.AddComponent<ClockTick>();
            CoroutineWorker.name = "CoroutineWorker(TimeAssets)";
        }

        public static int Register(string name)
        {
            return CoroutineWorker.GetComponent<ClockTick>().Register(name);
        }

        public static int Register()
        {
            return CoroutineWorker.GetComponent<ClockTick>().Register();
        }
    }

    public class ClockTick : MonoBehaviour
    {
        private List<string> TimeName = new List<string>();
        private List<float> TimeList = new List<float>();
        private List<float> TimeSpeed = new List<float>();

        private void Awake()
        {
            Register("global");
        }

        private void LateUpdate()
        {
            for (int i = 0; i < TimeList.Count; i++)
            {
                TimeList[i] += UnityEngine.Time.deltaTime * TimeSpeed[i];
            }
        }

        public int Register(string name)
        {
            int index = TimeName.Count;
            TimeName.Add(name);
            TimeList.Add(0);
            TimeSpeed.Add(1);
            return index;
        }

        public int Register()
        {
            int index = TimeName.Count;
            TimeName.Add("null");
            TimeList.Add(0);
            TimeSpeed.Add(1);
            return index;
        }

        public bool UnRegister(string name)
        {
            if (TimeName.Contains(name))
            {
                int index = TimeName.FindIndex((T) => { return T == name; });
                UnRegister(index);
                return true;
            }
            else return false;
        }

        public bool UnRegister(int index)
        {
            if (TimeName.Count>index)
            { 
                TimeName.RemoveAt(index);
                TimeList.RemoveAt(index);
                TimeSpeed.RemoveAt(index);
                return true;
            }
            else return false;
        }
    }
}
