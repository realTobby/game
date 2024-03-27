uniform sampler2D texture; // The texture to which the scene is rendered
uniform vec2 resolution; // The resolution of the texture

void main() {
    // Get the original texture coordinate (no redeclaration, ensure 'uv' is only declared once)
    vec2 uv = gl_FragCoord.xy / resolution.xy;

    // Apply barrel distortion for the CRT effect
    uv -= 0.5; // Center the coordinate system
    float amount = 0.1;
    float sqrlen = dot(uv, uv);
    uv += uv * sqrlen * amount;
    uv += 0.5; // Re-center the coordinate system

    // Sample the texture at the distorted coordinate
    vec4 crtColor = texture2D(texture, uv);

    // Apply scanlines
    float scanlines = sin(uv.y * resolution.y * 3.14) * 0.5 + 0.5;
    crtColor.rgb -= crtColor.rgb * (1.0 - scanlines) * 0.15;

    // Apply a slight RGB shift to mimic color fringing (chromatic aberration)
    vec2 rgbShift = vec2(0.01, 0.0) / resolution.xy;
    crtColor.r = texture2D(texture, uv - rgbShift).r;
    crtColor.g = texture2D(texture, uv).g;
    crtColor.b = texture2D(texture, uv + rgbShift).b;

    // Output the final color
    gl_FragColor = crtColor;
}
