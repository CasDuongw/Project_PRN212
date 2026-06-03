<img width="730" height="806" alt="image" src="https://github.com/user-attachments/assets/14731c14-3600-4569-a8d0-e596b640e3f5" />

I. Nhóm Bảng Hạ Tầng Bãi Xe (Vị Trí Đỗ)
Nhóm này quản lý mô hình không gian của tòa nhà gửi xe, giải quyết trực tiếp yêu cầu "Phân tầng theo loại xe" và "Theo dõi trạng thái ô đỗ".

1. Bảng Zones (Tầng / Khu vực)
Chức năng: Quản lý cấu trúc của tòa nhà (Ví dụ: Tầng 1, Tầng 2, Khu A...). Đề bài yêu cầu phân tầng theo loại xe, nên mỗi Zone sẽ gắn chặt với một loại phương tiện duy nhất.


Mối quan hệ: * VehicleTypes (1) -> Zones (N): Một loại xe (ví dụ: Ô tô) có thể được phân cho nhiều tầng/khu vực đỗ. Khóa ngoại VehicleTypeID nằm ở bảng Zones để giới hạn tầng này chỉ nhận loại xe đó.

2. Bảng ParkingSlots (Vị trí đỗ xe cụ thể)

Chức năng: Lưu thông tin chi tiết của từng ô đỗ xe (ví dụ ô T1-A01, T1-A02) và theo dõi trạng thái thời gian thực (Status): Trống (0), Đang đỗ (1), Đã đặt trước (2), Bảo trì (3).

Mối quan hệ:


Zones (1) -> ParkingSlots (N): Một tầng/khu vực (Zone) sẽ chứa nhiều ô đỗ xe cụ thể. Khóa ngoại ZoneID nằm ở bảng ParkingSlots.

II. Nhóm Quản Lý Người Dùng & Phương Tiện
Nhóm này phân rõ quyền hạn xử lý của từng Actor (Admin, Manager, Staff, Driver) và quản lý thông tin xe đã đăng ký trước trên hệ thống.

1. Bảng Roles (Chức vụ / Quyền hạn)

Chức năng: Lưu danh mục các nhóm quyền trong hệ thống, bao gồm: Admin, Manager, Staff, và Driver.

Mối quan hệ:


Roles (1) -> Users (N): Một chức vụ sẽ áp dụng cho nhiều người dùng khác nhau.

2. Bảng Users (Người dùng)

Chức năng: Lưu thông tin tài khoản, mật khẩu, thông tin cá nhân của tất cả các Actor để đăng nhập và phân quyền hiển thị màn hình.


Mối quan hệ: Nhận RoleID làm khóa ngoại từ bảng Roles.

3. Bảng VehicleTypes (Danh mục loại xe)

Chức năng: Định nghĩa các loại xe được phép gửi (Xe máy, Ô tô 4 chỗ, Ô tô 7 chỗ...). Đây là bảng xương sống vì cả sơ đồ tầng, giá tiền, và thuật toán phân bổ đều bám theo loại xe này.

4. Bảng Vehicles (Xe của khách hàng)

Chức năng: Lưu thông tin xe (biển số) mà các tài xế (Driver) đã đăng ký cố định trên hệ thống để phục vụ tính năng "Đặt chỗ trước".

Mối quan hệ:


Users (1) -> Vehicles (N): Một khách hàng (Driver) có thể sở hữu và đăng ký nhiều xe khác nhau. Khóa ngoại là OwnerID.


VehicleTypes (1) -> Vehicles (N): Một loại xe (ví dụ: Ô tô 4 chỗ) áp dụng cho nhiều xe cụ thể của các khách hàng khác nhau. Khóa ngoại là VehicleTypeID.

III. Nhóm Nghiệp Vụ Vận Hành & Tính Tiền (Cốt Lõi)
Đây là nơi xử lý vòng đời xe ra/vào và cấu hình doanh thu.

1. Bảng PriceConfigs (Cấu hình giá tiền)

Chức năng: Lưu thông tin giá vé theo block giờ hoặc ngày cho từng loại xe. Thiết kế này giúp Manager có thể cập nhật giá linh hoạt thay vì code cứng vào C#.

Mối quan hệ:


VehicleTypes (1) -> PriceConfigs (N): Một loại xe sẽ có một hoặc nhiều lượt thay đổi/cấu hình giá theo thời gian (ApplyDate). Khóa ngoại là VehicleTypeID.

2. Bảng ParkingSessions (Lượt gửi xe)

Chức năng: Bảng "trung tâm" và quan trọng nhất dự án. Nó ghi lại toàn bộ lịch sử từ lúc xe quét thẻ đi vào (giờ vào, biển số, nhân viên trực, slot đỗ) cho đến khi tính tiền đi ra (giờ ra, tổng tiền, nhân viên thu tiền).

Mối quan hệ (Rất nhiều kết nối):


ParkingSlots (1) -> ParkingSessions (N): Một ô đỗ xe sẽ phục vụ nhiều lượt gửi xe khác nhau theo thời gian. Khóa ngoại là SlotID.


Vehicles (1) -> ParkingSessions (N): Một xe đã đăng ký tài khoản có thể ra vào bãi nhiều lần. Khóa ngoại VehicleID có thể NULL nếu đó là khách vãng lai (chỉ cần nhập biển số trực tiếp LicensePlate).


Users (1) -> ParkingSessions (N): Kết nối với bảng Users qua 2 khóa ngoại là StaffInID (nhân viên cho xe vào) và StaffOutID (nhân viên cho xe ra) để phục vụ việc đối soát doanh thu cuối ngày cho Manager.
