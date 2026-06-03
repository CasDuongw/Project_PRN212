<img width="730" height="806" alt="image" src="https://github.com/user-attachments/assets/14731c14-3600-4569-a8d0-e596b640e3f5" />

# 🚗 Parking Management System (Database Schema & Architecture)

Chào mừng bạn đến với tài liệu kỹ thuật về kiến trúc dữ liệu của dự án **Hệ thống quản lý tòa nhà gửi xe**. Dưới đây là mô tả chi tiết về chức năng, mối quan hệ giữa các bảng trong cơ sở dữ liệu `ParkingManagementDB` và luồng nghiệp vụ cốt lõi phục vụ cho việc phát triển ứng dụng C# (WPF/WinForms).

---

## 🛠️ Sơ Đồ Thiết Kế Cơ Sở Dữ Liệu (Database Schema)

Hệ thống cơ sở dữ liệu được chia làm **3 nhóm bảng chính** phối hợp chặt chẽ với nhau nhằm đáp ứng đầy đủ các yêu cầu nghiệp vụ như: phân tầng theo loại xe, đặt chỗ trước và quản lý doanh thu.

### I. Nhóm Bảng Hạ Tầng Bãi Xe (Vị Trí Đỗ)
Nhóm này chịu trách nhiệm quản lý mô hình không gian vật lý của tòa nhà, giải quyết bài toán "Phân tầng theo loại xe" và "Theo dõi trạng thái ô đỗ theo thời gian thực".

#### 1. Bảng `Zones` (Tầng / Khu vực)
* **Chức năng:** Quản lý cấu trúc phân khu của tòa nhà (Ví dụ: Tầng 1, Tầng 2, Khu A, Khu B...). Nhằm đáp ứng yêu cầu phân tầng theo loại xe, mỗi `Zone` sẽ được chỉ định cố định cho một loại phương tiện cụ thể.
* **Mối quan hệ:** * `VehicleTypes (1) -> Zones (N)`: Một loại xe (ví dụ: Ô tô) có thể được phân phối đỗ ở nhiều tầng/khu vực khác nhau. Khóa ngoại `VehicleTypeID` nằm tại bảng `Zones`.

#### 2. Bảng `ParkingSlots` (Vị trí đỗ xe cụ thể)
* **Chức năng:** Lưu thông tin chi tiết cấu thành từng ô đỗ xe vật lý (Ví dụ: `T1-A01` - Tầng 1 Ô A01) và cập nhật trạng thái thực tế của ô đỗ qua thuộc tính `Status`:
    * `0`: Trống (Available)
    * `1`: Đang đỗ (Occupied)
    * `2`: Đã đặt trước (Reserved)
    * `3`: Bảo trì/Tạm khóa (Maintenance)
* **Mối quan hệ:**
    * `Zones (1) -> ParkingSlots (N)`: Một khu vực hoặc một tầng (`Zone`) sẽ bao gồm nhiều ô đỗ xe đơn lẻ. Khóa ngoại là `ZoneID`.

---

### II. Nhóm Quản Lý Người Dùng & Phương Tiện
Nhóm này định nghĩa và phân rã quyền hạn xử lý của từng tác nhân hệ thống (Actor), đồng thời quản lý thông tin các phương tiện đã đăng ký cố định.

#### 1. Bảng `Roles` (Chức vụ / Quyền hạn)
* **Chức năng:** Lưu trữ danh mục phân quyền trong hệ thống bao gồm: `Admin`, `Manager`, `Staff`, và `Driver`.
* **Mối quan hệ:**
    * `Roles (1) -> Users (N)`: Một nhóm quyền hạn/chức vụ sẽ áp dụng cho nhiều người dùng.

#### 2. Bảng `Users` (Người dùng)
* **Chức năng:** Lưu thông tin tài khoản, mật khẩu đã mã hóa, thông tin cá nhân của tất cả các Actor dùng để phục vụ quá trình Đăng nhập (Login) và phân quyền màn hình tương ứng.
* **Mối quan hệ:** Nhận `RoleID` làm khóa ngoại liên kết từ bảng `Roles`.

#### 3. Bảng `VehicleTypes` (Danh mục loại xe)
* **Chức năng:** Định nghĩa phân loại các phương tiện được phép sử dụng dịch vụ (Xe máy, Ô tô 4 chỗ, Ô tô 7 chỗ...). 
> 💡 **Lưu ý:** Đây là bảng "xương sống" của hệ thống vì toàn bộ cấu hình giá tiền, sơ đồ tầng và thuật toán tự động phân bổ slot đều bám theo loại xe này.

#### 4. Bảng `Vehicles` (Xe của khách hàng)
* **Chức năng:** Lưu thông tin biển số xe của những khách hàng thân thiết (`Driver`) đã đăng ký tài khoản cố định, phục vụ đắc lực cho tính năng "Đặt chỗ trước".
* **Mối quan hệ:**
    * `Users (1) -> Vehicles (N)`: Một khách hàng (`Driver`) có quyền sở hữu và đăng ký nhiều xe khác nhau. Khóa ngoại là `OwnerID`.
    * `VehicleTypes (1) -> Vehicles (N)`: Một loại xe áp dụng cho nhiều xe cụ thể của nhiều khách hàng khác nhau. Khóa ngoại là `VehicleTypeID`.

---

