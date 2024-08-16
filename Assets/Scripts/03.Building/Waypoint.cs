using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<AnimalStat> animals = new List<AnimalStat>();

    public List<string> clips = new List<string>();
    private List<int> clipsInt = new List<int>();

    public List<string> eyeClips = new List<string>();
    private List<int> eyeClipsInt = new List<int>();

    public List<float> clipSpeed = new List<float>();

    private int currentClipIndex = 0;
    public int maxPopulation = 1;
    public bool IsFull
    {
        get
        {
            foreach(var animal in animals)
            {
                if (animal == null)
                    animals.Remove(animal);
            }

            return animals.Count >= maxPopulation;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return animals.Count <= 0;
        }
    }

    private void OnEnable()
    {
        var floorWayPoint = GetComponentInParent<FloorWaypoint>();

        if (floorWayPoint.waypoints.Contains(this))
            return;

        floorWayPoint.waypoints.Add(this);
    }

    private void Start()
    {
        for (int i = 0; i < clips.Count; ++i )
        {
            clipsInt.Add(Animator.StringToHash(clips[i]));
        }        
        
        for (int i = 0; i < eyeClips.Count; ++i )
        {
            eyeClipsInt.Add(Animator.StringToHash(eyeClips[i]));
        }
    }

    public void EnterAnimal(AnimalStat animal)
    {
        if (animals.Contains(animal))
            return;

        if (IsFull)
            return;

        animals.Add(animal);
    }

    public void ExitAnimal(AnimalStat animal)
    {
        if (IsEmpty)
            return;

        if (!animals.Contains(animal))
            return;

        animals.Remove(animal);
    }

    public int GetRandomClip(out float speed)
    {
        speed = 1.0f;
        if (clips.Count == 0)
            return -1;

        currentClipIndex = Random.Range(0, clips.Count);

        if (clipSpeed.Count != 0)
            speed = clipSpeed[currentClipIndex];

        return clipsInt[currentClipIndex];
    }

    public int GetEyeClip()
    {
        return eyeClipsInt[currentClipIndex];
    }
}
