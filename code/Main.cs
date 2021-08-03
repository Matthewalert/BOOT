using BepInEx;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Reflection;
using UnityEngine;

namespace Back_Out_Of_Tree
{
    public class BOOT : MonoBehaviour
    {
        private static GameObject Toggle_Button = new GameObject();
        private static Button TB_class;
        private static GorillaTriggerBox[] triggers = new GorillaTriggerBox[3];
        void Awake()
        {
            //Buttons and triggers :D
            Toggle_Button = Instantiate(Resources.FindObjectsOfTypeAll<GorillaHatButton>()[0].gameObject);
            triggers = Resources.FindObjectsOfTypeAll<GorillaTriggerBox>();

            Destroy(Toggle_Button.GetComponent<GorillaHatButton>());
            Toggle_Button.AddComponent<Button>();

            Toggle_Button.transform.position = new Vector3(-63.5f, 12.5f, -81.55f);
            Toggle_Button.transform.forward = new Vector3(0, 0, -1);

            TB_class = Toggle_Button.GetComponent<Button>();
        }
        
        private static bool Desire = true;
        private static bool CurState = false;
        void Update()
        {
            //Don't disable the triggers while in a room - it'll let people leave the map without leaving
            Desire = TB_class.state;
            bool Disable = PhotonNetwork.CurrentRoom == null ? Desire : false;

            if (CurState != Disable)
            {
                //Yes I have to do this awfullnes 
                foreach (GorillaTriggerBox trigger in triggers)
                {
                    if (trigger.name.Contains("JoinPublicRoom"))
                    {
                        float offset = Disable ? -100 : 100;
                        trigger.transform.position += new Vector3(0, offset, 0);
                    }
                }
                CurState = Disable;
            }
        }
    }


    [BepInPlugin("org.Charlie.gorilla.BOOT", "Back Out Of Tree", "1.0.0")]
    public class HarmonyStuff : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = new Harmony("com.Charlie.gorilla.BOOT");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake")]

    public class Apply
    {
        private static void Postfix(GorillaLocomotion.Player __instance)
        {
            __instance.gameObject.AddComponent<BOOT>();
        }
    }
}

//If you are reading this you may want to watch the unlisted youtube video of how this works -> https://www.youtube.com/watch?v=8JoFsORvFWE