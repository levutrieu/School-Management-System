using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ID3
{
    public class TTS_ID3
    {
        List<List<double>> Examples;
        List<Attribute> Attributes;
        TreeNode _tree;
        int _depth;
        string _solution;
        private double diem_xet1 = 0;
        private double diem_xet2 = 0;

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

        public TTS_ID3(List<List<double>> Examples, List<Attribute> Attributes, double Diem_Xet1, double Diem_Xet2)
        {
            this.Examples = Examples;
            this.Attributes = Attributes;
            this.Tree = null;
            Depth = 0;
            this.diem_xet1 = Diem_Xet1;
            this.diem_xet2 = Diem_Xet2;
        }

        // tính entroypy

        private double GetEntropy(int Positives , int Negatives)
        {
            if (Positives == 0)
                return 0;
            if (Negatives == 0)
                return 0;
            double Entropy;
            int total = Negatives + Positives;
            double RatePositves = (double)Positives / total;
            double RateNegatives = (double)Negatives / total;
            Entropy = -RatePositves * Math.Log(RatePositves, 2) - RateNegatives * Math.Log(RateNegatives, 2);
            return Entropy;
        }

        // tính Gain(bestat,A);

        private double Gain(List<List<double>> Examples, Attribute A, string bestat)
        {
            double result;
            int CountPositives = 0;
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
                if (Examples[i][Examples[0].Count - 1] == diem_xet1)
                {
                    if (j >= 0)
                    {
                        CountPositives++;
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
            result = GetEntropy(CountPositives, Examples.Count - CountPositives);
            for (int i = 0; i < A.Value.Count; i++)
            {
                double RateValue = (double)(CountPositivesA[i] + CountNegativeA[i]) / Examples.Count;
                result = result - RateValue * GetEntropy(CountPositivesA[i], CountNegativeA[i]);
            }
            
            Solution = Solution + "\n * Gain(" + bestat + "," + A.Name + ") = " + result.ToString();            
            return result;
        }

        // giải thuật ID3

        private TreeNode ID3(List<List<double>> Examples, List<Attribute> Attribute, string bestat)
        {
            Solution = Solution + "---------------------------------    Xét " + bestat + "     -------------------------------";
            if (CheckAllPositive(Examples))
            {
                Solution += "\n Tất cả các mẫu đều khẳng định => Trả về nút gốc với nhãn" + diem_xet1.ToString();
                return new TreeNode(new Attribute(diem_xet1.ToString()));
            }
            if (CheckAllNegative(Examples))
            {
                Solution += "\n Tất cả các mẫu đều phủ định => Trả về nút gốc với nhãn " + diem_xet2.ToString();
                return new TreeNode(new Attribute(diem_xet2.ToString()));
            }
            if (Attribute.Count == 0)
            {
                Solution += "\n Các thuộc tính rỗng => Trả về nút gốc có giá trị phổ biến nhất ";
                return new TreeNode(new Attribute(GetMostCommonValue(Examples)));
            }
            Attribute BestAttribute = GetBestAttribute(Examples, Attribute, bestat);
            int LocationBA = Attributes.IndexOf(BestAttribute);
            TreeNode Root = new TreeNode(BestAttribute);
            for (int i = 0; i < BestAttribute.Value.Count; i++)
            {
                List<List<double>> Examplesvi = new List<List<double>>();
                for (int j = 0; j < Examples.Count; j++)
                {
                    if (Examples[j][LocationBA] == BestAttribute.Value[i])
                        Examplesvi.Add(Examples[j]);
                }
                if (Examplesvi.Count==0)
                {
                    Solution += "\n Các thuộc tính rỗng => Trả về nút gốc có giá trị phổ biến nhất ";
                    return new TreeNode(new Attribute(GetMostCommonValue(Examplesvi)));
                }
                else
                {
                    Solution += "\n";
                    Attribute.Remove(BestAttribute);
                    Root.AddNode(ID3(Examplesvi, Attribute,BestAttribute.Value[i].ToString()));
                }
            }
            return Root;
        }

        // lấy thuật tính có Gain cao nhất

        private Attribute GetBestAttribute(List<List<double>> Examples, List<Attribute> Attributes, string bestat)
        {
            double MaxGain = Gain(Examples, Attributes[0], bestat);
            int Max = 0;
            for (int i = 1; i < Attributes.Count; i++)
            {
                double GainCurrent = Gain(Examples, Attributes[i], bestat);
                if (MaxGain < GainCurrent)
                {
                    MaxGain = GainCurrent;
                    Max = i;
                }
            }
            Solution = Solution + "\n\t=> Ta chọn đặc tính tốt nhất là : " + Attributes[Max].Name ;
            return Attributes[Max];
        }

        // lấy giá trị phổ biến nhất của tập đích

        private string GetMostCommonValue(List<List<double>> Examples)
        {
            int CountPositive = 0;
            for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1]== diem_xet1)
                    CountPositive++;
            }
            int CountNegative = Examples.Count - CountPositive;
            string Label;
            if (CountPositive > CountNegative)
                Label = diem_xet1.ToString();
            else
                Label = diem_xet2.ToString();
            Solution = Solution + " là " + Label;
            return Label;
        }

        // kiểm tra xem tất cả tập có phải là positive không

        private bool CheckAllPositive(List<List<double>> Examples)
        {
            for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1]== diem_xet2)
                    return false;
            }
            return true;
        }

        // kiểm tra xem tất cả tập có phải là Negative không

        private bool CheckAllNegative(List<List<double>> Examples)
        {
            for (int i = 0; i < Examples.Count; i++)
            {
                if (Examples[i][Examples[0].Count - 1]== diem_xet1)
                    return false;
            }
            return true;
        }

        // xây dựng cây

        public void GetTree()
        {
            Solution = "";
            List<Attribute> at = new List<Attribute>();
            for (int i = 0; i < Attributes.Count; i++)
            {
                at.Add(Attributes[i]);
            }
            Tree = ID3(Examples, at, "S");
            Depth = GetDepth(Tree);
        }

        // lấy độ sâu của cây

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

        // Tìm giá trị 
        public List<double> SearchTree(TreeNode tree, DataTable iDataSource, List<double> lst_result, bool kt)
        {
            if (!string.IsNullOrEmpty(tree.Attributes.Label))
            {
                if (kt == true)
                {
                    lst_result.Add(Convert.ToDouble(tree.Attributes.Label));
                }
            }
            else
            {
                // Kiểm tra giá trị tại node đang xét có tồn tại trong đk đưa vào
                if (!string.IsNullOrEmpty(iDataSource.Rows[0][tree.Attributes.Name.Trim()].ToString()))
                {
                    int check = 0;
                    for (int i = 0; i < tree.Attributes.Value.Count; i++)
                    {
                        if (Convert.ToDouble(iDataSource.Rows[0][tree.Attributes.Name.Trim()]) ==
                            tree.Attributes.Value[i])
                        {
                            lst_result = SearchTree(tree.Childs[i], iDataSource, lst_result, true);
                            check ++;
                        }
                    }
                    if (check == 0)
                    {
                        foreach (TreeNode trnode in tree.Childs)
                        {
                            lst_result = SearchTree(trnode, iDataSource, lst_result,false);
                        }
                    }

                }
                else
                {
                    foreach (TreeNode trnode in tree.Childs)
                    {
                        lst_result = SearchTree(trnode, iDataSource, lst_result,false);
                    }
                }
            }
            return lst_result;
        }

    }
}
