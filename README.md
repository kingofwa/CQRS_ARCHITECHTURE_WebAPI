## Setup

### Bước 1: Setup
1. **Chọn Cơ Sở Dữ Liệu:**
   - Mở tệp `DependencyInjection.cs` trong thư mục `Persistence`.
   - Chọn loại cơ sở dữ liệu mong muốn: SQL Server hoặc PostgreSQL.

### Bước 2: Tạo Database
2. **Tạo Database:**
   - Đặt tên database là `DB_Name`.
   - Cấu hình kết nối:
     - **Nếu sử dụng PostgreSQL:**
       ```json
       "DefaultConnectionNpgsql": "Host=host; Database=DB_Name; Username=Usernameforyou; Password=passforyou"
       ```
     - **Nếu sử dụng SQL Server:**
       ```json
       "DefaultConnectionSql": "Server=server;Database=DB_Name;uid=sa;pwd=passforyou;MultipleActiveResultSets=true;Encrypt=False;"
       ```

### Bước 3: Seed Data và Tạo Table
3. **Thực Thi Câu Lệnh:**
   - Mở cửa sổ `Package Manager Console`.
   - Chuyển đến layer `Persistence`.
   - Thực thi câu lệnh 1: `add-migration contents-migration`.
   - Thực thi câu lệnh 2: `update-database`.

### Bước 4: Run Project

## Web API
### Tài Khoản và Mật Khẩu
- **Admin:**
  - Tên Đăng Nhập: admin
  - Mật Khẩu: admin123
  - Vai Trò: Admin (Toàn quyền)

- **Office:**
  - Tên Đăng Nhập: office
  - Mật Khẩu: office123
  - Vai Trò: Office (Truy cập hạn chế)

- **Client:**
  - Tên Đăng Nhập: client
  - Mật Khẩu: client123
  - Vai Trò: Client (Truy cập hạn chế chỉ xem)

### Vai Trò và Quyền Hạn
- **Admin:**
  - Toàn quyền truy cập vào tất cả các tính năng và tài nguyên.
- **Office:**
  - Truy cập để xem chi tiết và danh sách tạo cate và product.
- **Client:**
  - Truy cập để xem chi tiết và danh sách.
