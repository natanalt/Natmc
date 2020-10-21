using Natmc.Core;
using Natmc.Graphics.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc
{
    public class TestScene : IGameState
    {
        public static readonly float[] Vertices =
        {
            +0.0f, -0.5f, +0.0f,
            -0.5f, +0.5f, +0.0f,
            +0.5f, +0.5f, +0.0f,
        };

        public static readonly uint[] Indices =
        {
            0, 1, 2,
        };

        public static readonly ShaderProgram Program = ShaderProgram.Compile(
            @"
#version 330 core

layout (location 0) in vec3 inVertexPosition;

uniform mat4 ProjectionMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ModelMatrix;

void main()
{
    gl_Position = vec4(inVertexPosition, 1) * ProjectionMatrix * ViewMatrix * ModelMatrix;
}
",
            @"
#version 330 core

out vec4 OutputColor;

void main()
{
    OutputColor = vec4(1,0,1,1);
}
"

        
        );
    }
}
