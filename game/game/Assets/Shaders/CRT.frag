uniform sampler2D texture; // The texture to which the scene is rendered
uniform vec2 resolution; // The resolution of the texture
float bendAmount = 0.1f;

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

    // Add more visible scanlines by increasing the frequency and contrast
    float scanlineIntensity = 1.25; // Increase this to make scanlines darker
    float scanlineFrequency = resolution.y * 0.75; // Adjust frequency for denser scanlines
    float scanlineEffect = sin(uv.y * scanlineFrequency * 3.14159) * scanlineIntensity;
    
    // Modify the color based on the scanline effect
    color.rgb *= 0.7 + 0.3 * scanlineEffect; // Adjust these values to fine-tune the visibility

    // Check if the uv coordinates are outside the 0 to 1 range
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        // Set the color to black if the pixel is out of bounds
        gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
    } else {
        // Output the final color with enhanced scanlines
        gl_FragColor = color;
    }
}
