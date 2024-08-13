using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<AnimalStat> animals = new List<AnimalStat>();

    public List<AnimationClip> clips = new List<AnimationClip>();
    private List<int> clipsInt = new List<int>();
    public List<AnimationClip> eyeClips = new List<AnimationClip>();
    private List<int> eyeClipsInt = new List<int>();

    private int currentClipIndex = 0;
    public int maxPopulation = 1;
    public bool IsFull
    {
        get
        {
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

    private void Start()
    {
        var floorWayPoint = GetComponentInParent<FloorWaypoint>();

        floorWayPoint.waypoints.Add(this);

        for (int i = 0; i < clips.Count; ++i )
        {
            clipsInt.Add(Animator.StringToHash(clips[i].name));
        }        
        
        for (int i = 0; i < eyeClips.Count; ++i )
        {
            eyeClipsInt.Add(Animator.StringToHash(eyeClips[i].name));
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

    public int GetRandomClip()
    {
        currentClipIndex = Random.Range(0, clips.Count);
        return clipsInt[currentClipIndex];
    }

    public int GetEyeClip()
    {
        return eyeClipsInt[currentClipIndex];
    }
}
