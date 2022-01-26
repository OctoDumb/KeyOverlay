#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D texture;
uniform vec2 resolution;
uniform float size;
uniform int side;

void main( void ) {
    gl_FragColor = texture2D(texture, vec2(gl_TexCoord[0].x, 1 - gl_TexCoord[0].y)) * gl_Color;
    
    float p = size / resolution.y;

    float coord = 1 - gl_TexCoord[0].y;

    if(side == 1) {
        coord = 1 - gl_TexCoord[0].x;
    } else if(side == 2) {
        coord = gl_TexCoord[0].y;
    } else if(side == 3) {
        coord = gl_TexCoord[0].x;
    }

    gl_FragColor.a = min(gl_FragColor.a, min(coord, p) / p);
}