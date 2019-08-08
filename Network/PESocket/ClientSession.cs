using PENet;
using Protocol;
using UnityEngine;

public class ClientSession : PESession<Package> {
    protected override void OnConnected() {
        Debug.Log("连接成功");
    }

    protected override void OnReciveMsg(Package msg) {
        //PETool.LogMsg("Server Response:" + msg.GetString());
        //EventData<Package>.CreateEvent(msg.type,msg).SendToHandler();
    }

    protected override void OnDisConnected() {
        Debug.Log("断开连接");
    }
}