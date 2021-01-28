// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.EventSystems {
#if UNITY_EDITOR

    [AddComponentMenu ("Event/LeopotamGroup/Fast Physics 3D Raycaster")]
#endif
    [RequireComponent (typeof (Camera))]
    public class FastPhysics3DRaycaster : FastPhysics2DRaycaster {
        readonly RaycastHit[] _hitsCache = new RaycastHit[1];

        RaycastResult _result;

        public override void Raycast (PointerEventData eventData, List<RaycastResult> resultAppendList) {
            var eventPos = eventData.position;
            var ray = _eventCamera.ScreenPointToRay (eventPos);
            var distance = _eventCamera.farClipPlane - _eventCamera.nearClipPlane;
            if (Physics.RaycastNonAlloc (ray, _hitsCache, distance, eventMask) > 0) {
                var hitInfo = _hitsCache[0];
                _result.gameObject = hitInfo.collider.gameObject;
                _result.module = this;
                _result.distance = (hitInfo.transform.position - _eventCameraTransform.position).sqrMagnitude;
                _result.index = resultAppendList.Count;
                _result.worldPosition = hitInfo.point;
                _result.worldNormal = hitInfo.normal;
                _result.screenPosition = eventPos;
                resultAppendList.Add (_result);
            }
        }
    }
}