using EFramework.Tweens.Core;
using UnityEngine;

namespace EFramework.Tweens
{
    public static class PathTween
    {
        public static Tween<Vector3[]> TweenPath(this Component self, Vector3[] path, float speed) =>
            Tween<Vector3[]>.Add<Driver>(self).Finalize(CalculateDuration(path, speed), path);

        public static Tween<Vector3[]> TweenPath(this GameObject self, Vector3[] path, float speed) =>
            Tween<Vector3[]>.Add<Driver>(self).Finalize(CalculateDuration(path, speed), path);

        private static float CalculateDuration(Vector3[] path, float speed)
        {
            if (path == null || path.Length < 2 || speed <= 0)
                return 0f;

            float totalLength = 0f;
            for (int i = 0; i < path.Length - 1; i++)
            {
                totalLength += Vector3.Distance(path[i], path[i + 1]);
            }

            return totalLength / speed;
        }

        private class Driver : Tween<Vector3[]>
        {
            private float progress;
            private Vector3[] pathPoints;
            private float[] segmentLengths;
            private float totalLength;

            public override bool OnInitialize()
            {
                if (this.valueTo == null || this.valueTo.Length < 2)
                {
                    Debug.LogError("Path points array must contain at least 2 points");
                    return false;
                }

                // 预计算各段长度和总长度
                this.pathPoints = this.valueTo;
                this.segmentLengths = new float[pathPoints.Length - 1];
                this.totalLength = 0f;

                for (int i = 0; i < segmentLengths.Length; i++)
                {
                    segmentLengths[i] = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
                    totalLength += segmentLengths[i];
                }

                return true;
            }

            public override Vector3[] OnGetFrom()
            {
                // 起始位置作为路径的第一个点
                return new Vector3[] { this.transform.position };
            }

            public override void OnUpdate(float easedTime)
            {
                this.progress = easedTime;
                
                if (pathPoints == null || pathPoints.Length == 0)
                    return;

                // 计算当前路径位置
                this.transform.position = CalculateLinearPathPosition(this.progress);
            }

            private Vector3 CalculateLinearPathPosition(float t)
            {
                if (pathPoints.Length == 1)
                    return pathPoints[0];

                float targetLength = t * totalLength;
                float accumulatedLength = 0f;

                for (int i = 0; i < segmentLengths.Length; i++)
                {
                    if (accumulatedLength + segmentLengths[i] >= targetLength)
                    {
                        float segmentT = (targetLength - accumulatedLength) / segmentLengths[i];
                        return Vector3.Lerp(pathPoints[i], pathPoints[i + 1], segmentT);
                    }
                    accumulatedLength += segmentLengths[i];
                }

                return pathPoints[pathPoints.Length - 1];
            }
        }
    }
}