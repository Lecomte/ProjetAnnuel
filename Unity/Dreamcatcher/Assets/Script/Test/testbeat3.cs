using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testbeat3 : MonoBehaviour {

    public int precision = 8192;

    List<int> excludeIndex;
    List<double[]> energySubBandHistory;
    public int nbBps=0;
    float[] historyBuffer = new float[43];
    float currentConstant;

	// Use this for initialization
	void Start () {
        energySubBandHistory = new List<double[]>();
        nbBps = 0;
        InvokeRepeating("lookForBeats", 0.1f, .25f);
        InvokeRepeating("calculBeat", 0f, 0.15f);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void calculBeat()
    {
        float[] channelRight = new float[precision];
        GetComponent<AudioSource>().GetSpectrumData(channelRight, 1, FFTWindow.BlackmanHarris);
        float[] channelLeft = new float[precision];
        GetComponent<AudioSource>().GetSpectrumData(channelLeft, 2, FFTWindow.BlackmanHarris);
        float e = sumStereo(channelLeft, channelRight);
        //compute local average sound evergy
        float E = sumLocalEnergy() / historyBuffer.Length; // E being the average local sound energy
        //calculate variance
        float sumV = 0;
        for (int i = 0; i < 43; i++)
            sumV += (historyBuffer[i] - E) * (historyBuffer[i] - E);
        float V = sumV / historyBuffer.Length;
        float constant = (float)((-0.0025714 * V) + 1.5142857);
        currentConstant = constant;
        float[] shiftingHistoryBuffer = new float[historyBuffer.Length]; // make a new array and copy all the values to it

        for (int i = 0; i < (historyBuffer.Length - 1); i++)
        { // now we shift the array one slot to the right
            shiftingHistoryBuffer[i + 1] = historyBuffer[i]; // and fill the empty slot with the new instant sound energy
        }

        shiftingHistoryBuffer[0] = e;

        for (int i = 0; i < historyBuffer.Length; i++)
        {
            historyBuffer[i] = shiftingHistoryBuffer[i]; //then we return the values to the original array
        }
        //

        double[] channelRightSubBand = createSubband(channelRight);
        double[] channelLeftSubBand = createSubband(channelLeft);

        double[] energySubBand = new double[precision / 32];
        for (int i = 0; i < precision / 32; i++)
            energySubBand[i] = (channelRightSubBand[i] * channelRightSubBand[i]) + (channelLeftSubBand[i] * channelLeftSubBand[i]);

        /*for (int indexSubBand = 0; indexSubBand < energySubBand.Length; indexSubBand++)
        {
            for (int indexHistory = 0; indexHistory < energySubBandHistory.Count; indexHistory++)
            {
                cumEnergy += energySubBandHistory[indexHistory][indexSubBand];
            }
            //Debug.Log("Energie cumulé : " + cumEnergy);
            averageEnergy = cumEnergy / energySubBandHistory.Count;
            if (energySubBand[indexSubBand] > (constant * averageEnergy) && energySubBandHistory.Count > 41)
            {
                nbBps++;
                currentBps++;
            }
        }*/

        energySubBandHistory.Insert(0, energySubBand);
        if (energySubBandHistory.Count > 43)
        {
            energySubBandHistory.RemoveAt(43);
        }
        //Debug.Log(constant);
        //Debug.Log(nbBps * (60 / audio.time));
        //nbBps = 0;

    }

    private void lookForBeats()
    {
        double cumEnergy = 0;
        double averageEnergy = 0;
        int currentBps = 0;
        for (int indexSubBand = 0; indexSubBand < energySubBandHistory[0].Length; indexSubBand++)
        {
            for (int indexHistory = 1; indexHistory < energySubBandHistory.Count; indexHistory++)
            {
                cumEnergy += energySubBandHistory[indexHistory][indexSubBand];
            }
            //Debug.Log("Energie cumulé : " + cumEnergy);
            averageEnergy = cumEnergy / energySubBandHistory.Count;
            if (energySubBandHistory.Count > 42 && energySubBandHistory[0][indexSubBand] > (currentConstant * averageEnergy))
            {
                nbBps++;
                currentBps++;
            }
        }
        Debug.Log(GetComponent<AudioSource>().time + " bpm :" + nbBps * (60 / GetComponent<AudioSource>().time));
        //Debug.Log("current bps :" + currentBps );
    }

    double[] createSubband(float[] data)
    {
        double[] energySubBand = new double[precision / 32];
        int i = 0;
        int j = 0;
        double sum = 0;
        for (i = 0; i < energySubBand.Length; i++)
        {
            sum = 0;
            for (j = 0; j < precision / 32; j++)
            {
                sum += data[j];
            }
            energySubBand[i] = sum;
        }
        return energySubBand;
    }

    float sumStereo(float[] channel1, float[] channel2)
    {
        float e = 0;
        for (int i = 0; i < channel1.Length; i++)
        {
            e += ((channel1[i] * channel1[i]) + (channel2[i] * channel2[i]));
        }

        return e;
    }

    float sumLocalEnergy()
    {
        float E = 0;

        for (int i = 0; i < historyBuffer.Length; i++)
        {
            E += historyBuffer[i] * historyBuffer[i];
        }

        return E;
    }
}
