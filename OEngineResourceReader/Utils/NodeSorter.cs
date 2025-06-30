using System.Collections;
using System.Runtime.InteropServices;

namespace OEngineResourceReader.Utils
{
    public class NodeSorter : IComparer
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        public int Compare(object? x, object? y)
        {
            TreeNode? nodeX = x as TreeNode;
            TreeNode? nodeY = y as TreeNode;

            if (nodeX == null || nodeY == null)
                return 0;

            bool isDirX = (nodeX.Nodes.Count > 0 && nodeX.Nodes[0].Text == "Loading...") || Directory.Exists(nodeX.Tag as string);
            bool isDirY = (nodeY.Nodes.Count > 0 && nodeY.Nodes[0].Text == "Loading...") || Directory.Exists(nodeY.Tag as string);

           
            if (isDirX && !isDirY)
            {
                return -1;
            }
            if (!isDirX && isDirY)
            {
                return 1;
            }

            return StrCmpLogicalW(nodeX.Text, nodeY.Text);
        }
    }
}
