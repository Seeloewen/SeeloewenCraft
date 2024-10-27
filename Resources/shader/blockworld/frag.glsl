#version 330 core

in vec2 p_texCoords;
in float p_grey;

out vec4 FragColor;

uniform sampler2D texImage;

void main()
{
    vec4 o = texture(texImage, p_texCoords);
    o.x = o.x * p_grey;
    o.y = o.y * p_grey;
    o.z = o.z * p_grey;
    FragColor = o;
    //FragColor = vec4(p_texCoords, 0.0, 1.0);
}
