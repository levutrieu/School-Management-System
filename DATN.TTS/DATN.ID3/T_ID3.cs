using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ID3
{
    public class T_ID3
    {
        List<List<double>> Examples;
        List<Attribute> Attributes;
        TreeNode _tree;
        int _depth;
        string _solution;
        private List<double> dsKQ;
        private string diemKT = "";
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

        public T_ID3(List<List<double>> _Examples, List<Attribute> _Attributes, List<double> dsKQua)
        {
            this.Examples = _Examples;
            this.Attributes = _Attributes;
            this.Tree = null;
            Depth = 0;
            this.dsKQ = dsKQua;
        }

        // giải thuật ID3

        private TreeNode ID3(List<List<double>> Examples, List<Attribute> Attribute, string bestat)
        {
            Solution = Solution + "---------------------------------    Xét " + bestat + "     -------------------------------";
            if (CheckAllLaKQDuyNhat(Examples))
            {
                Solution += "\n Tất cả các mẫu đều khẳng định => Trả về nút gốc với nhãn" + diemKT;
                string diem = diemKT;
                diemKT = "";
                return new TreeNode(new Attribute(diem));
            }
            if (Attribute.Count == 0)
            {
                Solution += "\n Các thuộc tính rỗng => Trả về nút gốc có giá trị phổ biến nhất ";
                return new TreeNode(new Attribute(GetMostCommonValue(Examples)));
            }
            Attribute BestAttribute = GetBestAttribute(Examples, Attribute, bestat);
            int LocationBA = Attributes.IndexOf(BestAttribute);
            TreeNode Root = new TreeNode(BestAttribute);
            Attribute.Remove(BestAttribute);
            List<Attribute> tmp_Attribute = Attribute;
            for (int i = 0; i < BestAttribute.Value.Count; i++)
            {
                List<List<double>> Examplesvi = new List<List<double>>();
                for (int j = 0; j < Examples.Count; j++)
                {
                    if (Examples[j][LocationBA] == BestAttribute.Value[i])
                        Examplesvi.Add(Examples[j]);
                }
                if (Examplesvi.Count == 0)
                {
                    Solution += "\n Các thuộc tính rỗng => Trả về nút gốc có giá trị phổ biến nhất ";
                    //return new TreeNode(new Attribute(GetMostCommonValue(Examples)));
                    Root.AddNode(new TreeNode(new Attribute(GetMostCommonValue(Examples))));
                }
                else
                {
                    Solution += "\n";
                    //Attribute.Remove(BestAttribute);
                    Root.AddNode(ID3(Examplesvi, Attribute, BestAttribute.Value[i].ToString())); //diem
                }
                Attribute = tmp_Attribute;
            }
            return Root;
        }

        // tính entroypy

        private double GetEntropy(int[] x )
        {
            int check = 0;
            int total = 0;
            foreach (int kt in x)
            {
                if (kt > 0)
                {
                    check++;
                }
                total += kt;
            }
            if (check <= 1)
            {
                return 0;
            }
            double Entropy = 0;
            for (int i = 0; i < x.Count(); i++)
            {
                if (x[i] != 0)
                {
                    double Rate = (double) x[i]/total;
                    Entropy = Entropy - Rate*Math.Log(Rate, 2);
                }
            }
            return Entropy;
        }

        // tính Gain(bestat,A);

        private double Gain(List<List<double>> Examples, Attribute A, string bestat, List<double> dsKQua)
        {
            double result;
            int[,] CountLanCuaKQ = new int[A.Value.Count,dsKQua.Count];
            int[] Count_A = new int[dsKQua.Count];
            int Col = Attributes.IndexOf(A);
            for (int i = 0; i < A.Value.Count; i++)
            {
                for (int j = 0; j < dsKQua.Count; j++)
                {
                    CountLanCuaKQ[i, j] = 0;
                }
            }
            for (int j = 0; j < dsKQua.Count; j++)
            {
                Count_A[j] = 0;
            }
            for (int i = 0; i < Examples.Count; i++)
            {
                int j = A.Value.IndexOf(Examples[i][Col]); //vi tri value trong child
                for (int k = 0; k < dsKQua.Count; k++)
                {
                    if (Examples[i][Examples[0].Count - 1] == dsKQua[k])
                    {
                        Count_A[k]++;
                        if (j >= 0)
                        {
                            CountLanCuaKQ[j,k]++;
                        }
                    }
                }
            }
            result = GetEntropy(Count_A);
            for (int i = 0; i < A.Value.Count; i++)
            {
                double Rate = 0;
                for (int j = 0; j < dsKQua.Count; j++)
                {
                    Rate += CountLanCuaKQ[i, j];
                }
                double RateValue = (double)Rate / Examples.Count;
                int[] dsTUNGValue =new int[dsKQua.Count];
                for (int j = 0; j < dsKQua.Count; j++)
                {
                    dsTUNGValue[j] = CountLanCuaKQ[i, j];
                }
                double ccc = GetEntropy(dsTUNGValue);
                result = result - RateValue * GetEntropy(dsTUNGValue);
            }

            Solution = Solution + "\n * Gain(" + bestat + "," + A.Name + ") = " + result.ToString();
            return result;
        }

        // lấy thuật tính có Gain cao nhất

        private Attribute GetBestAttribute(List<List<double>> Examples, List<Attribute> Attributes, string bestat)
        {
            double MaxGain = Gain(Examples, Attributes[0], bestat, dsKQ);
            int Max = 0;
            for (int i = 1; i < Attributes.Count; i++)
            {
                double GainCurrent = Gain(Examples, Attributes[i], bestat, dsKQ);
                if (MaxGain < GainCurrent)
                {
                    MaxGain = GainCurrent;
                    Max = i;
                }
            }
            Solution = Solution + "\n\t=> Ta chọn đặc tính tốt nhất là : " + Attributes[Max].Name;
            return Attributes[Max];
        }

        // lấy giá trị phổ biến nhất của tập đích

        private string GetMostCommonValue(List<List<double>> Examples)
        {
            int max = 0;
            string diemPBN = "";
            for (int i = 0; i < dsKQ.Count; i++)
            {
                int dem = 0;
                for (int j = 0; j < Examples.Count; j++)
                {
                    if (Examples[j][Examples[0].Count - 1] == dsKQ[i])
                        dem++;
                }
                if (dem > max)
                {
                    max = dem;
                    diemPBN = dsKQ[i].ToString();
                }
            }
            Solution = Solution + " là " + diemPBN;
            return diemPBN;
        }

        // kiểm tra xem tất cả tập có phải chỉ còn 1 điểm kết quả cuối cùng duy nhất không

        private bool CheckAllLaKQDuyNhat(List<List<double>> Examples)
        {
            int check = 0;
            for (int i = 0; i < dsKQ.Count; i++)
            {
                for (int j = 0; j < Examples.Count; j++)
                {
                    if (Examples[j][Examples[0].Count - 1] == dsKQ[i])
                    {
                        check++;
                        diemKT = dsKQ[i].ToString();
                        break;
                    }
                }
            }
            if (check > 1)
            {
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
        public List<double> SearchTree(TreeNode tree, DataTable iDataSource, List<double> lst_result, bool kt, int dosau)
        {
            if (dosau == 1)
            {
                lst_result.Add(Convert.ToDouble(tree.Attributes.Label));
            }
            else
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
                                lst_result = SearchTree(tree.Childs[i], iDataSource, lst_result, true,dosau);
                                check++;
                            }
                        }
                        if (check == 0)
                        {
                            foreach (TreeNode trnode in tree.Childs)
                            {
                                lst_result = SearchTree(trnode, iDataSource, lst_result, false,dosau);
                            }
                        }

                    }
                    else
                    {
                        foreach (TreeNode trnode in tree.Childs)
                        {
                            lst_result = SearchTree(trnode, iDataSource, lst_result, false,dosau);
                        }
                    }
                }
            }
            return lst_result;
        }
    }
}
