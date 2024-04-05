uniform sampler2D texture; // The texture to which the scene is rendered
uniform vec2 resolution; // The resolution of the viewport/screen
uniform float overallBrightness; // Control overall brightness

// Warp parameters to simulate the curvature of a CRT monitor
const float warpX = 0.1; // Increase for more curvature
const float warpY = 0.1; // Increase for more curvature

// Function to apply CRT warp effect
vec2 applyCRTWarp(vec2 uv, float warpX, float warpY) {
    vec2 dir = uv - vec2(0.5);
    float dist = length(dir);
    dir = dir * (1.0 + dist * (warpX + warpY));
    return dir + vec2(0.5);
}

void main() {
    vec2 uv = gl_FragCoord.xy / resolution.xy;

    // Apply CRT warp effect
    uv = applyCRTWarp(uv, warpX, warpY);

    // Sample the texture
    vec4 texColor = texture2D(texture, uv);

    // If UVs are outside [0, 1] after distortion, color them black
    if(uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
        return;
    }

    // Calculate the scanline effect based on the warped Y coordinate
    float lineY = uv.y * resolution.y;
    float scanlineEffect = mod(floor(lineY), 2.0) < 1.0 ? 0.9 : 1.0; // Alternate between darker and original brightness

    // Apply the scanline effect
    texColor.rgb *= scanlineEffect * overallBrightness;

    // Set the final color
    gl_FragColor = texColor;
}
