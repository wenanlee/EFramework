using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    PENet.PESocket<ClientSession, Package> skt = null;

    public void Init()
    {
        skt = new PENet.PESocket<ClientSession, Package>();
        skt.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);

        skt.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
    }
    public void Send(Package package)
    {
        skt.session.SendMsg(package); 
    }
}
