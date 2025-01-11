#version 330 core

layout (location = 0) in vec2 v_Pos;
layout (location = 1) in vec2 v_texCoords;
layout (location = 2) in float v_grey;

out vec2 p_texCoords;
out float p_grey;

void main() {
	gl_Position = vec4(v_Pos, 0.5, 1.0);
	p_texCoords = v_texCoords;
	p_grey = v_grey;
}