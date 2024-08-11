using BepInEx;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilla;

namespace SpeedCalculator
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public SpeedChecker[] checkers;

        bool gui;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        string MagnitudeFix(float mag)
        {
            string str = mag.ToString();
            if (str.Length < 4)
                return str;

            str = mag.ToString().Substring(0, 4);
            return str;
        }

        void OnGUI()
        {
            GUI.color = new Color(1, 0.7f, 1);

            if (gui)
            {
                bool tellStuff = checkers != null;

                GUILayout.Label("\n\n");

                if (!tellStuff)
                {
                    GUILayout.Label("Join a room.");
                    GUILayout.Label("For each person you will be able to see their speed and more speed information.");
                }

                if (tellStuff)
                {
                    foreach (SpeedChecker checker in checkers)
                    {
                        string str = $"{checker.player.NickName}, {MagnitudeFix(checker.magnitude)} - Highest Speed: {MagnitudeFix(checker.highestSpeed)}";
                        GUILayout.Label(str);
                    }

                    if (GUILayout.Button("Reset"))
                        foreach (SpeedChecker checker in checkers)
                            checker.Reset();
                }
            }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "Speed Checker";
            gameObject.AddComponent<ComponentAdderSC>();

            instance = this;
        }

        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
                gui ^= true;
        }

    }
}
