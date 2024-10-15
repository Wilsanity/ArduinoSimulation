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
        sPort.Open();//Port is opened for data transfers
        sPort.ReadTimeout = 1000;//timeout period for reading data
    }

    // Update is called once per frame
    void Update()
    {
        if(sPort.IsOpen) //When the portal is running
        {
            try
            {
                ShowMessageOne("Operation Started! Equipment Ready!");
                string data = sPort.ReadLine();//reads incoming data
                string[] vals = data.Split(',');//splits data into an array for organizing it as a delimeter

                if (vals.Length == 3)//Match the data values for the 3 potentiometers
                {
                    float angle = float.Parse(vals[0]);//data values are parsed for different parts of the arm based on the values
                    float angle2 = float.Parse(vals[1]);//of the potentiometers
                    float angle3 = float.Parse(vals[2]);

                    armPart1.localRotation = Quaternion.Euler(angle, 0, 0);
                    armPart2.localRotation = Quaternion.Euler(angle2, 0, 0);
                    armPart3.localRotation = Quaternion.Euler(angle3, 0, 0);
                    Debug.Log("Moving...");
                    
                }
            }

            catch (TimeoutException)//If there's an intentional break in reading data from the port.
            {
                ShowMessageTwo("Operation Not Started! Equipment Inactive!");
                Debug.LogWarning("Timeout reading serial port");
                
            }
        }
    }

    public void ShowMessageOne(string msg)//Toggle between the 2 on-screen messages depending on the state of the arduino.
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

    private void OnApplicationQuit()//the serial port is closed properly when the unity scene is exited and does not continue running.
    {
        if (sPort.IsOpen)
        {
            sPort.Close();
        }
    }
}
