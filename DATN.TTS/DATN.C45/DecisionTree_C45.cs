using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.C45
{
    public class DecisionTree_C45
    {
        #region Properties
        List<List<double>> Examples;
        List<Attribute> Attributes;
        TreeNode _tree;
        List<string> _ListMonHoc;
        int _depth;
        string _solution;
        private double diem1 = 0;
        private double diem2 = 0;

        public TreeNode Tree
        {
            get { return _tree; }
            set { _tree = value; }
        }

        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public string Solution
        {
            get { return _solution; }
            set { _solution = value; }
        }

        public List<string> ListMonHoc
        {
            get { return _ListMonHoc; }
            set { _ListMonHoc = value; }
        }

        public DecisionTree_C45(List<List<double>> Examples, List<Attribute> Attributes, double kq1, double kq2)
        {
            this.Examples = Examples;
            this.Attributes = Attributes;
            this.Tree = null;
            this.ListMonHoc = new List<string>();
            Depth = 0;
            diem1 = kq1;
            diem2 = kq2;
        }
        #endregion

        #region GetTree and Get depth tree
        private int GetDepth(TreeNode tree)
        {
            int depth;
            if (tree.Childs.Length == 0)
                return 1;
            else
            {
                depth = GetDepth(tree.Childs[0]);
                for (int i = 1; i < tree.Childs.Length; i++)
                {
                    int depthchild = GetDepth(tree.Childs[i]);
                    if (depth < depthchild)
                        depth = depthchild;
                }
                depth++;
            }
            return depth;
        }
        #endregion

        public void GetTree()
        {
            Solution = "";
            List<Attribute> at = new List<Attribute>();
            for (int i = 0; i < Attributes.Count; i++)
            {
                at.Add(Attributes[i]);
            }
            Tree = GetTreeNode_DecisionC45(Examples, at, "S");
            Depth = GetDepth(Tree);
        }

        private TreeNode GetTreeNode_DecisionC45(List<List<double>> Examples, List<Attribute> Attribute_Examp, string bestat)
        {
            Solution = Solution + "---------------------------------    Xét " + bestat + "     -------------------------------";
            if (CheckAllPositive(Examples))
            {
                return new TreeNode(new Attribute("10"));
            }
            if (CheckAllNegative(Examples))
            {
                return new TreeNode(new Attribute("0"));
            }
            if (Attribute_Examp.Count == 0)
            {
                return new TreeNode(new Attribute(GetMostCommonValue(Examples)));
            }
            Attribute Best_attributes = GetBestAttribute(Examples, Attribute_Examp, bestat);
            int LocationBA = Attribute_Examp.IndexOf(Best_attributes);
            TreeNode Root = new TreeNode(Best_attributes);
            for (int i = 0; i < Best_attributes.Value.Count; i++)
            {
                List<List<double>> Examplesvi = new List<List<double>>();
                for (int j = 0; j < Examples.Count; j++)
                {
                    double x = (double)Examples[j][LocationBA];
                    double y = Convert.ToDouble(Best_attributes.Value[i].ToString());
                    if (x == y)
                        Examplesvi.Add(Examples[j]);
                }
                if (Examplesvi.Count == 0)
                {
                    Solution += "\n Các thuộc tính rỗng => Trả về nút gốc có giá trị phổ biến nhất ";
                    return new TreeNode(new Attribute(GetMostCommonValue(Examplesvi)));
                }
                else
                {
                    Solution += "\n";
                    Attribute_Examp.Remove(Best_attributes);
                    Root.AddNode(GetTreeNode_DecisionC45(Examplesvi, Attribute_Examp, Best_attributes.Value[i].ToString()));
                }
            }
            return Root;
        }

        private Attribute GetBestAttribute(List<List<double>> Examples, List<Attribute> Attributes, string bestat)
        {
            double maxGain = GainRatio(Examples, Attributes[0], bestat);
            int max = 0;
            for (int i = 1; i < Attributes.Count; i++)
            {
                double GainCurrent = GainRatio(Examples, Attributes[i], bestat);
                if (maxGain < GainCurrent)
                {
                    maxGain = GainCurrent;
                    max = i;
                }
            }
            Solution = Solution + "\n\t=> Ta chọn đặc tính tốt nhất là : " + Attributes[max].Name;
            return Attributes[max];
        }

        private double GainRatio(List<List<double>> Examples, Attribute A, string bestat)
        {
            double gainRatio = 0;
            double Entropy = 0;
            double EntropyCurrent = 0;
            double GainValue = 0;
            int CountPoSitive = 0;
            double SplitInformation = 0;
            int[] CountPositivesA = new int[A.Value.Count];
            int[] CountNegativeA = new int[A.Value.Count];
            int Col = Attributes.IndexOf(A);
            for (int i = 0; i < A.Value.Count; i++)
            {
                CountPositivesA[i] = 0;
                CountNegativeA[i] = 0;
            }
            for (int i = 0; i < Examples.Count; i++)
            {
                int j = A.Value.IndexOf(Examples[i][Col]);
                int x = Examples[0].Count - 1;
                int y = Convert.ToInt32((Examples[i][x]));
                if (y == diem2)
                {
                    if (j >= 0)
                    {
                        CountPoSitive++;
                        CountPositivesA[j]++;
                    }
                }
                else
                {
                    if (j >= 0)
                    {
                        CountNegativeA[j]++;
                    }
                }
            }
            Entropy = GetEntropy(CountPoSitive, Examples.Count - CountPoSitive);
            for (int t = 0; t < A.Value.Count; t++)
            {
                int PoSitive = CountPositivesA[t];
                int Negative = CountNegativeA[t];
                double RateValue = (double)(CountPositivesA[t] + CountNegativeA[t]) / Examples.Count;
                EntropyCurrent += RateValue * GetEntropy(PoSitive, Negative);
                SplitInformation += RateValue * Math.Log(RateValue, 2);
            }
            GainValue = Entropy - EntropyCurrent;
            gainRatio = (double)GainValue / (-SplitInformation);//result = GetEntropy(CountPoSitive, Examples.Count - CountPoSitive) - result;
            Solution = Solution + "\n * Gain(" + bestat + "," + A.Name + ") = " + gainRatio.ToString();
            return gainRatio;
        }

        private double GetEntropy(int Positives, int Negatives)
        {
            double Entropy = 0;
            if (Positives == 0)
                return 0;
            if (Negatives == 0)
                return 0;

            int total = Negatives + Positives;// tổng phần tử trên tập đang xét
            double RatePositves = (double)Positives / total;// tập khẳng định
            double RateNegatives = (double)Negatives / total;// tập phủ định

            Entropy = (-RatePositves * Math.Log(RatePositves, 2)) - (RateNegatives * Math.Log(RateNegatives, 2));

            return Entropy;
        }

        private string GetMostCommonValue(List<List<double>> Examples)
        {
            int CountPositive = 0; for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1] == diem1)
                    CountPositive++;
            }
            int CountNegative = Examples.Count - CountPositive;
            string Label;
            if (CountPositive > CountNegative)
                Label = 0.ToString();
            else
                Label = 10.ToString();
            Solution = Solution + " là " + Label;
            return Label;
        }

        private bool CheckAllPositive(List<List<double>> Examples)
        {
            for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1] == diem1)
                    return false;
            }
            return true;
        }

        private bool CheckAllNegative(List<List<double>> Examples)
        {

            for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1] == diem2)
                    return false;
            }
            return true;
        }

        // Tìm giá trị 
        public List<double> SearchTree(TreeNode tree, DataTable iDataSource, List<double> lst_result, bool kt)
        {
            if (!string.IsNullOrEmpty(tree.Attribute.Label))
            {
                if (kt == true)
                {
                    lst_result.Add(Convert.ToDouble(tree.Attribute.Label));
                }
            }
            else
            {
                // Kiểm tra giá trị tại node đang xét có tồn tại trong đk đưa vào
                if (!string.IsNullOrEmpty(iDataSource.Rows[0][tree.Attribute.Name.Trim()].ToString()))
                {
                    int check = 0;
                    for (int i = 0; i < tree.Attribute.Value.Count; i++)
                    {
                        if (Convert.ToDouble(iDataSource.Rows[0][tree.Attribute.Name.Trim()]) ==
                            tree.Attribute.Value[i])
                        {
                            lst_result = SearchTree(tree.Childs[i], iDataSource, lst_result, true);
                            check++;
                        }
                    }
                    if (check == 0)
                    {
                        foreach (TreeNode trnode in tree.Childs)
                        {
                            lst_result = SearchTree(trnode, iDataSource, lst_result, false);
                        }
                    }

                }
                else
                {
                    foreach (TreeNode trnode in tree.Childs)
                    {
                        lst_result = SearchTree(trnode, iDataSource, lst_result, false);
                    }
                }
            }
            return lst_result;
        }
    }
}
