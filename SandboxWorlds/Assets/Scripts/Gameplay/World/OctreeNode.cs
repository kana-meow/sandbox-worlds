using System.Collections.Generic;
using UnityEngine;

public class OctreeNode {
    public bool isLeaf; // if true, this node has no children
    public Vector3Int position; // center of node
    public int size; // size of this cube
    public List<VoxelData> voxels; // holds voxels if leaf
    public OctreeNode[] children; // 8 children

    public bool IsFull {
        get {
            return OccupancyRatio >= 1f; // 1 = all spots have to be occupied for a node to be considered full
        }
    }

    public float OccupancyRatio {
        get {
            int capacity = size * size * size;
            float occupancyRatio = (float)voxels.Count / capacity;
            return occupancyRatio;
        }
    }

    public OctreeNode(Vector3Int position = default, int size = WorldData.OCTREE_CHUNK_SIZE, bool log = false) {
        this.position = position;
        this.size = size;
        isLeaf = true;
        children = null;
        voxels = new List<VoxelData>();
        if (log) Debug.Log("Created new Octree Node at " + position);
    }

    public OctreeNode(bool log = false, Vector3Int position = default, int size = WorldData.OCTREE_CHUNK_SIZE) {
        this.position = position;
        this.size = size;
        isLeaf = true;
        children = null;
        voxels = new List<VoxelData>();
        if (log) Debug.Log("Created new Octree Node at " + position);
    }

    public void Subdivide() {
        if (!isLeaf) {
            Debug.LogWarning("Tried to subdivide already divided Octree.");
            return; // prevent redundant subdivision
        }
        isLeaf = false;
        children = new OctreeNode[8];
        int childSize = size / 2;
        int offsetAmount = childSize / 2;

        for (int i = 0; i < 8; i++) {
            Vector3Int offset = new Vector3Int(
                (i & 1) == 0 ? -offsetAmount : offsetAmount,
                (i & 2) == 0 ? -offsetAmount : offsetAmount,
                (i & 4) == 0 ? -offsetAmount : offsetAmount
            );

            children[i] = new OctreeNode(position + offset, childSize);
        }
    }
}