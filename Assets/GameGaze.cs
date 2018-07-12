using aGlassDKII;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameGaze : MonoBehaviour
{
    int count = 0;
    private StreamWriter _writer;

    // Use this for initialization
    void Start()
    {
        print(aGlass.Instance.aGlassStart());
        _writer = new StreamWriter("Roupeiro_camisa.txt");
        _writer.Write("\n\n=============== Game started ================\n\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1))
        {
            Application.Quit();
        }
        if (aGlass.Instance.GetEyeValid())
        {
            
            print(Time.time.ToString() + " -- Eye X: " + aGlass.Instance.GetGazePoint().x + "  Y: " + aGlass.Instance.GetGazePoint().y);
            _writer.WriteLine(Time.time.ToString() + " -- Eye X: " + aGlass.Instance.GetGazePoint().x + "  Y: " + aGlass.Instance.GetGazePoint().y);

        }
    }

    void OnDestroy()
    {
        print(aGlass.Instance.aGlassStop());
        _writer.Close();
    }

    public void GetPos(GameObject c, ref float cx, ref float cy)
    {
        if (aGlass.Instance.GetEyeValid())
        {
            count = 0;
            if (!c.activeSelf)
            {
                c.SetActive(true);
            }
            cx = aGlass.Instance.GetGazePoint().x;
            cy = aGlass.Instance.GetGazePoint().y;
        }
        else
        {
            count++;
            if (count > 10 && c.activeSelf)
            {
                c.SetActive(false);
            }
        }
    }
}
