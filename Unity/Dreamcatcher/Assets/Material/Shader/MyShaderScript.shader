Shader "Custom/MyShaderScript" {
	Properties {
		_Intensity0 ("Intensity 0", Float) = 0		
		_Intensity1 ("Intensity 1", Float) = 0
		_Intensity2 ("Intensity 2", Float) = 0
		_Intensity3 ("Intensity 3", Float) = 0
		_Intensity4 ("Intensity 4", Float) = 0
		_Intensity5 ("Intensity 5", Float) = 0
		_Intensity6 ("Intensity 6", Float) = 0
		_Intensity7 ("Intensity 7", Float) = 0
		_Intensity8 ("Intensity 8", Float) = 0
		_Intensity9 ("Intensity 9", Float) = 0
		_Intensity10 ("Intensity 10", Float) = 0
		_Intensity11 ("Intensity 11", Float) = 0
		_Intensity12 ("Intensity 12", Float) = 0
		_Intensity13 ("Intensity 13", Float) = 0
		_Intensity14 ("Intensity 14", Float) = 0
		_Intensity15 ("Intensity 15", Float) = 0
		_ColumnWidth ("_ColumnWidth", Float) = 0
		_ColumnHeight ("_ColumnHeight", Float) = 0
		_HeightDisplacementFactor ("_HeightDisplacementFactor", Float) = 0
		_OffSetX ("_OffSetX", Float) = 0
	}
	SubShader {
        Pass {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off 
        Fog { Mode Off }
        ZWrite Off
     	Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform float _Intensity0;
            uniform float _Intensity1;
            uniform float _Intensity2;
            uniform float _Intensity3;
            uniform float _Intensity4;
            uniform float _Intensity5;
            uniform float _Intensity6;
            uniform float _Intensity7;
            uniform float _Intensity8;
            uniform float _Intensity9;
            uniform float _Intensity10;
            uniform float _Intensity11;
            uniform float _Intensity12;
            uniform float _Intensity13;
            uniform float _Intensity14;
            uniform float _Intensity15;
            uniform float _ColumnWidth;
            uniform float _ColumnHeight;
            uniform float _HeightDisplacementFactor;
            uniform float _OffSetX;
            
            struct ColorRGB {
                float R;
                float G;
                float B;
				float a;
            };
            
            struct vertexInput {
                float4 vertex : POSITION;
            };

            struct fragmentInput{
            	float4 oldPosition : COLOR;
                float4 position : SV_POSITION;
            }; 
            
            ColorRGB HSL2RGB(float h, float sl, float l)
      		{
            	float v;
            	float r,g,b;
 
            	r = l;   // default to gray
            	g = l;
            	b = l;
            	v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            	if (v > 0)
            	{
                  float m;
                  float sv;
                  int sextant;
                  float fract, vsf, mid1, mid2;
 
                  m = l + l - v;
                  sv = (v - m ) / v;
                  h *= 6.0;
                  sextant = (int)h;
                  fract = h - sextant;
                  vsf = v * sv * fract;
                  mid1 = m + vsf;
                  mid2 = v - vsf;
                  switch (sextant)
                  {
                        case 0:
                              r = 0;
                              g = h * 255;
                              b = 0;
                              break;
                        case 1:
                              r = 0;
                              g = 255;
                              b = h-1 * 255;
                              break;
                        case 2:
                              r = 0;
                              g = 255 - ((h - 2)*255);
                              b = 255;
                              break;
                        case 3:
                              r = (h-3) * 255;
                              g = 0;
                              b = 255;
                              break;
                        default:
                              r = 255;
                              g = 0;
                              b = 255 - ((h-4) * 255);
                              break;
							
                  }
            	}
            	ColorRGB rgb;
            	rgb.R = r;
            	rgb.G = g;
            	rgb.B = b;
				rgb.a = 0.5f;
            	return rgb;
      		}      

            fragmentInput vert(vertexInput i) {            	
            	fragmentInput o;
            	o.oldPosition = i.vertex;
                o.position = mul (UNITY_MATRIX_MVP, i.vertex);
                ColorRGB rbg = HSL2RGB(_Intensity0, 0.5, 0.5);
            	return o;
            }

            fixed4 frag(fragmentInput i) : COLOR {
            	int rank = int(abs(i.oldPosition.x + _OffSetX) / _ColumnWidth) % 16;
            	float intensity;
            	ColorRGB col;
            	switch (rank)
            	{
            		case 0: 
            		{
            			intensity = _Intensity0;
            			break;
            		}
            		case 1:
            		{            		
            			intensity = _Intensity1;
            			break;
            		}
            		case 2:
            		{            		
            			intensity = _Intensity2;
            			break;
            		}
            		case 3:
            		{            		
            			intensity = _Intensity3;
            			break;
            		}
            		case 4:
            		{            		
            			intensity = _Intensity4;
            			break;
            		}
            		case 5:
            		{            		
            			intensity = _Intensity5;
            			break;
            		}
            		case 6:
            		{            		
            			intensity = _Intensity6;
            			break;
            		}
            		case 7:
            		{            		
            			intensity = _Intensity7;
            			break;
            		}
            		case 8:
            		{            		
            			intensity = _Intensity8;
            			break;
            		}
            		case 9:
            		{            		
            			intensity = _Intensity9;
            			break;
            		}
            		case 10:
            		{            		
            			intensity = _Intensity10;
            			break;
            		}
            		case 11:
            		{            		
            			intensity = _Intensity11;
            			break;
            		}
            		case 12:
            		{            		
            			intensity = _Intensity12;
            			break;
            		}
            		case 13:
            		{            		
            			intensity = _Intensity13;
            			break;
            		}
            		case 14:
            		{            		
            			intensity = _Intensity14;
            			break;
            		}
            		case 15:
            		{            		
            			intensity = _Intensity15;
            			break;
            		}
            	}
            	float xcenter = (rank * _ColumnWidth) + _ColumnWidth / 2;
            	float ycenter = (intensity * _HeightDisplacementFactor);
            	if (i.oldPosition.y + ycenter <= _ColumnHeight / 2 &&
            				i.oldPosition.y + ycenter  >= -_ColumnHeight / 2)
            		col = HSL2RGB(intensity, 0.8, 0.5);
            	
                return fixed4(col.R,col.G,col.B,1 - abs(_ColumnHeight / 2 + ycenter + i.oldPosition.y) / _ColumnHeight/2);
            }

            ENDCG
        }
    }
}
