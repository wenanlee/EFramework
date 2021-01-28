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
    [AddComponentMenu ("Event/LeopotamGroup/Fast Physics 2D Raycaster")]
#endif
    [RequireComponent (typeof (Camera))]
    public class FastPhysics2DRaycaster : BaseRaycaster {
        public virtual int depth {
            get { return (int) eventCamera.depth; }
        }

        public override Camera eventCamera {
            get { return _eventCamera; }
        }

        public LayerMask eventMask = -1;

        protected Camera _eventCamera;

        protected Transform _eventCameraTransform;

        protected override void OnEnable () {
            base.OnEnable ();
            _eventCamera = GetComponent<Camera> ();
            _eventCameraTransform = _eventCamera.transform;
        }

        readonly RaycastHit2D[] _hitsCache = new RaycastHit2D[1];

        RaycastResult _result;

        public override void Raycast (PointerEventData eventData, List<RaycastResult> resultAppendList) {
            var eventPos = eventData.position;
            var ray = _eventCamera.ScreenPointToRay (eventPos);
            var distance = _eventCamera.farClipPlane - _eventCamera.nearClipPlane;
            if (Physics2D.GetRayIntersectionNonAlloc (ray, _hitsCache, distance, eventMask) > 0) {
                var hitInfo = _hitsCache[0];
                _result.gameObject = hitInfo.collider.gameObject;
                _result.module = this;
                _result.distance = (_result.gameObject.transform.position - _eventCameraTransform.position).sqrMagnitude;
                _result.index = resultAppendList.Count;
                _result.worldPosition = hitInfo.point;
                _result.screenPosition = eventPos;
                resultAppendList.Add (_result);
            }
        }
    }
}