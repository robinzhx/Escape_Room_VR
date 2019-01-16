using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class unlockLevel : MonoBehaviour
{
    Transform originalParent;
    public Transform KeySnapPosition;
    public AudioClip unlockSound;
    public AudioClip lockSound;
    public AudioSource source;

    private bool keyAttached = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "key_black" || col.gameObject.name == "key_grey" || col.gameObject.name == "key_gold" || col.gameObject.name == "key")
        {




            if (col.gameObject.name == "key_gold")
            {
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                originalParent = col.gameObject.transform.parent;
                col.gameObject.transform.rotation = KeySnapPosition.rotation;
                col.gameObject.transform.parent = this.transform;

                col.gameObject.transform.localPosition = KeySnapPosition.localPosition;
                col.gameObject.GetComponent<BoxCollider>().enabled = false;

                col.gameObject.GetComponent<NVRInteractableItem>().CanAttach = false;
                NVRInteractableItem driveScript = GetComponent<NVRInteractableItem>();
                driveScript.CanAttach = true;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;
                source.PlayOneShot(unlockSound, 1.0f);
            }
            else
            {
                source.PlayOneShot(lockSound, 1.0f);
            }
        }
        if (col.gameObject.name == "trackhat" || col.gameObject.name == "LeftHand")
        {
            source.PlayOneShot(lockSound, 1.0f);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "key_black" || col.gameObject.name == "key_grey" || col.gameObject.name == "key_gold" || col.gameObject.name == "key")
        {
            col.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            col.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

            col.gameObject.transform.parent = originalParent;

            col.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
