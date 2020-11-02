using Natmc.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public class Camera : IDisposable
    {
        private Vector3 m_Position;
        private Quaternion m_Rotation;
        private float m_FOV;
        private float m_DepthNear;
        private float m_DepthFar;

        public Vector3 Position
        {
            get => m_Position;
            set
            {
                if (m_Position == value)
                    return;
                m_Position = value;
                RegenerateViewMatrix();
            }
        }
        public Quaternion Rotation
        {
            get => m_Rotation;
            set
            {
                if (m_Rotation == value)
                    return;
                m_Rotation = value;
                RegenerateViewMatrix();
            }
        }
        public float FovRadians
        {
            get => m_FOV;
            set
            {
                if (m_FOV == value)
                    return;
                m_FOV = value;
                RegenerateProjectionMatrix();
            }
        }
        public float FovDegrees
        {
            get => MathHelper.RadiansToDegrees(m_FOV);
            set
            {
                float radians = MathHelper.DegreesToRadians(value);
                if (m_FOV == radians)
                    return;
                m_FOV = radians;
                RegenerateProjectionMatrix();
            }
        }
        public float DepthNear
        {
            get => m_DepthNear;
            set
            {
                if (m_DepthNear == value)
                    return;
                m_DepthNear = value;
                RegenerateProjectionMatrix();
            }
        }
        public float DepthFar
        {
            get => m_DepthFar;
            set
            {
                if (m_DepthFar == value)
                    return;
                m_DepthFar = value;
                RegenerateProjectionMatrix();
            }
        }

        public Matrix4 ProjectionMatrix { get; protected set; }
        public Matrix4 ViewMatrix { get; protected set; }

        public Camera()
        {
        }
        
        public void Translate(Vector3 move)
        {
            Position += move;
        }

        public void Dispose()
        {
        }

        private void RegenerateProjectionMatrix()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                FovRadians,
                (float)Engine.Window.Size.X / Engine.Window.Size.Y,
                DepthNear,
                DepthFar);
        }

        private void RegenerateViewMatrix()
        {
            ViewMatrix = Matrix4.CreateFromQuaternion(Rotation.Inverted()) * Matrix4.CreateTranslation(-Position);
        }

        private void ResizeHook(ResizeEventArgs _)
        {
            RegenerateProjectionMatrix();
        }
    }
}
