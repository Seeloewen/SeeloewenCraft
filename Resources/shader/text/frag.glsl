#version 330 core

in vec2 p_texCoords;
in vec3 p_color;

uniform sampler2D texImage;

void main()
{
    vec4 o = texture(texImage, p_texCoords);
    o.r = p_color.r * (1-o.r);
    o.g = p_color.g * (1-o.g);
    o.b = p_color.b * (1-o.b);
    gl_FragColor = o;

}