#ifndef PAINTER_FOUNDATION
#define PAINTER_FOUNDATION

float4 SampleTexture(sampler2D tex, float2 uv) {
#if SHADER_TARGET < 30
	return tex2D(tex, uv);
#else
	return tex2Dlod(tex, float4(uv, 0, 0));
#endif
}

bool ExistPointInTriangle(float3 p, float3 t1, float3 t2, float3 t3)
{
	const float TOLERANCE = 1 - 0.1;

	float3 a = normalize(cross(t1 - t3, p - t1));
	float3 b = normalize(cross(t2 - t1, p - t2));
	float3 c = normalize(cross(t3 - t2, p - t3));

	float d_ab =dot(a, b);
	float d_bc =dot(b, c);

	if (TOLERANCE < d_ab && TOLERANCE < d_bc) {
		return true;
	}
	return false;
}

float2 Rotate(float2 p, float degree) {
	float rad = radians(degree);
	float x = p.x * cos(rad) - p.y * sin(rad);
	float y = p.x * sin(rad) + p.y * cos(rad);
	return float2(x, y);
}

bool IsPaintRange(float2 mainUV, float2 paintUV, float brushScale, float deg) {
	float3 p = float3(mainUV, 0);
	float3 v1 = float3(Rotate(float2(-brushScale, brushScale), deg) + paintUV, 0);
	float3 v2 = float3(Rotate(float2(-brushScale, -brushScale), deg) + paintUV, 0);
	float3 v3 = float3(Rotate(float2(brushScale, -brushScale), deg) + paintUV, 0);
	float3 v4 = float3(Rotate(float2(brushScale, brushScale), deg) + paintUV, 0);
	return ExistPointInTriangle(p, v1, v2, v3) || ExistPointInTriangle(p, v1, v3, v4);
}

float2 CalcBrushUV(float2 mainUV, float2 paintUV, float brushScale, float deg) {
#if UNITY_UV_STARTS_AT_TOP
	return Rotate((mainUV - paintUV) / brushScale, -deg) * 0.5 + 0.5;
#else
	return Rotate((paintUV - mainUV) / brushScale, deg) * 0.5 + 0.5;
#endif
}

#ifdef PAINTER_COLOR_BLEND_USE_CONTROL
	#define PAINTER_COLOR_BLEND(targetColor, brushColor, controlColor) PainterColorBlendUseControl(targetColor, brushColor, controlColor)
#elif PAINTER_COLOR_BLEND_USE_BRUSH
	#define PAINTER_COLOR_BLEND(targetColor, brushColor, controlColor) PainterColorBlendUseBrush(targetColor, brushColor, controlColor)
#elif PAINTER_COLOR_BLEND_NEUTRAL
	#define PAINTER_COLOR_BLEND(targetColor, brushColor, controlColor) PainterColorBlendNeutral(targetColor, brushColor, controlColor)
#elif PAINTER_COLOR_BLEND_ALPHA_ONLY
	#define PAINTER_COLOR_BLEND(targetColor, brushColor, controlColor) PainterColorBlendAlphaOnly(targetColor, brushColor, controlColor)
#else
	#define PAINTER_COLOR_BLEND(targetColor, brushColor, controlColor) PainterColorBlendUseControl(targetColor, brushColor, controlColor)
#endif

float4 ColorBlend(float4 targetColor, float4 brushColor, float blend) {
	return brushColor * (1 - blend * targetColor.a) + targetColor * targetColor.a * blend;
}

#define __COLOR_BLEND(targetColor) ColorBlend((targetColor), mainColor, brushColor.a)

float4 PainterColorBlendUseControl(float4 mainColor, float4 brushColor, float4 controlColor) {
	return __COLOR_BLEND(controlColor);
}

float4 PainterColorBlendUseBrush(float4 mainColor, float4 brushColor, float4 controlColor) {
	return __COLOR_BLEND(brushColor);
}

float4 PainterColorBlendNeutral(float4 mainColor, float4 brushColor, float4 controlColor) {
	return __COLOR_BLEND((brushColor + controlColor * controlColor.a) * 0.5);
}

float4 PainterColorBlendAlphaOnly(float4 mainColor, float4 brushColor, float4 controlColor) {
	float4 col = mainColor;
	col.a = controlColor.a;
	return __COLOR_BLEND(col);
}
#endif