public enum MessageType
{
    //测试
    Test1, Test2, Test3,

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
    image=40,                   //传输图片
    /// <summary>
    /// 心跳包
    /// </summary>
    heartbeat = 100,          //心跳包
    /// <summary>
    /// 回应心跳
    /// </summary>
    reHeartbeat=101,

    

    游戏存档 = 500,

    跳转场景 = 1000,

    结束场景,

}