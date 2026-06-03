using Project_PRN212.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_PRN212
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Khởi tạo Database Context (Lưu ý: Tên DB Context có thể khác tùy vào tên Database bạn đặt)
        //ParkingManagementDbContext _context = new ParkingManagementDbContext();
        public MainWindow()
        {
            InitializeComponent();

            // Căn giữa màn hình khi chạy
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        // Sự kiện khi bấm nút "Quản Lý Loại Xe"
        private void btnManageVehicles_Click(object sender, RoutedEventArgs e)
        {
            // 1. Khởi tạo form con
            frmVehicleType frm = new frmVehicleType();

            // 2. Tùy chọn: Chỉnh form con căn giữa màn hình cha
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // 3. Mở form con lên (Bắt buộc phải tắt form này mới quay lại Menu được)
            frm.ShowDialog();
        }

        // Sự kiện khi bấm nút "Đăng xuất"
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã đăng xuất khỏi hệ thống!");
            this.Close(); // Đóng MainWindow
        }

    }
}