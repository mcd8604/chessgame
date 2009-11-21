using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChessLib
{
    public partial class TreeViewForm : Form
    {
        public TreeViewForm()
        {
            InitializeComponent();
            addNodeDelegate += new AddNodeDelegate(addNode);
            setNodeDelegate += new SetNodeDelegate(setNode);
        }

        private delegate void AddNodeDelegate(TreeNode parent, TreeNode child);
        private AddNodeDelegate addNodeDelegate;

        private delegate void SetNodeDelegate(TreeNode curNode, string text);
        private SetNodeDelegate setNodeDelegate;

        private void addNode(TreeNode parent, TreeNode child)
        {
            if (parent == null)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(child);
            }
            else
            {
                parent.Nodes.Add(child);
                parent.Expand();
            }
        }

        public void AddNode(TreeNode parent, TreeNode child)
        {
            object[] args = { parent, child };
            Invoke(addNodeDelegate, args);
        }

        private void setNode(TreeNode curNode, string text)
        {
            curNode.Text = text;
        }

        internal void SetNode(TreeNode curNode, string text)
        {
            object[] args = { curNode, text };
            Invoke(setNodeDelegate, args);
        }
    }
}
