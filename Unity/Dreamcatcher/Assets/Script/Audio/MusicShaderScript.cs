using UnityEngine;
using System.Collections;

public class MusicShaderScript : MonoBehaviour {


    public int varColor=3;
    public Material shader;
    public AudioSource audioSource;

    private int precision = 1024;

	// Use this for initialization
	void Start () {
        InvokeRepeating("updateWall", 0.0f,0.07f );
	}

	// Update is called once per frame
    void updateWall()
    {
        float[] channelRight = new float[precision];
        audioSource.GetSpectrumData(channelRight, 1, FFTWindow.BlackmanHarris);
        float[] channelLeft = new float[precision];
        audioSource.GetSpectrumData(channelLeft, 2, FFTWindow.BlackmanHarris);
        int i=1;

        float sum = 0;
        for (i = 0; i < precision; i++)
        {
            sum += (channelRight[i] + channelLeft[i]);
        }

        int refValue=16;
        shader.SetFloat("_Intensity16", ((sum / 2) / varColor) * 5);
        for (i = 0; i < refValue; i++)
        {
            shader.SetFloat("_Intensity" + i, shader.GetFloat("_Intensity" + (i + 1)));
        }
	}
}
