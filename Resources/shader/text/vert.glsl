#version 330 core

layout(location = 0) in vec3 i_pos;
layout(location = 1) in vec2 i_texCoords;
layout(location = 2) in vec3 i_color;

out vec2 p_texCoords;
out vec3 p_color;

void main() {
    gl_Position = vec4(i_pos, 1);
    p_texCoords = i_texCoords;
    p_color = i_color;
}