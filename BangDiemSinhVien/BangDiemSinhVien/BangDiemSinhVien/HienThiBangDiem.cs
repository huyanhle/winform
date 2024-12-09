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
    public partial class Form1 : Form
    {
        SqlConnection conn;
        string StrCon = "server = admin\\sqlexpress ; " +
            "database = QLSV;" +
            "integrated security = SSPI ";
        public Form1()
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
        public void load_gridview()
        {
            MoKetNoi();
            string sql = "SELECT MaSV , (HoSV + ' '+TenSV) AS [HoTenSV], NgaySinh, NoiSinh FROM SINHVIEN";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            DataTable tbSinhVien = new DataTable();
            adapter.Fill(ds);
            tbSinhVien = ds.Tables[0];
            dgvsinhvien.DataSource = tbSinhVien;
            DongKetNoi();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            load_gridview();
        }

        

        private void btnthem_Click(object sender, EventArgs e)
        {
            string maSV = txtmasv.Text;  // Giả sử bạn có TextBox để nhập Mã Sinh Viên
            string hoSV = txtholot.Text;  // Giả sử bạn có TextBox để nhập Họ Sinh Viên
            string tenSV = txtten.Text; // Giả sử bạn có TextBox để nhập Tên Sinh Viên
            DateTime ngaySinh = dtngaysinh.Value;  // Giả sử bạn có DateTimePicker để chọn Ngày Sinh
            string noiSinh = txtnoisinh.Text;  // Giả sử bạn có TextBox để nhập Nơi Sinh

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hoSV) || string.IsNullOrEmpty(tenSV))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên!");
                return;
            }

            try
            {
                MoKetNoi();

                string sqlInsert = "INSERT INTO SINHVIEN (MaSV, HoSV, TenSV, NgaySinh, NoiSinh) " +
                    "VALUES (@MaSV, @HoSV, @TenSV, @NgaySinh, @NoiSinh)";

                SqlCommand cmd = new SqlCommand(sqlInsert, conn);
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                cmd.Parameters.AddWithValue("@HoSV", hoSV);
                cmd.Parameters.AddWithValue("@TenSV", tenSV);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmd.Parameters.AddWithValue("@NoiSinh", noiSinh);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm sinh viên thành công!");

                load_gridview();  // Cập nhật lại DataGridView sau khi thêm sinh viên
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (dgvsinhvien.SelectedRows.Count > 0)
            {
                string maSV = dgvsinhvien.SelectedRows[0].Cells[0].Value.ToString();  // Lấy MaSV từ dòng đã chọn

                try
                {
                    MoKetNoi();

                    // Xóa sinh viên theo MaSV
                    string sqlDelete = "DELETE FROM SINHVIEN WHERE MaSV = @MaSV";
                    SqlCommand cmd = new SqlCommand(sqlDelete, conn);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa sinh viên thành công!");

                    load_gridview();  // Cập nhật lại DataGridView sau khi xóa sinh viên
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
                finally
                {
                    DongKetNoi();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!");
            }
        }

        private void dgvsinhvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             if (e.RowIndex >= 0)  // Kiểm tra nếu người dùng đã chọn một dòng
            {
                DataGridViewRow row = dgvsinhvien.Rows[e.RowIndex];
                
                // Lấy dữ liệu từ các ô trong dòng đã chọn
                string maSV = row.Cells[0].Value.ToString();  // Mã Sinh Viên
                string hoTenSV = row.Cells[1].Value.ToString();  // Họ và Tên
                int index_last_space = hoTenSV.LastIndexOf(" ");
                string ten = hoTenSV.Substring(index_last_space + 1, hoTenSV.Length - index_last_space - 1);
                string ho_lot = hoTenSV.Substring(0, index_last_space);
                string ngaySinh = row.Cells[2].Value.ToString();  // Ngày Sinh
                string noiSinh = row.Cells[3].Value.ToString();  // Nơi Sinh

                // Cập nhật các TextBox với giá trị lấy được từ dòng đã chọn
                txtmasv.Text = maSV;
                txtholot.Text = ho_lot;  // Hiển thị Họ Lót
                txtten.Text = ten;   // Hiển thị Tên
                dtngaysinh.Text = ngaySinh;
                txtnoisinh.Text = noiSinh;
            }   
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmasv.Text) || string.IsNullOrEmpty(txtholot.Text) || string.IsNullOrEmpty(txtten.Text) ||
        string.IsNullOrEmpty(dtngaysinh.Text) || string.IsNullOrEmpty(txtnoisinh.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên!");
                return;
            }

            try
            {
                // Lấy dữ liệu từ các TextBox
                string maSV = txtmasv.Text;
                string ho = txtholot.Text;
                string ten = txtten.Text;
                string ngaySinh = dtngaysinh.Text;
                string noiSinh = txtnoisinh.Text;

                // Kết nối tới cơ sở dữ liệu
                MoKetNoi();

                // Cập nhật thông tin sinh viên trong bảng SINHVIEN
                string sql = "UPDATE SINHVIEN SET HoSV = @HoSV, TenSV = @TenSV, NgaySinh = @NgaySinh, NoiSinh = @NoiSinh WHERE MaSV = @MaSV";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Thêm tham số để tránh SQL injection
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                cmd.Parameters.AddWithValue("@HoSV", ho);
                cmd.Parameters.AddWithValue("@TenSV", ten);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmd.Parameters.AddWithValue("@NoiSinh", noiSinh);

                // Thực thi câu lệnh cập nhật
                cmd.ExecuteNonQuery();

                // Đóng kết nối
                DongKetNoi();

                // Thông báo cho người dùng
                MessageBox.Show("Thông tin sinh viên đã được cập nhật!");

                // Tải lại dữ liệu trong DataGridView
                load_gridview();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, thông báo lỗi
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            try
            {
                // Mở kết nối
                MoKetNoi();

                // Câu lệnh SQL để lưu thông tin
                string sql = "INSERT INTO SINHVIEN (MaSV, HoSV, TenSV, NgaySinh, NoiSinh) VALUES (@MaSV, @HoSV, @TenSV, @NgaySinh, @NoiSinh)";

                // Tạo câu lệnh SQL command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSV", txtmasv.Text);
                cmd.Parameters.AddWithValue("@HoSV", txtholot.Text); // Họ lót
                cmd.Parameters.AddWithValue("@TenSV", txtten.Text); // Tên
                cmd.Parameters.AddWithValue("@NgaySinh", dtngaysinh.Value); // Ngày sinh
                cmd.Parameters.AddWithValue("@NoiSinh", txtnoisinh.Text); // Nơi sinh

                // Thực thi câu lệnh
                cmd.ExecuteNonQuery();

                // Đóng kết nối
                DongKetNoi();

                // Tải lại dữ liệu trong DataGridView
                load_gridview();

                MessageBox.Show("Thêm sinh viên thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}
