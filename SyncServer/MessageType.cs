using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MessageType
{
    //测试
    Test1, Test2, Test3,
    /// <summary>
    /// 连接服务器
    /// </summary>
    access = 10,
    //业务逻辑
    /// <summary>
    /// 登录
    /// </summary>
    login = 30,               //登录
    /// <summary>
    /// 添加学生
    /// </summary>
    addStudents = 31,         //添加学生
    /// <summary>
    /// 删除学生
    /// </summary>
    delStudent = 32,          //删除学生
    /// <summary>
    /// 传输图片
    /// </summary>
    Texture = 40,             //传输图片
    /// <summary>
    /// 心跳包
    /// </summary>
    heartbeat = 100,          //心跳包
    /// <summary>
    /// 回应心跳
    /// </summary>
    reHeartbeat = 101,

    /// <summary>
    /// 连接成功
    /// </summary>
    connectok,

    Save = 500,

    File = 1000,


    结束场景,


    SynPlayer,

    PlayerID,
}