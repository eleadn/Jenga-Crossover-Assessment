using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField]
    GameObject jengaPiece;
    [SerializeField]
    Material[] masteryMaterials = new Material[3];
    [SerializeField]
    TextMeshPro label;
    [SerializeField, Min(0f)]
    float distanceBetweenPieces;

    Skill[] skills;
    
    enum State
    {
        IDLE,
        TESTMYSTACK
    }

    State state = State.IDLE;

    public string Grade { get { return skills[0].grade; } }

    void TestMyStack()
    {
        if (state == State.TESTMYSTACK)
            return;

        for (int i = 1; i < transform.childCount; ++i)
        {
            if (skills[i - 1].mastery == 0)
                transform.GetChild(i).gameObject.SetActive(false);
            else
                transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
        }

        state = State.TESTMYSTACK;
    }

    void GoIdle()
    {
        if (state == State.IDLE)
            return;

        for (int i = 1; i < transform.childCount; ++i)
        {
            if (skills[i - 1].mastery == 0)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                Destroy(transform.GetChild(i).GetComponent<Rigidbody>());
        }

        BuildTower();

        state = State.IDLE;
    }

    public void SwitchTestMyStack(bool testStack)
    {
        if (testStack)
            TestMyStack();
        else
            GoIdle();
    }

    public string GetInfos(int i)
    {
        return skills[i].grade + ": " + skills[i].domain + "\n" +
            skills[i].cluster + "\n" + skills[i].standardid + ": " + skills[i].standarddescription;
    }

    public float GetHalfHeight()
    {
        int height = Mathf.FloorToInt(skills.Length / 3f);
        return (jengaPiece.transform.localScale.y + height * jengaPiece.transform.localScale.y) * 0.5f;
    }
    public float MaxWidth()
    {
        float widthZ = jengaPiece.transform.localScale.z;
        float widthX = jengaPiece.transform.localScale.x * 3f + 2f * distanceBetweenPieces;
        return widthZ > widthX ? widthZ : widthX;
    }

    public void CreateTower(Skill[] skills)
    {
        this.skills = skills;

        label.text = skills[0].grade;
        foreach (Skill skill in this.skills)
        {
            Transform piece = Instantiate(jengaPiece).GetComponent<Transform>();
            piece.parent = transform;
            piece.GetComponent<MeshRenderer>().material = masteryMaterials[skill.mastery];
        }

        BuildTower();
    }

    void PutPieceInPlace(Transform piece, int height, int orientation, int position)
    {
        Vector3 newPosition = Vector3.zero;
        piece.eulerAngles = Vector3.zero;

        newPosition.y = jengaPiece.transform.localScale.y * 0.5f + jengaPiece.transform.localScale.y * height;
        float posX = 0f;

        if (position == 0)
            posX = -jengaPiece.transform.localScale.x - distanceBetweenPieces;
        else if (position == 2)
            posX = jengaPiece.transform.localScale.x + distanceBetweenPieces;

        if (orientation == 0)
            newPosition.x = posX;
        else if (orientation == 1)
        {
            piece.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            newPosition.z = posX;
        }

        piece.transform.localPosition = newPosition;
    }

    void BuildTower()
    {
        for (int i = 0; (i + 1) < transform.childCount; ++i)
        {
            int height = Mathf.FloorToInt(i / 3f);
            int orientation = height % 2;
            int position = i % 3;

            PutPieceInPlace(transform.GetChild(i + 1), height, orientation, position);
        }
    }
}
