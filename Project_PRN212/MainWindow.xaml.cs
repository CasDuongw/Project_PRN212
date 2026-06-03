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
        ParkingManagementDbContext _context = new ParkingManagementDbContext();
        public MainWindow()
        {
            InitializeComponent();

            // Load dữ liệu bảng Loại xe (VehicleTypes) lên DataGrid
            LoadData();
        }

        private void LoadData()
        {
            // Lấy danh sách loại xe từ DB và gán vào DataGrid
            var vehicleTypes = _context.VehicleTypes.ToList();
            dgVehicleTypes.ItemsSource = vehicleTypes;
        }
    }
}