using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoom_PuzzleManager : MonoBehaviour
{
    private bool solved = false;

    public bool[] unlockBool = { false, false, false };

    public AudioClip unlockSound;
    public AudioClip lockSound;
    public AudioSource source;
    public NVRInteractableItem objToUnlock;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void unlock(int i)
    {
        print("sucess " + i);
        unlockBool[i] = true;
        checkUnlockAll();
    }

    public void lockback(int i)
    {
        print("lock " + i);
        unlockBool[i] = false;
    }

    void checkUnlockAll()
    {
        bool allUnlock = true;
        foreach (bool b in unlockBool)
        {
            allUnlock = allUnlock && b;
        }

        if (objToUnlock != null && allUnlock && !solved)
        {
            print("Unlock All");

            objToUnlock.CanAttach = true;
            source.PlayOneShot(unlockSound, 1.0f);
            solved = true;


        }
        else {
            print("Unlock All: " + allUnlock + "  solved?: " + solved );
        }
        
    }
}
