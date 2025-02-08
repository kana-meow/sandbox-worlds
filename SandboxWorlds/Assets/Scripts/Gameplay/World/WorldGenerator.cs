using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class WorldGenerator : MonoBehaviour {
    public List<OctreeNode> rootNodes = new();

    [Header("World Settings")]
    public int heightInChunks = 2;
    public int widthInChunks = 2;
    public int minGroundHeight = 40;

    [Header("Debug")]
    public float childNodeSizeMult = .95f;
    public bool showOctreeGizmo = true;
    //public bool showAllVoxels = false;
    //public bool showSurfaceOnly = true;
    //[Range(0.1f, 1f)] public float voxelRenderSize;
    //public bool renderTreeFirst = false;

    private NoiseLayer _noiseLayer;

    public NoiseLayer NoiseLayer {
        get {
            if (_noiseLayer == null) {
                _noiseLayer = GetComponent<NoiseLayer>();
            }

            return _noiseLayer;
        }
    }

    public void Generate() {
        Reset();

        // Generate each Root Octree
        int chunkSize = WorldData.OCTREE_CHUNK_SIZE;
        for (int x = -widthInChunks / 2; x < (widthInChunks + 1) / 2; x++) {
            for (int y = 0; y < heightInChunks; y++) { // y=0 is the very bottom
                for (int z = -widthInChunks / 2; z < (widthInChunks + 1) / 2; z++) {
                    rootNodes.Add(new OctreeNode(new Vector3Int(x * chunkSize, y * chunkSize + chunkSize / 2, z * chunkSize)));
                }
            }
        }
        Debug.Log($"Generated world of size {widthInChunks * chunkSize}x{heightInChunks * chunkSize}x{widthInChunks * chunkSize} ({widthInChunks}x{heightInChunks}x{widthInChunks} chunks)");

        foreach (OctreeNode rootNode in rootNodes) {
            PopulateOctree(rootNode);
            SubdivideNodeByVoxels(rootNode);
        }
        GenerateMesh();
    }

    public void Reset() {
        rootNodes.Clear();
    }

    private void PopulateOctree(OctreeNode node) {
        int voxels = 0; // track for debug log, no other use

        // go through each point in node, while concidering center position of node
        int chunkHalfSize = WorldData.OCTREE_CHUNK_SIZE / 2;
        for (int x = -chunkHalfSize; x < chunkHalfSize; x++) {
            for (int y = -chunkHalfSize; y < chunkHalfSize; y++) {
                for (int z = -chunkHalfSize; z < chunkHalfSize; z++) {
                    if (IsSolid(new Vector3Int(x, y, z) + node.position)) {
                        voxels++;
                        node.voxels.Add(new VoxelData {
                            position = node.position + new Vector3Int(x, y, z),
                            blockID = Random.Range(0, 1)
                        });
                    }
                }
            }
        }
        Debug.Log("Generated " + voxels + " voxels.");
    }

    private void SubdivideNodeByVoxels(OctreeNode node) {
        if (node.isLeaf && node.voxels.Count > WorldData.MIN_VOXELS_PER_NODE * 2 && !node.IsFull) {
            node.Subdivide(); // subdivide this node into 8 children
                              // distribute voxels into child nodes
            foreach (VoxelData voxel in node.voxels) {
                int index = (voxel.position.x >= node.position.x ? 1 : 0)
                            | (voxel.position.y >= node.position.y ? 2 : 0)
                            | (voxel.position.z >= node.position.z ? 4 : 0);
                node.children[index].voxels.Add(voxel);
            }
            node.voxels.Clear();
            foreach (OctreeNode child in node.children) {
                SubdivideNodeByVoxels(child);
            }
        }
    }

    private void GenerateMesh() {
        List<VoxelData> voxelData = new List<VoxelData>();
        foreach (OctreeNode rootNode in rootNodes) {
            if (rootNode.isLeaf) {
                voxelData.AddRange(rootNode.voxels);
            } else {
                GetVoxelData(rootNode, voxelData);
            }
        }
        Debug.Log("Collected " + voxelData.Count + " voxels");

        Mesh mesh = new Mesh();
        foreach (VoxelData voxel in voxelData) {
            int mask = GetExposedFaces(voxel.position);

            if ((mask & (1 << 0)) != 0) AddQuad(mesh, voxel.position, Vector3.right);   // +x (right)
            if ((mask & (1 << 1)) != 0) AddQuad(mesh, voxel.position, Vector3.left);    // -x (left)
            if ((mask & (1 << 2)) != 0) AddQuad(mesh, voxel.position, Vector3.up);      // +y (up)
            if ((mask & (1 << 3)) != 0) AddQuad(mesh, voxel.position, Vector3.down);    // -y (down)
            if ((mask & (1 << 4)) != 0) AddQuad(mesh, voxel.position, Vector3.forward); // +z (forward)
            if ((mask & (1 << 5)) != 0) AddQuad(mesh, voxel.position, Vector3.back);    // -z (back)
        }
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>();
    }

    private void GetVoxelData(OctreeNode node, List<VoxelData> voxelData) {
        if (node.isLeaf) {
            voxelData.AddRange(node.voxels);
        } else {
            foreach (OctreeNode child in node.children) {
                GetVoxelData(child, voxelData);
            }
        }
    }

    private void AddQuad(Mesh mesh, Vector3Int position, Vector3 normal) {
        Vector3[] quadVerts = GetQuadVerticies(position, normal);

        mesh.vertices = quadVerts;
    }

    private Vector3[] GetQuadVerticies(Vector3Int position, Vector3 normal) {
        Vector3[] quad = new Vector3[4];

        // X+ (Right)
        if (normal == Vector3Int.right) {
            quad[0] = position + new Vector3(1, 0, 0);
            quad[1] = position + new Vector3(1, 0, 1);
            quad[2] = position + new Vector3(1, 1, 0);
            quad[3] = position + new Vector3(1, 1, 1);
        }
        // X- (Left)
        else if (normal == Vector3Int.left) {
            quad[0] = position + new Vector3(0, 0, 1);
            quad[1] = position + new Vector3(0, 0, 0);
            quad[2] = position + new Vector3(0, 1, 1);
            quad[3] = position + new Vector3(0, 1, 0);
        }
        // Y+ (Top)
        else if (normal == Vector3Int.up) {
            quad[0] = position + new Vector3(0, 1, 0);
            quad[1] = position + new Vector3(1, 1, 0);
            quad[2] = position + new Vector3(0, 1, 1);
            quad[3] = position + new Vector3(1, 1, 1);
        }
        // Y- (Bottom)
        else if (normal == Vector3Int.down) {
            quad[0] = position + new Vector3(0, 0, 1);
            quad[1] = position + new Vector3(1, 0, 1);
            quad[2] = position + new Vector3(0, 0, 0);
            quad[3] = position + new Vector3(1, 0, 0);
        }
        // Z+ (Front)
        else if (normal == Vector3Int.forward) {
            quad[0] = position + new Vector3(0, 0, 1);
            quad[1] = position + new Vector3(1, 0, 1);
            quad[2] = position + new Vector3(0, 1, 1);
            quad[3] = position + new Vector3(1, 1, 1);
        }
        // Z- (Back)
        else if (normal == Vector3Int.back) {
            quad[0] = position + new Vector3(1, 0, 0);
            quad[1] = position + new Vector3(0, 0, 0);
            quad[2] = position + new Vector3(1, 1, 0);
            quad[3] = position + new Vector3(0, 1, 0);
        }

        return quad;
    }

    private float GetNoiseValue(Vector3Int pos) {
        return minGroundHeight + Mathf.FloorToInt(NoiseLayer.GetFinalNoise(pos.x, pos.z)) + 1;
    }

    private bool IsSolid(Vector3Int pos) {
        return pos.y <= GetNoiseValue(pos);
    }

    private bool IsSurface(Vector3Int pos) {
        return pos.y == GetNoiseValue(pos);
    }

    private int GetExposedFaces(Vector3Int pos) {
        int mask = 0;

        // write to the bitmask which faces are exposed
        /* How it works:
        *   if for example the right (+x) and the bottom (-y) face are exposed, the output it: 0b001001
        *   (One bit for each possible direction)
        */
        if (!IsSolid(pos + Vector3Int.right)) mask |= (1 << 0); // +x
        if (!IsSolid(pos + Vector3Int.left)) mask |= (1 << 1); // -x
        if (!IsSolid(pos + Vector3Int.up)) mask |= (1 << 2); // +y
        if (!IsSolid(pos + Vector3Int.down)) mask |= (1 << 3); // -y
        if (!IsSolid(pos + Vector3Int.forward)) mask |= (1 << 4); // +z
        if (!IsSolid(pos + Vector3Int.back)) mask |= (1 << 5); // -z

        return mask;
    }

    // generate tree structure
    private void DrawOctreeChildren(OctreeNode node) {
        foreach (OctreeNode child in node.children) {
            if (child.isLeaf) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(child.position, child.size * childNodeSizeMult * Vector3.one);
            } else {
                //Gizmos.color = Color.red;
                //Gizmos.DrawWireCube(child.position, child.size * childNodeSizeMult * Vector3.one);
                DrawOctreeChildren(child);
            }
        }
    }

    private void OnDrawGizmos() {
        foreach (OctreeNode rootNode in rootNodes) {
            if (showOctreeGizmo && rootNode != null) {
                if (rootNode.isLeaf) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                } else {
                    //Gizmos.color = Color.red;
                    //Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                    DrawOctreeChildren(rootNode);
                }
            }
        }
    }

    /* Gizmos

    private void DrawVoxelInChild(OctreeNode node) {
        foreach (OctreeNode child in node.children) {
            if (child.isLeaf) {
                foreach (VoxelData voxel in child.voxels) {
                    Gizmos.color = Color.white;
                    if (showSurfaceOnly) {
                        if (IsExposed(voxel.position))
                            if (voxelRenderType == VoxelRenderType.Blocks) {
                                Gizmos.DrawCube((Vector3)voxel.position + (Vector3.one * 0.5f), Vector3.one * voxelRenderSize);
                            } else if (voxelRenderType == VoxelRenderType.PointCloud) {
                                Gizmos.DrawSphere((Vector3)voxel.position + (Vector3.one * 0.5f), voxelRenderSize);
                            }
                    } else {
                        if (voxelRenderType == VoxelRenderType.Blocks) {
                            Gizmos.DrawCube((Vector3)voxel.position + (Vector3.one * 0.5f), Vector3.one * voxelRenderSize);
                        } else if (voxelRenderType == VoxelRenderType.PointCloud) {
                            Gizmos.DrawSphere((Vector3)voxel.position + (Vector3.one * 0.5f), voxelRenderSize);
                        }
                    }
                }
            } else {
                DrawVoxelInChild(child);
            }
        }
    }

    private void OnDrawGizmos() {
        if (renderTreeFirst) {
            foreach (OctreeNode rootNode in rootNodes) {
                if (showAllVoxels && rootNode != null) {
                    if (!rootNode.isLeaf) DrawVoxelInChild(rootNode);
                }
                if (showOctreeGizmo && rootNode != null) {
                    if (rootNode.isLeaf) {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                    } else {
                        //Gizmos.color = Color.red;
                        //Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                        DrawOctreeChildren(rootNode);
                    }
                }
            }
        } else {
            foreach (OctreeNode rootNode in rootNodes) {
                if (showOctreeGizmo && rootNode != null) {
                    if (rootNode.isLeaf) {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                    } else {
                        //Gizmos.color = Color.red;
                        //Gizmos.DrawWireCube(rootNode.position, Vector3.one * rootNode.size);
                        DrawOctreeChildren(rootNode);
                    }
                }
            }
        }
        foreach (OctreeNode rootNode in rootNodes) {
            if (showAllVoxels && rootNode != null) {
                if (!rootNode.isLeaf) DrawVoxelInChild(rootNode);
                else {
                    foreach (VoxelData voxel in rootNode.voxels) {
                        Gizmos.color = Color.white;
                        if (showSurfaceOnly) {
                            if (IsExposed(voxel.position))
                                if (voxelRenderType == VoxelRenderType.Blocks) {
                                    Gizmos.DrawCube((Vector3)voxel.position + (Vector3.one * 0.5f), Vector3.one * voxelRenderSize);
                                } else if (voxelRenderType == VoxelRenderType.PointCloud) {
                                    Gizmos.DrawSphere((Vector3)voxel.position + (Vector3.one * 0.5f), voxelRenderSize);
                                }
                        } else {
                            if (voxelRenderType == VoxelRenderType.Blocks) {
                                Gizmos.DrawCube((Vector3)voxel.position + (Vector3.one * 0.5f), Vector3.one * voxelRenderSize);
                            } else if (voxelRenderType == VoxelRenderType.PointCloud) {
                                Gizmos.DrawSphere((Vector3)voxel.position + (Vector3.one * 0.5f), voxelRenderSize);
                            }
                        }
                    }
                }
            }
        }
    }*/
}