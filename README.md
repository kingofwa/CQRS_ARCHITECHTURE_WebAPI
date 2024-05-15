Setup :

b1: Vào File: ...\WebApi_CQRS\Persistence\DependencyInjection.cs  để chọn cơ sở dữ liệu mong muốn SqlServer or postgresSql 

b2:Tạo database name: DB_Name
---Nếu sử dụng PostGresSQL : "DefaultConnectionNpgsql": "Host=host; Database=DB_Name; Username=postgres; Password=Hoilamchi123"
---Nếu sử dụng SQL Server : "DefaultConnectionSql": "Server=server;Database=DB_Name;uid=sa;pwd=123456;MultipleActiveResultSets=true;Encrypt=False;",

b3:Thực thi câu lệnh để seedata và tạo table và quan hệ
----b3.1 mở cửa sổ manager package console , switch đến layer Persistence
----b3.2 thực thi câu lệnh: update-database

b4: Run project 

Web API
==============Account Credentials==============
Admin:
Username: admin
Password: admin123
Role: Admin (Full access)

Office:
Username: office
Password: office123
Role: Office (Limited access)

Client:
Username: client
Password: client123
Role: Client (Limited access)

==============Roles and Permissions==============
Admin:
Full access to all features and resources.
Office:
Access to view details and lists.
Client:
Access to view details and lists.
