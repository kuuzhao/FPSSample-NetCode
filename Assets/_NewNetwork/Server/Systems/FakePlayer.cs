using UnityEngine;


// TODO: LZ:
//      to be removed
public class FakePlayer
{
    public static GameObject m_ThePlayer;
    public static Transform m_HeadTr;

    public static void PrepareFakePlayerIfNeeded()
    {
        if (m_ThePlayer != null)
            return;

        CharacterPresentationSetup[] cpSetups = Object.FindObjectsOfType<CharacterPresentationSetup>();
        if (cpSetups.Length > 0)
        {
            m_ThePlayer = cpSetups[0].gameObject;
            m_HeadTr = SearchHierarchyForBone(m_ThePlayer.transform, "Head");
        }
    }

    private static Transform SearchHierarchyForBone(Transform current, string name)
    {
        if (current.name == name)
            return current;

        for (int i = 0; i < current.childCount; ++i)
        {
            Transform found = SearchHierarchyForBone(current.GetChild(i), name);

            if (found != null)
                return found;
        }

        return null;
    }
}
