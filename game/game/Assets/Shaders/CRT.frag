uniform sampler2D texture; // The texture to which the scene is rendered
uniform vec2 resolution; // The resolution of the texture
float bendAmount = 1.25f;

void main() {
    // Get the original texture coordinate
    vec2 uv = gl_FragCoord.xy / resolution.xy;

    // Apply barrel distortion for the CRT effect
    uv -= 0.5; // Center the coordinate system
    float sqrlen = dot(uv, uv);
    uv += uv * sqrlen * bendAmount;
    uv += 0.5; // Re-center the coordinate system

    // Sample the texture at the distorted coordinate
    vec4 color = texture2D(texture, uv);

    // Add scanlines
    float scanline = sin(uv.y * resolution.y * 3.14159);
    color.rgb *= 0.9 + 0.1 * scanline;

    // Check if the uv coordinates are outside the 0 to 1 range
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        // Set the color to black if the pixel is out of bounds
        gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
    } else {
        // Sample the texture at the distorted coordinate
        // Output the final color
        gl_FragColor = color;
    }
}
