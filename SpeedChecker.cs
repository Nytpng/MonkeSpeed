using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpeedCalculator
{
    public class SpeedChecker : MonoBehaviourPunCallbacks
    {
        Vector3 current;
        Vector3 last;

        public Player player; 
        public Vector3 vector;
        public float highestSpeed;
        public float magnitude;

        float multiply = 40;
        float timer;

        void Update()
        {
            current = transform.position;

            timer += Time.deltaTime;
            if (timer > 0.4f)
            {
                timer = 0;
                vector = (current - last) * multiply;
                magnitude = vector.magnitude;

                if (magnitude > highestSpeed)
                    highestSpeed = magnitude;
            }

            last = current;
        }

        public void Reset()
        {
            highestSpeed = 0;
        }
    }
}
