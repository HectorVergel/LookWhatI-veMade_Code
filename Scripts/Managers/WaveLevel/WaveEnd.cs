using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnd : MonoBehaviour
{
    public void EndWaveLevel()
    {
        FindObjectOfType<WaveManager>().killedLastOne = true;
    }
}
