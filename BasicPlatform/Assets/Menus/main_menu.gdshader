shader_type canvas_item;

// Captura lo que hay detrás del ColorRect
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
// Control de transparencia del efecto (0.0 = invisible, 1.0 = muy fuerte)
uniform float opacidad_filtro : hint_range(0.0, 1.0) = 0.5;

void fragment() {
    vec2 uv = SCREEN_UV;
    
    // 1. Capturamos el color original de lo que hay DETRÁS
    vec4 color_original = texture(screen_texture, uv);
    
    // 2. Curvatura (opcional)
    vec2 centered_uv = uv * 2.0 - 1.0;
    vec2 uv_curva = uv + centered_uv * (dot(centered_uv, centered_uv)) * 0.03;
    
    // 3. Calculamos el efecto CRT sobre una copia
    vec4 color_crt = texture(screen_texture, uv_curva);
    
    // Añadimos las líneas (scanlines)
    float scanline = sin(uv_curva.y * 600.0) * 0.1;
    color_crt.rgb -= scanline;
    
    // 4. MEZCLA FINAL: Aquí es donde ocurre la transparencia
    // Mezclamos el color original limpio con el color con efecto CRT
    COLOR = mix(color_original, color_crt, opacidad_filtro);
}