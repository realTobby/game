uniform sampler2D texture; // The texture to which the scene is rendered
uniform vec2 resolution; // The resolution of the viewport/screen
uniform float crtSpeed; // This should be the elapsed time, increasing every frame
uniform float overallBrightness; // Control overall brightness, should be less than 1.0

float warp = 0.75; // Warp effect strength, for simulating curvature of CRT monitor
float scan = 0.75; // Scanline effect strength, for simulating darkness between scanlines

void main() {
    // Normalized texture coordinates
    vec2 uv = gl_FragCoord.xy / resolution.xy;
    vec2 dc = uv - 0.5; // Centered coordinates (0.5, 0.5) is the center

    // Apply barrel distortion
    float dist = dot(dc, dc); // Distance squared from the center
    dc *= mix(1.0, 1.0 + (warp * dist), dist); // Apply the warp factor based on distance from center
    uv = dc + 0.5; // Re-add the center displacement after warping

     // Calculate scanlines that move with time
    float scanlineY = uv.y * resolution.y - crtSpeed * 20.0; // Speed of scanline movement
    float scanlineEffect = abs(sin(scanlineY)); // Sin wave for scanline effect


    // Boundary check - If coordinates are outside, set to black
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
    } else {
        // Sample the texture and apply the scanline effect
        vec4 texColor = texture2D(texture, uv);

        // Apply overall brightness reduction
        texColor.rgb *= overallBrightness;

        // Apply scanlines effect
        float applyScan = scan * scanlineEffect;
        gl_FragColor = mix(texColor, texColor * applyScan, applyScan);
    }
}
