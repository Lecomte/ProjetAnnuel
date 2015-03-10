using UnityEngine;
using System.Collections;

public class environnement : MonoBehaviour {

    public GameObject refCube;
    private GameObject[] listGameObject;
    private int precision = 1024;
    public int varColor=3;
    private AudioClip[] itunes;

	// Use this for initialization
	void Start () {
        float z = -45;
        float x = 50;

        //Debug.Log("start import music : " + Time.deltaTime);
        //importMusic();
        //Debug.Log("end import music : " + Time.deltaTime);

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

    /*void importMusic()
    {
        Object[] musicsAssets = Resources.LoadAll("Music");
        itunes = new AudioClip[musicsAssets.Length];
        AudioClip song;
        int i = 0;
        foreach (Object obj in musicsAssets)
        {
            song = (AudioClip)obj;
            itunes[i] = song;
            i++;
        }
        Debug.LogError("Nombre chargé : " + i);
    }*/
	
    void Update()
    {
        /*if(!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = itunes[Random.Range(0,itunes.Length)];
            GetComponent<AudioSource>().Play();
        }*/
    }

	// Update is called once per frame
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
        float colorID;
        int refValue=64/4;
        int baseNumber;
        for (int j = 0; j < 4; j++)
        {
            if (j % 2 == 0)
            {
                baseNumber = j * refValue;
                listGameObject[baseNumber].transform.position = new Vector3(listGameObject[baseNumber].transform.position.x, sum * 3, listGameObject[baseNumber].transform.position.z);
                colorID = ((listGameObject[baseNumber].transform.position.y / 2) / varColor) * 5;
                listGameObject[baseNumber].GetComponentInChildren<Light>().color = returnColor(colorID);
                for (i = refValue - 1; i > 0; i--)
                {
                    int number = baseNumber + i;
                    //Debug.Log("number : " + number);
                    listGameObject[number].transform.position = new Vector3(listGameObject[number].transform.position.x, listGameObject[number - 1].transform.position.y, listGameObject[number].transform.position.z);
                    colorID = ((listGameObject[number].transform.position.y / 2) / varColor) * 5;
                    listGameObject[number].GetComponentInChildren<Light>().color = returnColor(colorID);
                }
            }
            else
            {
                baseNumber = ((j + 1) * refValue) - 1;
                listGameObject[baseNumber].transform.position = new Vector3(listGameObject[baseNumber].transform.position.x, sum * 3, listGameObject[baseNumber].transform.position.z);
                colorID = ((listGameObject[baseNumber].transform.position.y / 2) / varColor) * 5;
                listGameObject[baseNumber].GetComponentInChildren<Light>().color = returnColor(colorID);
                for (i = refValue - 1; i > 0; i--)
                {
                    int number = baseNumber - i; 
                    //Debug.Log("number : " + number);
                    listGameObject[number].transform.position = new Vector3(listGameObject[number].transform.position.x, listGameObject[number + 1].transform.position.y, listGameObject[number].transform.position.z);
                    colorID = ((listGameObject[number].transform.position.y / 2) / varColor) * 5;
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
