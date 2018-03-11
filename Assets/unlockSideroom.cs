using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class unlockSideroom : MonoBehaviour
{
    Transform originalParent;

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
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            col.gameObject.transform.rotation = Quaternion.Euler(0f, -180f, 90f);
            originalParent = col.gameObject.transform.parent;
            col.gameObject.transform.parent = this.transform;
            col.gameObject.transform.localPosition = new Vector3(-0.0178f, -0.0872f, 0.005059996f);

            if (col.gameObject.name == "key_black")
            {
                col.gameObject.GetComponent<BoxCollider>().enabled = false;
                NVRInteractableItem driveScript = GetComponent<NVRInteractableItem>();
                driveScript.CanAttach = true;
            }
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
