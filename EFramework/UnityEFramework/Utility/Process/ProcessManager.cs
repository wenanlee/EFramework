using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EFramework.Unity.Utility;
using UnityEditor;
using UnityEngine;
namespace EFramework.Unity.Process
{
    public class ProcedureManager : MonoSingleton<ProcedureManager>
    {
        public ProcessType currentProcess;

        private readonly List<I_100_Logo> _0_Logos = new List<I_100_Logo>();
        private List<I_101_Loading> _1_Loadings = new List<I_101_Loading>();
        private List<I_102_GameLoadData> _2_GameLoadDatas = new List<I_102_GameLoadData>();
        private List<I_103_GameLoadDataComplete> _3_GameLoadDataCompletes = new List<I_103_GameLoadDataComplete>();
        private List<I_104_GameInit> _4_GameInits = new List<I_104_GameInit>();
        private List<I_105_GameStart> _5_GameStarts = new List<I_105_GameStart>();

        private List<I_110_SceneLoad> _10_SceneLoads = new List<I_110_SceneLoad>();
        private List<I_111_SceneLoadComplete> _11_SceneLoadCompletes = new List<I_111_SceneLoadComplete>();
        private List<I_112_SceneLoadData> _12_SceneLoadDatas = new List<I_112_SceneLoadData>();
        private List<I_113_SceneLoadDataComplete> _13_SceneLoadDataCompletes = new List<I_113_SceneLoadDataComplete>();
        private List<I_114_SceneInit> _14_SceneInits = new List<I_114_SceneInit>();
        private List<I_115_SceneStart> _15_SceneStarts = new List<I_115_SceneStart>();
        private MonoBehaviour[] behaviours;
        private void Awake()
        {
            Load();
        }
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            _0_Logos.ForEach(x => x.OnLogo());
            currentProcess += 1;
            yield return new WaitForSeconds(0.1f);
            _1_Loadings.ForEach(x => x.OnLoading());
            currentProcess += 1;
            yield return new WaitForSeconds(0.1f);
            _2_GameLoadDatas.ForEach(x => x.OnGameLoadData());
            currentProcess += 1;
            yield return new WaitForSeconds(0.1f);
            _3_GameLoadDataCompletes.ForEach(x => x.OnGameLoadDataComplete());
            currentProcess += 1;
            yield return new WaitForSeconds(0.1f);
            _4_GameInits.ForEach(x => x.OnGameInit());
            currentProcess += 1;
            yield return new WaitForSeconds(0.1f);
            _5_GameStarts.ForEach(x => x.OnGameStart());
            yield return new WaitForSeconds(0.1f);
        }
        public void Load()
        {
            behaviours = FindObjectsOfType<MonoBehaviour>();

            _0_Logos.AddRange(Enumerable.OfType<I_100_Logo>(behaviours));
            _1_Loadings.AddRange(behaviours.OfType<I_101_Loading>());
            _2_GameLoadDatas.AddRange(behaviours.OfType<I_102_GameLoadData>());
            _3_GameLoadDataCompletes.AddRange(behaviours.OfType<I_103_GameLoadDataComplete>());
            _4_GameInits.AddRange(behaviours.OfType<I_104_GameInit>());
            _5_GameStarts.AddRange(behaviours.OfType<I_105_GameStart>());

            _10_SceneLoads.AddRange(behaviours.OfType<I_110_SceneLoad>());
            _11_SceneLoadCompletes.AddRange(behaviours.OfType<I_111_SceneLoadComplete>());
            _12_SceneLoadDatas.AddRange(behaviours.OfType<I_112_SceneLoadData>());
            _13_SceneLoadDataCompletes.AddRange(behaviours.OfType<I_113_SceneLoadDataComplete>());
            _14_SceneInits.AddRange(behaviours.OfType<I_114_SceneInit>());
            _15_SceneStarts.AddRange(behaviours.OfType<I_115_SceneStart>());
        }
    }
    public enum ProcessType
    {
        None = -1,
        _0_Logo = 0,
        _1_Loading,
        _2_GameLoadData,
        _3_GameLoadDataComplete,
        _4_GameInit,
        _5_GameStart,

        _10_SceneLoad = 10,
        _11_SceneLoadComplete,
        _12_SceneLoadData,
        _13_SceneLoadDataComplete,
        _14_SceneInit,
        _15_SceneStart,
    }
}

