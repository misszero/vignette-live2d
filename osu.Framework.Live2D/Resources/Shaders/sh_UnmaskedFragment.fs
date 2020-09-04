#ifndef GL_ES
    #define lowp
    #define mediump
    #define highp
#else
    // GL_ES expects a defined precision for every member. Users may miss this requirement, so a default precision is specified.
    precision mediump float;
#endif

#include "sh_Utils.h"

varying vec2 v_texCoord;
uniform sampler2D s_texture0;
uniform vec4 u_baseColor;

void main() {
    vec4 color = toSRGB(texture2D(s_texture0, v_texCoord) * u_baseColor);
    gl_FragColor = vec4(color.rgb * color.a, color.a);
}
