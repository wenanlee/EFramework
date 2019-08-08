/************************************************************
    文件：GameStart.cs
	作者：Plane
    QQ ：1785275942
    日期：2018/10/29 5:18
	功能：PESocket客户端使用示例
*************************************************************/

using Protocol;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    PENet.PESocket<ClientSession, Package> skt = null;

    private void Start()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 30000; i++)
            {
                //skt.session.SendMsg(new Package
                //{
                //    msg = new byte[1024]
                //});
            }
        }
    }
}