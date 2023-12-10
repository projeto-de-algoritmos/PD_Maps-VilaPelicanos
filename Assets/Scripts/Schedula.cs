using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Schedula : MonoBehaviour
{
    public List<Task> tasks = new();
    List<Task> activeTasks = new();

    public void StartSchedule()
    {
        tasks = tasks.OrderBy(task => task.end).ToList();
        
        for (int i = 0; i < tasks.Count ; i++)
            tasks[i].transform.SetSiblingIndex(i);

        activeTasks.Clear();

        foreach (Task task in tasks)
            if (task.isActive)
                activeTasks.Add(task);

        Tuple<int, List<Task>> result = FindBetterSchedule(activeTasks.Count - 1, int.MaxValue);

        foreach (Task task in activeTasks)
            task.DesactiveTask();

        for (int i = 0; i < result.Item2.Count; i++)
        {
            result.Item2[i].ActiveTask();
            result.Item2[i].transform.SetSiblingIndex(i);
        }
    }

    public Tuple<int, List<Task>> FindBetterSchedule(int index, int time)
    {
        if (index == -1)
            return Tuple.Create(0, new List<Task>());
        else
        {
            int take = 0;
            List<Task> takeList = new List<Task>();

            if (activeTasks[index].end <= time)
            {
                Tuple<int, List<Task>> takeResult = FindBetterSchedule(index - 1, activeTasks[index].start);
                take = activeTasks[index].weight + takeResult.Item1;
                takeList = takeResult.Item2;
                takeList.Add(activeTasks[index]);
            }

            Tuple<int, List<Task>> notTakeResult = FindBetterSchedule(index - 1, time);
            int notTake = notTakeResult.Item1;
            List<Task> notTakeList = notTakeResult.Item2;

            if (take >= notTake)
                return Tuple.Create(take, takeList);
            else
                return Tuple.Create(notTake, notTakeList);
        }
    }
}
