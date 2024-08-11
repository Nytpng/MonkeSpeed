using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpeedCalculator
{
    public class ComponentAdderSC : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            StartCoroutine(CheckWait());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            StartCoroutine(CheckWait());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            StartCoroutine(CheckWait());
        }

        Player FindPlayerFromRig(VRRig vrrig)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
                if (vrrig == rig)
                    return player;
            }

            return PhotonNetwork.LocalPlayer;
        }

        void Check()
        {
            GameObject offlineRig = GorillaTagger.Instance.offlineVRRig.gameObject;
            if (!offlineRig.GetComponent<SpeedChecker>())
            {
                offlineRig.AddComponent<SpeedChecker>().player = PhotonNetwork.LocalPlayer;
            }


            foreach(VRRig vrrig in FindObjectsOfType<VRRig>())
            {
                bool get = vrrig.gameObject.GetComponent<SpeedChecker>();
                bool can = !get && !vrrig.isLocal;

                if (can)
                {
                    SpeedChecker checker = vrrig.gameObject.AddComponent<SpeedChecker>();
                    checker.player = FindPlayerFromRig(vrrig);
                }

                if (get)
                {
                    SpeedChecker checker = vrrig.gameObject.GetComponent<SpeedChecker>();
                    checker.player = FindPlayerFromRig(vrrig);
                    checker.Reset();
                }
            }

            Plugin.instance.checkers = FindObjectsOfType<SpeedChecker>();
        }

        IEnumerator CheckWait()
        {
            yield return new WaitForSeconds(1);
            Check();
        }
    }
}
