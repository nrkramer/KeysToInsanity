uniform extern texture ScreenTexture;

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(ScreenS, coords);
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}