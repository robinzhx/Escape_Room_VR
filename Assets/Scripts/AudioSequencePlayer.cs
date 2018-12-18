using UnityEngine;
using System.Collections;
using Valve.VR;

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(AudioSource))]
    public class AudioSequencePlayer : MonoBehaviour
    {
        public TextMesh TextInstruction;
        public AudioClip[] Clip;
        public string[] text;
        public GameObject[] ObjectsToEnable;
        public CheckIfColliderEnter objToCheckIfTeleportHere;
        public AudioClip greatClip;

        void Start()
        {
            StartCoroutine(playSound());
        }

        private void Update()
        {

        }

        IEnumerator playSound()
        {
            yield return new WaitForSeconds(5.0f);
            TextInstruction.text = text[0];
            GetComponent<AudioSource>().clip = Clip[0];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[0].length + 0.5f);
            TextInstruction.text = text[1];
            GetComponent<AudioSource>().clip = Clip[1];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[1].length + 0.5f);
            TextInstruction.text = text[2];
            GetComponent<AudioSource>().clip = Clip[2];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[2].length + 0.5f);
            TextInstruction.text = text[3];
            GetComponent<AudioSource>().clip = Clip[3];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[3].length + 0.5f);
            TextInstruction.text = text[4];
            GetComponent<AudioSource>().clip = Clip[4];
            GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(5.0f);
            foreach (Hand hand in Player.instance.hands)
            {
                ControllerButtonHints.HideAllButtonHints(hand);
                ControllerButtonHints.HideAllTextHints(hand);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_SteamVR_Touchpad, "Touchpad");
            }
            TextInstruction.text = text[5];
            GetComponent<AudioSource>().clip = Clip[5];
            GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(Clip[5].length + 0.5f);
            TextInstruction.text = text[6];
            GetComponent<AudioSource>().clip = Clip[6];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[6].length + 0.5f);
            TextInstruction.text = text[7];
            GetComponent<AudioSource>().clip = Clip[7];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[7].length + 0.5f);

            ObjectsToEnable[0].SetActive(true);
            TextInstruction.text = text[8];
            GetComponent<AudioSource>().clip = Clip[8];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[8].length + 0.5f);

            if (!(objToCheckIfTeleportHere.isEnter))
            {
                TextInstruction.text = text[9];
                GetComponent<AudioSource>().clip = Clip[9];
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(Clip[9].length + 0.5f);
                TextInstruction.text = text[10];
                GetComponent<AudioSource>().clip = Clip[10];
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(Clip[10].length + 0.5f);
                TextInstruction.text = text[11];
                GetComponent<AudioSource>().clip = Clip[11];
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(5.0f);
            } else
            {
                TextInstruction.text = text[11];
                GetComponent<AudioSource>().clip = greatClip;
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(greatClip.length + 0.5f);
            }

            foreach (Hand hand in Player.instance.hands)
            {
                ControllerButtonHints.HideAllButtonHints(hand);
                ControllerButtonHints.HideAllTextHints(hand);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_SteamVR_Trigger, "Trigger");
            }
            TextInstruction.text = text[12];
            GetComponent<AudioSource>().clip = Clip[12];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[12].length + 0.5f);
            TextInstruction.text = text[13];
            GetComponent<AudioSource>().clip = Clip[13];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[13].length + 0.5f);
            TextInstruction.text = text[14];
            GetComponent<AudioSource>().clip = Clip[14];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[14].length + 0.5f);

            ObjectsToEnable[1].SetActive(true);
            TextInstruction.text = text[15];
            GetComponent<AudioSource>().clip = Clip[15];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[15].length + 0.5f);
            TextInstruction.text = text[16];
            GetComponent<AudioSource>().clip = Clip[16];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[16].length + 0.5f);
            TextInstruction.text = text[17];
            GetComponent<AudioSource>().clip = Clip[17];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[17].length + 0.5f);

            foreach (Hand hand in Player.instance.hands)
            {
                ControllerButtonHints.HideAllButtonHints(hand);
                ControllerButtonHints.HideAllTextHints(hand);
            }
            yield return new WaitForSeconds(5.0f);
            ObjectsToEnable[2].SetActive(true);
            TextInstruction.text = text[18];
            GetComponent<AudioSource>().clip = Clip[18];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Clip[18].length + 0.5f);
            TextInstruction.text = text[19];
            GetComponent<AudioSource>().clip = Clip[19];
            GetComponent<AudioSource>().Play();
        }
    }
}