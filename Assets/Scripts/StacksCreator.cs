using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacksCreator : MonoBehaviour
{
    [SerializeField]
    Manager manager;
    [SerializeField]
    TextAsset stacks;
    [SerializeField]
    GameObject tower;
    [SerializeField, Min(0f)]
    float distanceBetweenTowers;

    private class Comparer : IComparer<Skill>
    {
        public int Compare(Skill s1, Skill s2)
        {
            int result = s1.domain.CompareTo(s2.domain);
            if (result == 0)
            {
                result = s1.cluster.CompareTo(s2.cluster);

                if (result == 0)
                    result = s1.standardid.CompareTo(s2.standardid);
            }

            return result;
        }
    }

    Skill[] GetSkills()
    {
        return JsonArray.Retrieve<Skill>(stacks.text);
    }

    List<List<Skill>> SegregateSkillsByGrade(Skill[] skills)
    {
        List<List<Skill>> segregated = new List<List<Skill>>();
        List<string> existing = new List<string>();

        foreach (Skill skill in skills)
        {
            if (existing.Contains(skill.grade))
                segregated[existing.IndexOf(skill.grade)].Add(skill);
            else
            {
                segregated.Add(new List<Skill> { skill });
                existing.Add(skill.grade);
            }
        }

        return segregated;
    }

    void SortSkills(List<Skill> toSort)
    {
        toSort.Sort(new Comparer());
    }

    void SortSkills(List<List<Skill>> toSort)
    {
        foreach (List<Skill> skills in toSort)
            SortSkills(skills);
    }

    void CreateTower(List<Skill> skills, int position)
    {
        Tower newTower = Instantiate(tower).GetComponent<Tower>();
        float posX = position * (distanceBetweenTowers + newTower.MaxWidth());
        newTower.transform.position = Vector3.zero + new Vector3(posX, 0f, 0f);
        newTower.CreateTower(skills.ToArray());
        manager.AddTower(newTower);
    }

    void CreateTowers()
    {
        var skills = SegregateSkillsByGrade(GetSkills());
        SortSkills(skills);

        for (int i = 0; i < skills.Count; ++i)
            CreateTower(skills[i], i);
    }

    private void Start()
    {
        if (!stacks || !tower)
            return;

        CreateTowers();
    }
}
