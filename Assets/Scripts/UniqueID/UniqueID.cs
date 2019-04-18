using UnityEngine;
using System.Collections;

// Placeholder for UniqueIdDrawer script
public class UniqueIdentifierAttribute : PropertyAttribute { }

public class UniqueID : MonoBehaviour
{
    [UniqueIdentifier]
    public string guid;
}