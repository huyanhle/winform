using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace BangDiemSinhVien
{
    public partial class BangDiem : Form
    {
        SqlConnection conn;
        string StrCon = "server = admin\\sqlexpress ; " +
            "database = QLSV;" +
            "integrated security = SSPI ";
        public BangDiem()
        {
            InitializeComponent();
        }
        public void MoKetNoi()
        {
            conn = new SqlConnection(StrCon);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }
        public void DongKetNoi()
        {
            conn.Close();
        }
        private void LoadDiemSinhVien()
        {
            MoKetNoi(); // Mở kết nối đến cơ sở dữ liệu

            string sql = @"
                        SELECT 
                            SV.HoSV + ' ' + SV.TenSV AS [HoVaTen],
                            MH.TenMH AS [TenMonHoc],
                            LD.TenLD AS [TenLoaiDiem],
                            D.Diem AS [Diem]
                        FROM DIEM D
                        JOIN SINHVIEN SV ON D.MaSV = SV.MaSV
                        JOIN MONHOC MH ON D.MaMH = MH.MaMH
                        JOIN LOAIDIEM LD ON D.MaLD = LD.MaLD";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            DataTable tbDiem = new DataTable();
            adapter.Fill(ds);
            tbDiem = ds.Tables[0];
            dgvdiem.DataSource = tbDiem; // Gán DataTable làm nguồn dữ liệu cho DataGridView
            DongKetNoi(); // Đóng kết nối
        
        }

        private void HienThiBangDiem_Load(object sender, EventArgs e)
        {
            LoadDiemSinhVien();
        }

        private void btntim_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim(); // Lấy mã sinh viên từ TextBox
            if (!string.IsNullOrEmpty(maSV))
            {
                MoKetNoi(); // Mở kết nối

                string sql = @"
            SELECT 
                SV.HoSV + ' ' + SV.TenSV AS [Họ và Tên],
                MH.TenMH AS [Tên môn học],
                LD.TenLD AS [Tên loại điểm],
                D.Diem AS [Điểm]
            FROM DIEM D
            JOIN SINHVIEN SV ON D.MaSV = SV.MaSV
            JOIN MONHOC MH ON D.MaMH = MH.MaMH
            JOIN LOAIDIEM LD ON D.MaLD = LD.MaLD
            WHERE SV.MaSV = @MaSV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSV", maSV);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tbDiem = new DataTable();
                adapter.Fill(tbDiem);

                dgvdiem.DataSource = tbDiem; // Hiển thị dữ liệu lên DataGridView

                DongKetNoi(); // Đóng kết nối
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
