#version 330 core

in vec2 p_texCoords;

out vec4 FragColor;

uniform sampler2D texImage;

void main()
{
    FragColor = texture(texImage, p_texCoords);
    //FragColor = vec4(p_texCoords, 0.0, 1.0);
}
