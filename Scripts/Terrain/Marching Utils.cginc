// Source https://iquilezles.org/articles/morenoise/
float hash( float n )
{
    return frac(sin(n)*753.5453123);
}

// Source https://iquilezles.org/articles/morenoise/
float noise( in float3 x )
{
    // grid
    float3 p = floor(x);
    float3 w = frac(x);
    
    // quintic interpolant
    float3 u = w*w*w*(w*(w*6.0-15.0)+10.0);

    
    // gradients
    float3 ga = hash( p+float3(0.0,0.0,0.0) );
    float3 gb = hash( p+float3(1.0,0.0,0.0) );
    float3 gc = hash( p+float3(0.0,1.0,0.0) );
    float3 gd = hash( p+float3(1.0,1.0,0.0) );
    float3 ge = hash( p+float3(0.0,0.0,1.0) );
    float3 gf = hash( p+float3(1.0,0.0,1.0) );
    float3 gg = hash( p+float3(0.0,1.0,1.0) );
    float3 gh = hash( p+float3(1.0,1.0,1.0) );
    
    // projections
    float va = dot( ga, w-float3(0.0,0.0,0.0) );
    float vb = dot( gb, w-float3(1.0,0.0,0.0) );
    float vc = dot( gc, w-float3(0.0,1.0,0.0) );
    float vd = dot( gd, w-float3(1.0,1.0,0.0) );
    float ve = dot( ge, w-float3(0.0,0.0,1.0) );
    float vf = dot( gf, w-float3(1.0,0.0,1.0) );
    float vg = dot( gg, w-float3(0.0,1.0,1.0) );
    float vh = dot( gh, w-float3(1.0,1.0,1.0) );
	
    // interpolation
    return va + 
           u.x*(vb-va) + 
           u.y*(vc-va) + 
           u.z*(ve-va) + 
           u.x*u.y*(va-vb-vc+vd) + 
           u.y*u.z*(va-vc-ve+vg) + 
           u.z*u.x*(va-vb-ve+vf) + 
           u.x*u.y*u.z*(-va+vb+vc-vd+ve-vf-vg+vh);
}

float invLerp(float from, float to, float value){
    return (value - from) / (to - from);
}