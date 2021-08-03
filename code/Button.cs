using System;
using UnityEngine;
using UnityEngine.UI;

namespace Back_Out_Of_Tree
{
    class Button : MonoBehaviour
    {
        public string message_t = "DISABLED";
        public string message_f = "ENABLED";

        public Color Colour_t = Color.red;
        public Color Colour_f = Color.white;

        public bool state = false;

        private bool Is_Touched = false;
        
        private void Awake()
        {
            state = false;
            UpdateButton(state);
            Console.WriteLine("Button Loaded");
        }
        private void OnTriggerEnter(Collider collider)
        {
            string name = collider.gameObject.name;
            if (name != "LeftHandTriggerCollider" && name != "RightHandTriggerCollider") return;
            bool left = name == "LeftHandTriggerCollider" ? true : false;

            if (!Is_Touched)
            {
                GorillaTagger.Instance.StartVibration(left, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);

                state = !state;

                var new_message = state ? message_t : message_f;

                UpdateButton(state);

                Is_Touched = true;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Console.WriteLine(collider.gameObject.name);

            Is_Touched = false;
        }

        void UpdateButton(bool state)
        {
            string new_message = state ? message_t : message_f;

            Color new_colour = state ? Colour_t : Colour_f;
            Renderer render = gameObject.GetComponent<Renderer>();
            render.material.SetColor("_Color", new_colour);

            //Text txt = gameObject.GetComponentInChildren<Text>();
            //txt.text = new_message;

            Console.WriteLine("Button toggled");
        }
    }
}