### III. Nhóm Nghiệp Vụ Vận Hành & Tính Tiền (Cốt Lõi)
Đóng vai trò trung tâm xử lý toàn bộ vòng đời của một lượt xe ra/vào và quản lý cấu hình doanh thu bãi xe.

#### 1. Bảng `PriceConfigs` (Cấu hình giá tiền)
* **Chức năng:** Định nghĩa biểu phí gửi xe theo block giờ hoặc ngày cho từng loại xe cụ thể. Việc tách riêng bảng này giúp người quản lý (`Manager`) linh hoạt thay đổi chính sách giá mà không cần can thiệp sửa code hệ thống.
* **Mối quan hệ:**
    * `VehicleTypes (1) -> PriceConfigs (N)`: Một loại xe có thể có lịch sử điều chỉnh cấu hình giá theo nhiều mốc thời gian áp dụng (`ApplyDate`). Khóa ngoại là `VehicleTypeID`.

#### 2. Bảng `ParkingSessions` (Lượt gửi xe)
* **Chức năng:** Bảng nghiệp vụ cốt lõi nhất dự án. Ghi lại toàn bộ nhật ký lịch sử từ lúc xe quét thẻ/mã đi vào bãi cho đến khi tính tiền và đi ra.
* **Mối quan hệ phối hợp:**
    * `ParkingSlots (1) -> ParkingSessions (N)`: Một ô đỗ xe phục vụ nhiều lượt gửi xe luân phiên theo thời gian. Khóa ngoại là `SlotID`.
    * `Vehicles (1) -> ParkingSessions (N)`: Một xe đã đăng ký có thể ra vào bãi nhiều lần. Khóa ngoại `VehicleID` có thể chấp nhận `NULL` nếu là khách vãng lai (lúc này hệ thống chỉ lưu thông tin chuỗi tại `LicensePlate`).
    * `Users (1) -> ParkingSessions (N)`: Liên kết song song với bảng `Users` qua 2 khóa ngoại `StaffInID` (Nhân viên đón vào) và `StaffOutID` (Nhân viên duyệt ra) phục vụ đối soát doanh thu cuối ngày.

---

## 🔄 Bản Đồ Luồng Đi Của Dữ Liệu (Data Flow)

Để thuận tiện cho việc lập trình logic xử lý trên C# (WPF), luồng dữ liệu biến đổi của hệ thống tuân theo quy trình sau:

### 📥 1. Quy trình Xe VÀO bãi (Check-In)
1. Nhân viên (`Staff`) nhập/quét Biển số xe (`LicensePlate`).
2. Hệ thống thực hiện tìm kiếm một `ParkingSlot` trống (`Status = 0`) thuộc `Zone` phù hợp với loại phương tiện đó.
3. Hệ thống tự động chuyển trạng thái `Status` của `ParkingSlot` được chọn sang `1` (Đang đỗ).
4. Khởi tạo một dòng bản ghi mới trong bảng `ParkingSessions` lưu vết: Giờ vào (`CheckInTime`), `SlotID`, `StaffInID` và đặt `Status = 1` (Xe đang trong bãi).

### 📤 2. Quy trình Xe RA bãi (Check-Out)
1. Nhân viên (`Staff`) tìm kiếm lượt gửi tương ứng trong `ParkingSessions` dựa vào Biển số hoặc Mã thẻ xe (`CardCode`).
2. Hệ thống lấy thời gian hiện tại trừ đi `CheckInTime`, đối chiếu với bảng `PriceConfigs` của loại xe đó để tự động tính toán tổng số tiền phải trả (`TotalFee`).
3. Cập nhật các thông tin còn thiếu vào bản ghi gồm: `CheckOutTime`, `TotalFee`, `StaffOutID` và chuyển trạng thái lượt gửi sang `2` (Đã thanh toán & hoàn thành).
4. Giải phóng `ParkingSlot` tương ứng, chuyển `Status` quay về `0` (Trống) để sẵn sàng đón xe tiếp theo.

---

## 📈 Sơ Đồ Mối Quan Hệ Tổng Quan (Entity Relationship Summary)

| Bảng Gốc (1) | Bảng Liên Kết (N) | Khóa Ngoại (Foreign Key) | Ý Nghĩa Nghiệp Vụ |
| :--- | :--- | :--- | :--- |
| `Roles` | `Users` | `RoleID` | Phân quyền tài khoản (Admin, Staff...) |
| `VehicleTypes` | `Zones` | `VehicleTypeID` | Thiết lập phân tầng theo loại xe |
| `Zones` | `ParkingSlots` | `ZoneID` | Xác định vị trí ô đỗ thuộc tầng nào |
| `VehicleTypes` | `Vehicles` | `VehicleTypeID` | Xác định chủng loại của phương tiện |
| `Users` | `Vehicles` | `OwnerID` | Chủ sở hữu của phương tiện (Driver) |
| `VehicleTypes` | `PriceConfigs`| `VehicleTypeID` | Cấu hình biểu phí cho từng loại xe |
| `ParkingSlots` | `ParkingSessions`| `SlotID` | Lưu vết vị trí đỗ của lượt gửi |
| `Vehicles` | `ParkingSessions`| `VehicleID` | Định danh xe đã đăng ký gửi lượt |
| `Users` | `ParkingSessions`| `StaffInID` / `StaffOutID` | Đối soát nhân viên trực ca vào/ra |
