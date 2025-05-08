using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_100_Logo { public void OnLogo(); }
public interface I_101_Loading { public void OnLoading(); }
public interface I_102_GameLoadData { public void OnGameLoadData(); }
public interface I_103_GameLoadDataComplete { public void OnGameLoadDataComplete(); }
public interface I_104_GameInit { public void OnGameInit(); }
public interface I_105_GameStart { public void OnGameStart(); }

public interface I_110_SceneLoad { public void OnSceneLoad(); }
public interface I_111_SceneLoadComplete { public void OnSceneLoadComplete(); }
public interface I_112_SceneLoadData { public void OnSceneLoadData(); }
public interface I_113_SceneLoadDataComplete { public void OnSceneLoadDataComplete(); }
public interface I_114_SceneInit { public void OnSceneInit(); }
public interface I_115_SceneStart { public void OnSceneStart(); }