shader_type spatial;

uniform sampler2D diffuse;
uniform sampler2D height;
uniform float ratio;
uniform float UVScale;

void fragment(){
	vec2 compiledUV = UV;
	compiledUV = compiledUV * vec2(UVScale, UVScale);
	ALBEDO = texture(diffuse, compiledUV).rgb;
}

void vertex(){
	VERTEX.y = texture(height, UV).r * ratio;
}