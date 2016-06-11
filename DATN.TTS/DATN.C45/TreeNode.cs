using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.C45
{
    public class TreeNode
    {
        private Attribute _attributes;
        private TreeNode[] _childs;
        private int n;
        private int _numberLabel;

        public Attribute Attribute
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public TreeNode[] Childs
        {
            get { return _childs; }
            set { _childs = value; }
        }

        public int NumberLabel
        {
            get { return _numberLabel; }
            set { _numberLabel = value; }
        }

        public TreeNode(Attribute Attributes)
        {
            this.Attribute = Attributes;
            this.Childs = new TreeNode[Attributes.Value.Count];
            n = 0;
            for (int i = 0; i < Attributes.Value.Count; i++)
            {
                Childs[i] = null;
            }
            if (Attributes.Value.Count == 0)
                NumberLabel = 1;
            else
                NumberLabel = 0;
        }

        public void AddNode(TreeNode Child)
        {
            if (n < Childs.Length)
            {
                Childs[n] = Child;
                NumberLabel = NumberLabel + Child.NumberLabel;
            }
            n++;
        }
    }
}
