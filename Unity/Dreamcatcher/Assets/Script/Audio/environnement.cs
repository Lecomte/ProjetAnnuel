using UnityEngine;
using System.Collections;

public class environnement : MonoBehaviour {

    public GameObject refCube;
    private GameObject[] listGameObject;
    private int precision = 1024;
    public int varColor=3;
    private AudioClip[] itunes;

    public float intensity=0;

	public void Initialize() {
        float z = -45;
        float x = 50;

        listGameObject = new GameObject[64];
        
        GameObject obj;
        Vector3 startPos = new Vector3(x, 0, -z);
	    for(int i=0; i < 64; i++)
        {
            if(i < 16)
            {
                startPos.z -= refCube.GetComponent<Renderer>().bounds.size.z;
                obj = (GameObject)Instantiate(refCube, startPos, Quaternion.identity);
                obj.transform.Rotate(Vector3.up, 180);
            }
            else if (i < 32)
            {
                startPos.x -= refCube.GetComponent<Renderer>().bounds.size.z;
                obj = (GameObject)Instantiate(refCube, startPos, Quaternion.identity);
                obj.transform.Rotate(Vector3.up, -90);
            }
            else if (i < 48)
            {
                startPos.z += refCube.GetComponent<Renderer>().bounds.size.z;
                obj = (GameObject)Instantiate(refCube, startPos, Quaternion.identity);
            }
            else
            {
                startPos.x += refCube.GetComponent<Renderer>().bounds.size.z;
                obj = (GameObject)Instantiate(refCube, startPos, Quaternion.identity);
                obj.transform.Rotate(Vector3.up, 90);
            }
            listGameObject[i] = obj;
        }
        InvokeRepeating("updateWall", 0.0f,0.07f );
	}

    void updateWall()
    {
        float[] channelRight = new float[precision];
        GetComponent<AudioSource>().GetSpectrumData(channelRight, 1, FFTWindow.BlackmanHarris);
        float[] channelLeft = new float[precision];
        GetComponent<AudioSource>().GetSpectrumData(channelLeft, 2, FFTWindow.BlackmanHarris);
        int i=1;
        float sum = 0;
        for (i = 0; i < precision; i++)
        {
            sum += (channelRight[i] + channelLeft[i]);
        }
        this.intensity = sum;
        float colorID;
        int refValue=64/4;
        int baseNumber;
        for (int j = 0; j < 4; j++)
        {
            if (j % 2 == 0)
            {
                baseNumber = j * refValue;
                //listGameObject[baseNumber].transform.position = new Vector3(listGameObject[baseNumber].transform.position.x, sum * 3, listGameObject[baseNumber].transform.position.z);
                listGameObject[baseNumber].transform.localScale = new Vector3(listGameObject[baseNumber].transform.localScale.x, 25 + sum * 6, listGameObject[baseNumber].transform.localScale.z);
                colorID = (((listGameObject[baseNumber].transform.localScale.y - 25) / 4) / varColor) * 5;
                listGameObject[baseNumber].GetComponentInChildren<Light>().color = returnColor(colorID);
                for (i = refValue - 1; i > 0; i--)
                {
                    int number = baseNumber + i;
                    //listGameObject[number].transform.position = new Vector3(listGameObject[number].transform.position.x, listGameObject[number - 1].transform.position.y, listGameObject[number].transform.position.z);
                    listGameObject[number].transform.localScale = new Vector3(listGameObject[number].transform.localScale.x, listGameObject[number - 1].transform.localScale.y, listGameObject[number].transform.localScale.z);
                    colorID = (((listGameObject[number].transform.localScale.y - 25) / 4) / varColor) * 5;
                    listGameObject[number].GetComponentInChildren<Light>().color = returnColor(colorID);
                }
            }
            else
            {
                baseNumber = ((j + 1) * refValue) - 1;
                //listGameObject[baseNumber].transform.position = new Vector3(listGameObject[baseNumber].transform.position.x, sum * 3, listGameObject[baseNumber].transform.position.z);
                listGameObject[baseNumber].transform.localScale = new Vector3(listGameObject[baseNumber].transform.localScale.x, 25 + sum * 6, listGameObject[baseNumber].transform.localScale.z);
                colorID = (((listGameObject[baseNumber].transform.localScale.y - 25) / 4) / varColor) * 5;
                listGameObject[baseNumber].GetComponentInChildren<Light>().color = returnColor(colorID);
                for (i = refValue - 1; i > 0; i--)
                {
                    int number = baseNumber - i; 
                    //listGameObject[number].transform.position = new Vector3(listGameObject[number].transform.position.x, listGameObject[number + 1].transform.position.y, listGameObject[number].transform.position.z);
                    listGameObject[number].transform.localScale = new Vector3(listGameObject[number].transform.localScale.x, listGameObject[number + 1].transform.localScale.y, listGameObject[number].transform.localScale.z);
                    colorID = (((listGameObject[number].transform.localScale.y-25) / 4) / varColor) * 5;
                    listGameObject[number].GetComponentInChildren<Light>().color = returnColor(colorID);
                }
            }
        }
	}

    private Color returnColor(float colorID)
    {
        Color ligthColor = new Color();
        if (colorID < 1)
        {
            ligthColor.r = 0;
            ligthColor.g = colorID * 255;
            ligthColor.b = 0;
        }
        else if (colorID < 2)
        {
            ligthColor.r = 0;
            ligthColor.g = 255;
            ligthColor.b = (colorID - 1) * 255;
        }
        else if (colorID < 3)
        {
            ligthColor.r = 0;
            ligthColor.g = 255 - ((colorID - 2) * 255);
            ligthColor.b = 255;
        }
        else if (colorID < 4)
        {
            ligthColor.r = (colorID - 3) * 255;
            ligthColor.g = 0;
            ligthColor.b = 255;
        }
        else
        {
            ligthColor.r = 255;
            ligthColor.g = 0;
            ligthColor.b = 255 - ((colorID - 4) * 255);
        }
        return ligthColor;
    }
}
