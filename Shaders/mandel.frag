#version 400

uniform vec2 resolution;
uniform float zoom;

float Map(float x)
{
    return x / 255.0;
}

void main()
{
    vec2 c = vec2(gl_FragCoord.x / resolution.x - 0.75, gl_FragCoord.y / resolution.y - 0.5) * zoom;
    vec2 z = vec2(0);
    vec2 z2 = vec2(0);

    for(int i = 0; i < 256; i++)
    {
        // z*z + c
        z = vec2(z2.x - z2.y, z.y * (z.x + z.x)) + c;
        z2 = z * z;

        // lengthSqrd(z) > 2^2
        if(z2.x + z2.y > 4.0)
        {
            gl_FragColor = vec4(Map(i * 2), Map(i * 4), Map(i * 8), 1.0);
            return;
        }
    }

    gl_FragColor = vec4(vec3(0.0), 1.0);
}
