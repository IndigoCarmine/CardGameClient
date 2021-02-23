using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    NetWorkManager netWorkManager;



    // Start is called before the first frame update
    void Start()
    {
        netWorkManager = GameObject.FindGameObjectWithTag("NetWorkManager").GetComponent<NetWorkManager>();
    }

    public void OnClick()
    {
        string PlayerName = transform.GetChild(0).GetComponent<InputField>().text;
        string HostIPAddress = transform.GetChild(1).GetComponent<InputField>().text;
        netWorkManager.Connect(HostIPAddress);
        netWorkManager.Request(NetWorkManager.RequestParameter.PlayerName, PlayerName);
        this.transform.parent.gameObject.SetActive(false);
    }
}
