using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using TMPro;

public class LapoController : MonoBehaviour
{
    SerialPort sPort = new SerialPort("COM3", 9600); //Arduino connection serial port

    [SerializeField]
    public Transform armPart1;
    [SerializeField]
    public Transform armPart2;
    [SerializeField]
    public Transform armPart3;
    [SerializeField]
    public TMP_Text text1;
    public TMP_Text text2;

    // Start is called before the first frame update
    void Start()
    {
        sPort.Open();
        sPort.ReadTimeout = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if(sPort.IsOpen)
        {
            try
            {
                ShowMessageOne("Operation Started! Equipment Ready!");
                string data = sPort.ReadLine();
                string[] vals = data.Split(',');

                if (vals.Length == 3)
                {
                    float angle = float.Parse(vals[0]);
                    float angle2 = float.Parse(vals[1]);
                    float angle3 = float.Parse(vals[2]);

                    armPart1.localRotation = Quaternion.Euler(angle, 0, 0);
                    armPart2.localRotation = Quaternion.Euler(angle2, 0, 0);
                    armPart3.localRotation = Quaternion.Euler(angle3, 0, 0);
                    Debug.Log("Moving...");
                    
                }
            }

            catch (TimeoutException)
            {
                ShowMessageTwo("Operation Not Started! Equipment Inactive!");
                Debug.LogWarning("Timeout reading serial port");
                
            }
        }
    }

    public void ShowMessageOne(string msg)
    {
        text1.text = msg;
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
    }

    public void ShowMessageTwo(string msg)
    {
        text2.text = msg;
        text2.gameObject.SetActive(true);
        text1.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        if (sPort.IsOpen)
        {
            sPort.Close();
        }
    }
}
