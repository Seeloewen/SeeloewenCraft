#version 330 core

layout (location = 0) in vec2 v_Pos;
layout (location = 1) in vec2 v_texCoords;

out vec2 p_texCoords;

void main() {
	gl_Position = vec4(v_Pos, 0.0, 1.0);
	p_texCoords = v_texCoords;
}