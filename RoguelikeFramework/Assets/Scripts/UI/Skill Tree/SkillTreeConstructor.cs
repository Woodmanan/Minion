using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class SkillTreeConstructor : MonoBehaviour
{
    public SkillTree skillTree;
    public GameObject skillUIItem;

    [SerializeField] private bool resetSkillLevelsOnRestart = true;

    [SerializeField] private Vector2 offset = new Vector2(200, 125);
    [SerializeField] private Vector2 start = new Vector2(0, -250);

    [SerializeField] private float lineThickness = 10;

    // Data structure setup
    Queue q;
    Dictionary<SkillNode, int> depthDict;
    Dictionary<SkillNode, GameObject> goDict;
    HashSet<SkillNode> skillNodesFound;

    int numRoots = 0;
    int numRootsRemaining = 0;

    // Start is called before the first frame update
    void Start()
    {
        q = new Queue();
        depthDict = new Dictionary<SkillNode, int>();
        goDict = new Dictionary<SkillNode, GameObject>();
        skillNodesFound = new HashSet<SkillNode>();
        BuildTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildTree()
    {
        // Initializing the queue and dictionaries with the root nodes
        foreach (SkillNode skillNode in skillTree.nodes)
        {
            if (skillNode.IsRoot())
            {
                depthDict.Add(skillNode, 0);
                q.Enqueue(skillNode);

                // Only used for roots right now, probably implement this as a callback as well
                skillTree.availableSkills.Add(skillNode);
            }
        }

        // Loop while nodes still exist that we haven't observed yet
        while (q.Count > 0)
        {
            if (numRootsRemaining == 0)
            {
                numRoots = q.Count;
                numRootsRemaining = numRoots;
            }

            SkillNode skillNode = (SkillNode) q.Dequeue();
            numRootsRemaining--;
            List<NodePort> ports = null;
            if (!skillNode.IsRoot())
            {
                ports = skillNode.GetAllRoots();
                depthDict[skillNode] = depthDict[(SkillNode) ports[0].node] + 1;
            }
            if (!goDict.ContainsKey(skillNode)) goDict.Add(skillNode, DrawNode(skillNode));

            if (ports != null)
            {
                foreach (NodePort port in ports)
                {
                    SkillNode root = (SkillNode) port.node;
                    DrawLine(goDict[root].GetComponent<SkillUIItem>().childLineAnchor, goDict[skillNode].GetComponent<SkillUIItem>().rootLineAnchor, skillNode.purchaseAllRootsFirst ? Color.yellow : Color.black);
                }
            }

            foreach (NodePort port in skillNode.GetAllChildren())
            {
                SkillNode child = (SkillNode) port.node;

                if (depthDict.ContainsKey(child))
                {
                    // Potentially also need to shift this node up then
                    // This is probably not needed
                }
                else
                {
                    if (!skillNodesFound.Contains(child)) q.Enqueue(child);
                }
                skillNodesFound.Add(child);
            }

            // Reset the ScriptableObject skill level
            if (resetSkillLevelsOnRestart) skillNode.ResetSkillLevel();
        }
    }

    GameObject DrawNode(SkillNode skillNode)
    {
        float xPos = start.x;
        if (numRoots % 2 == 0) xPos += -((numRoots / 2) - 0.5f) * offset.x + (offset.x * (numRoots - (numRootsRemaining + 1)));
        else xPos += -((numRoots - 1) / 2) * offset.x + (offset.x * (numRoots - (numRootsRemaining + 1)));

        // If you want to just use the graph position, use this line below and comment out the above
        // float xPos = skillNode.position.y;

        float yPos = start.y + depthDict[skillNode] + (depthDict[skillNode] * Mathf.Abs(offset.y));

        GameObject uiItem = Instantiate(skillUIItem, transform);
        uiItem.transform.localPosition = new Vector3(xPos, yPos, 0);
        uiItem.GetComponent<SkillUIItem>().skillNode = skillNode;

        return uiItem;
    }

    // Source: https://forum.unity.com/threads/any-good-way-to-draw-lines-between-ui-elements.317902/
    void DrawLine(RectTransform root, RectTransform child, Color color)
    {
        GameObject line = new GameObject();
        Image newImage = line.AddComponent<Image>();
        newImage.color = color;
        RectTransform rect = line.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;

        Vector3 midpoint = (root.transform.position + child.transform.position) / 2;
        rect.position = midpoint;
        Vector3 dif = root.transform.position - child.transform.position;
        rect.sizeDelta = new Vector3(dif.magnitude, lineThickness);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        rect.SetParent(root);
    }
}
