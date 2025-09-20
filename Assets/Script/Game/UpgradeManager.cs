using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace GrowGame.Game
{

    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance;

        public GameObject ob;

        public float time = 1f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Instance = this;
        }

        public IEnumerator StopGame()
        {
            ob.SetActive(true);
            yield return new WaitForSeconds(time); // 일정 시간 대기
            ob.SetActive(false);
        }

        public IEnumerator StopText(GameObject obj)
        {
            yield return new WaitForSeconds(time);
            obj.SetActive(false);
        }
    }
}