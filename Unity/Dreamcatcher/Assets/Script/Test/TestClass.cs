using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestClass : MonoBehaviour {

    private List<double> pitchList;

    public double bpm=0;

	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
        freqData = new float[nSamples];
        fMax = AudioSettings.outputSampleRate / 2;
        pitchList = new List<double>();
	}

	// Update is called once per frame
    void Update()
    {
        getPitchValue();
        //BandVol(8000, 16000);
        //testBandVol();
        //getMaxDBPitch();
    }

    private float[] freqData;
    private int nSamples = 8192;
    private float fMax;

    private void testBandVol()
    {
        // get spectrum
        AudioListener.GetOutputData(freqData, 0);

        float countLowFrequency = 0;
        float countHightFrequency = 0;
        float countLostFrequency = 0;
        float refValue = 0.1f;

        // average the volumes of frequencies fLow to fHigh
        for (var i = 0; i < nSamples; i++)
        {
            float rmsValue = Mathf.Abs(freqData[i]); // rms = square root of average
            float dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
            //Debug.Log(realFreqValue);
            if (i < nSamples/3)
                countLowFrequency += Mathf.Abs(dbValue);
            else if (i > 2*(nSamples / 3))
                countHightFrequency += Mathf.Abs(dbValue);
            else
                countLostFrequency += Mathf.Abs(dbValue);
        }
        Debug.LogError("high F = " + countHightFrequency);
        Debug.LogError("lost F = " + countLostFrequency);
        Debug.LogError("low F = " + countLowFrequency);
        float count = countHightFrequency  - countLowFrequency ;
        Debug.Log(count == 0 ? "0" : count < 0 ? "-" : "+");
    }

    private void BandVol(float fLow, float fHigh){
 
        //fLow = Mathf.Clamp(fLow, 20, fMax); // limit low...
        //fHigh = Mathf.Clamp(fHigh, fLow, fMax); // and high frequencies
        // get spectrum
        AudioListener.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris); 
        //float n1 = Mathf.Floor(fLow * nSamples / fMax);
        //float n2 = Mathf.Floor(fHigh * nSamples / fMax);

        int countLowFrequency = 0;
        int countHightFrequency = 0;
        int countLostFrequency = 0;

        float realFreqValue = 0.0f;
        float dR = 0.0f;
        float dL = 0.0f;

        // average the volumes of frequencies fLow to fHigh
        for (var i=1; i<nSamples-1; i++){
            dL = freqData[i-1] / freqData[i] ;
            dR = freqData[i + 1] / freqData[i];
            realFreqValue = (i + 0.5f * (dR*dR - dL*dL)) * (fMax) / nSamples;
            //Debug.Log(realFreqValue);
            if (realFreqValue < fLow)
                countLowFrequency++;
            else if (realFreqValue > fHigh)
                countHightFrequency++;
            else
                countLostFrequency++;
        }
        Debug.LogError("high F = " + countHightFrequency);
        Debug.LogError("lost F = " + countLostFrequency);
        Debug.LogError("low F = " + countLowFrequency);
        float count = countHightFrequency - countLowFrequency;
        Debug.Log(count == 0 ? "0" : count < 0 ? "-" : "+");
    }

    private void getMaxDBPitch()
    {
        float[] samples = new float[nSamples];
        float[] spectrum = new float[nSamples];

        GetComponent<AudioSource>().GetOutputData(samples, 0);
        float maxValue = 0;
        int maxI = 0;
        float tmp;
        for (int i = 0; i < nSamples; i++)
        {
            tmp = samples[i] * samples[i];
            if ( tmp > maxValue)
            {
                maxValue = tmp;
                maxI = i;
            }
        }
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        pitchList.Add(maxI * (24000/nSamples));
        Debug.Log("Hz pitch : " + getAveragePitch());
    }

    private void getPitchValue()
    {
        float fSample = AudioSettings.outputSampleRate;
        float[] samples = new float[nSamples];
        float[] spectrum = new float[nSamples];
        float threshold = 0.02f;
        float refValue = 0.1f;

        GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i=0; i < nSamples; i++){
            sum += samples[i]*samples[i]; // sum squared samples
        }
        float rmsValue = Mathf.Sqrt(sum/nSamples); // rms = square root of average
        float dbValue = 20*Mathf.Log10(rmsValue/refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
        // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        for (i=0; i < nSamples; i++){ // find max 
            if (spectrum[i] > maxV && spectrum[i] > threshold){
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }
        double freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < nSamples-1){ // interpolate index using neighbours
            float dL = spectrum[maxN-1]/spectrum[maxN];
            float dR = spectrum[maxN+1]/spectrum[maxN];
            freqN += 0.5*(dR*dR - dL*dL);
        }
        double pitchValue = freqN*(fSample/2)/nSamples; // convert index to frequency
        pitchList.Add(pitchValue);
        //Debug.Log("average pitch : " + getAveragePitch());
        this.bpm = getAveragePitch();

        //Debug.Log("RMS: " + rmsValue.ToString("F2") + " (" + dbValue.ToString("F1") + " dB)\n" + "Pitch: " + pitchValue.ToString("F0") + " Hz");
    }

    private double getAveragePitch()
    {
        double sum = 0;
        foreach(double pitch in pitchList)
        {
            sum += pitch;
        }
        return sum / pitchList.Count;
    }
}
