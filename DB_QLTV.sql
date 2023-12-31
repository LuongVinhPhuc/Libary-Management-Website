USE [master]
GO
/****** Object:  Database [Quanlythuvien1]    Script Date: 12/15/2022 17:29:41 ******/
CREATE DATABASE [Quanlythuvien1]

GO
USE [Quanlythuvien1]
GO
create function [dbo].[ma_tang](@idnv varchar(6), @bottom varchar(3), @sizeid int)
returns varchar(6)
as
begin
if(@idnv='')
set @idnv= @bottom + REPLICATE(0,@sizeid - LEN(@bottom))
declare @num int, @next varchar(6)
set @idnv=LTRIM(RTRIM(@idnv))
set @num = REPLACE(@idnv,@bottom,'')+1
set @sizeid =@sizeid - LEN(@bottom)
set @next = @bottom + REPLICATE(0,@sizeid - LEN(@bottom))
set @next = @bottom + RIGHT(REPLICATE(0,@sizeid) + CONVERT(varchar(Max), @num ),@sizeid)
return @next
end
GO
/****** Object:  Table [dbo].[DocGia]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocGia](
	[MaDocGia] [varchar](6) NOT NULL,
	[TenDocGia] [nvarchar](50) NULL,
	[MaSo] [varchar](50) NULL,
	[NgaySinh] [date] NULL,
	[Khoa] [int] NULL,
	[SDT] [varchar](10) NULL,
	[GioiTinh] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDocGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Khoa]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Khoa](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenKhoa] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MuonSach]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MuonSach](
	[MaPhieu] [varchar](6) NOT NULL,
	[MaDocGia] [varchar](6) NULL,
	[NgayMuon] [date] NULL,
	[NgayHenTra] [date] NULL,
	[NgayLapPhieu] [date] NULL,
	[MaNV] [varchar](6) NULL,
	[MaSach] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNV] [varchar](6) NOT NULL,
	[HoTen] [nvarchar](100) NULL,
	[NgaySinh] [date] NULL,
	[SDT] [varchar](10) NULL,
	[GioiTinh] [nvarchar](10) NULL,
	[UserName] [nvarchar](100) NULL,
	[PassWord] [nvarchar](100) NULL,
	[HinhAnh] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phong]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TheThanhVien](
	[MaThe] [varchar](6) NOT NULL,
	[MaDocGia] [varchar](6) NULL,
	[NgayHetHan] [date] NULL,
	[TinhTrang] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaThe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sach]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sach](
	[MaSach] [varchar](6) NOT NULL,
	[MaKhoSach] [varchar](6) NULL,
	[Id_TL] [int] NULL,
	[TacGia] [nvarchar](100) NULL,
	[NhaXB] [nvarchar](100) NULL,
	[TinhTrang] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TheLoai]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TheLoai](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[KhoSach]    Script Date: 12/15/2022 17:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhoSach](
	[MaSachKho] [varchar](6) NOT NULL,
	[TenSach] [nvarchar](200) NULL,
	[SoLuong] [int] NULL,
	[NgayNhapKho] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSachKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE Sach
ALTER COLUMN TenSach varchar(6)
INSERT [dbo].[DocGia] ([MaDocGia], [TenDocGia], [MaSo], [NgaySinh], [Khoa], [SDT], [GioiTinh]) VALUES (N'01', N'Son Hai', N'1221221', CAST(N'2022-12-08' AS Date), 2, N'0899765776', N'Nam')
INSERT [dbo].[DocGia] ([MaDocGia], [TenDocGia], [MaSo], [NgaySinh], [Khoa], [SDT], [GioiTinh]) VALUES (N'02', N'Minh Tam', N'1241252', CAST(N'2022-11-28' AS Date), 1, N'0866952669', N'Nữ')
INSERT [dbo].[DocGia] ([MaDocGia], [TenDocGia], [MaSo], [NgaySinh], [Khoa], [SDT], [GioiTinh]) VALUES (N'04', N'Minh Tam', N'1241252', CAST(N'2022-11-28' AS Date), 1, N'0866952669', N'Nữ')

GO
SET IDENTITY_INSERT [dbo].[Khoa] ON 

INSERT [dbo].[Khoa] ([Id], [TenKhoa]) VALUES (1, N'CNTT')
INSERT [dbo].[Khoa] ([Id], [TenKhoa]) VALUES (2, N'Kế toán')
SET IDENTITY_INSERT [dbo].[Khoa] OFF
GO
INSERT [dbo].[MuonSach] ([MaPhieu], [MaDocGia], [NgayMuon], [NgayHenTra], [NgayLapPhieu], [MaNV], [MaSach]) VALUES (N'01', N'01', CAST(N'2022-11-28' AS Date), CAST(N'2022-12-15' AS Date), CAST(N'2022-11-28' AS Date), N'01', N'01')
INSERT [dbo].[MuonSach] ([MaPhieu], [MaDocGia], [NgayMuon], [NgayHenTra], [NgayLapPhieu], [MaNV], [MaSach]) VALUES (N'02', N'02', CAST(N'2021-12-27' AS Date), CAST(N'2022-11-30' AS Date), CAST(N'2022-11-28' AS Date), N'02', N'03')
INSERT [dbo].[MuonSach] ([MaPhieu], [MaDocGia], [NgayMuon], [NgayHenTra], [NgayLapPhieu], [MaNV], [MaSach]) VALUES (N'03', N'04', CAST(N'2022-12-27' AS Date), CAST(N'2022-12-28' AS Date), CAST(N'2022-11-28' AS Date), N'02', N'03')

GO
INSERT [dbo].[NhanVien] ([MaNV], [HoTen], [NgaySinh], [SDT], [GioiTinh], [UserName], [PassWord], [HinhAnh]) VALUES (N'01', N'Phúc', CAST(N'2002-10-12' AS Date), N'086952668', N'Nam', N'user1', N'123', N'"avatar.png"')
INSERT [dbo].[NhanVien] ([MaNV], [HoTen], [NgaySinh], [SDT], [GioiTinh], [UserName], [PassWord], [HinhAnh]) VALUES (N'02', N'Bao', CAST(N'1998-04-14' AS Date), N'086952668', N'Nam', N'user2', N'123', N'avatar2.png')
INSERT [dbo].[NhanVien] ([MaNV], [HoTen], [NgaySinh], [SDT], [GioiTinh], [UserName], [PassWord], [HinhAnh]) VALUES (N'03', N'Loc', CAST(N'2022-11-29' AS Date), N'0866952668', N'Nam', N'user3', N'123', N'avatar4.png')
INSERT [dbo].[NhanVien] ([MaNV], [HoTen], [NgaySinh], [SDT], [GioiTinh], [UserName], [PassWord], [HinhAnh]) VALUES (N'04', N'Truong', CAST(N'2022-02-01' AS Date), N'0866952669', N'Nữ', N'user4', N'123', N'avatar5.png')
GO
INSERT [dbo].[TheThanhVien] ([MaThe], [MaDocGia], [NgayHetHan], [TinhTrang]) VALUES (N'01', N'01', CAST(N'2022-11-28' AS Date), 1)
INSERT [dbo].[TheThanhVien] ([MaThe], [MaDocGia], [NgayHetHan], [TinhTrang]) VALUES (N'02', N'02', CAST(N'2022-12-28' AS Date), 2)
GO
INSERT [dbo].[Sach] ([MaSach], [MaKhoSach], [Id_TL], [TacGia], [NhaXB], [TinhTrang]) VALUES (N'01', N'01', 2, N'Nguyễn Huy Liệu', N'Kim đồng', 3)
INSERT [dbo].[Sach] ([MaSach], [MaKhoSach], [Id_TL], [TacGia], [NhaXB], [TinhTrang]) VALUES (N'02', N'02', 1, N'Trần Kỳ', N'Kim đồng', 1)
INSERT [dbo].[Sach] ([MaSach], [MaKhoSach], [Id_TL], [TacGia], [NhaXB], [TinhTrang]) VALUES (N'03', N'03', 2, N'Nguyễn Ngọc Thạch', N'Tuổi trẻ', 2)
INSERT [dbo].[Sach] ([MaSach], [MaKhoSach], [Id_TL], [TacGia], [NhaXB], [TinhTrang]) VALUES (N'04', N'04', 1, N'Nguyễn Nhật Ánh', N'Kim đồng ', 4)
INSERT [dbo].[Sach] ([MaSach], [MaKhoSach], [Id_TL], [TacGia], [NhaXB], [TinhTrang]) VALUES (N'05', N'05', 1, N'Đặng Hoàng Giang', N'Tuổi trẻ ', 5)

GO
INSERT [dbo].[KhoSach] ([MaSachKho], [TenSach],[SoLuong],[NgayNhapKho] ) VALUES (N'01', N'Tôi là ai',4,CAST(N'2022-12-28' AS Date))
SET IDENTITY_INSERT [dbo].[TheLoai] ON 

INSERT [dbo].[TheLoai] ([Id], [TenLoai]) VALUES (1, N'Truyện')
INSERT [dbo].[TheLoai] ([Id], [TenLoai]) VALUES (2, N'Giáo trình')
SET IDENTITY_INSERT [dbo].[TheLoai] OFF
GO
USE [master]
GO
ALTER DATABASE [Quanlythuvien1] SET  READ_WRITE 
GO
 
 drop table Sach
create function ma_tang(@idnv varchar(6), @bottom varchar(3), @sizeid int)
returns varchar(6)
as
begin
if(@idnv='')
set @idnv= @bottom + REPLICATE(0,@sizeid - LEN(@bottom))
declare @num int, @next varchar(6)
set @idnv=LTRIM(RTRIM(@idnv))
set @num = REPLACE(@idnv,@bottom,'')+1
set @sizeid =@sizeid - LEN(@bottom)
set @next = @bottom + REPLICATE(0,@sizeid - LEN(@bottom))
set @next = @bottom + RIGHT(REPLICATE(0,@sizeid) + CONVERT(varchar(Max), @num ),@sizeid)
return @next
end
go

create trigger next_id1 on [NhanVien]
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaNV from [NhanVien] order by maNV desc)
update [NhanVien] set maNV = dbo.ma_tang(@lastid,'',2)
where MaNV=''
end
go

create trigger next_id2 on DocGia
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaDocGia from DocGia order by MaDocGia desc)
update DocGia set MaDocGia = dbo.ma_tang(@lastid,'',2)
where MaDocGia=''
end
go

create trigger next_id3 on MuonSach
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaPhieu from MuonSach order by MaPhieu desc)
update MuonSach set MaPhieu = dbo.ma_tang(@lastid,'',2)
where MaPhieu=''
end
go

create trigger next_id4 on Sach
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaSach from Sach order by MaSach desc)
update Sach set MaSach = dbo.ma_tang(@lastid,'',2)
where MaSach=''
end
go

create trigger next_id5 on TheThanhVien
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaThe from TheThanhVien order by MaThe desc)
update TheThanhVien set MaThe = dbo.ma_tang(@lastid,'',2)
where MaThe=''
end
go
create trigger next_id6 on KhoSach
for insert 
as
begin
declare @lastid varchar(2)
set @lastid = (select top 1 MaSachKho from KhoSach order by MaSachKho desc)
update KhoSach set MaSachKho = dbo.ma_tang(@lastid,'',2)
where MaSachKho=''
end
go
create trigger reset_tt on TheThanhVien
for insert,update
as 
begin 
update TheThanhVien set TinhTrang = '2'
where NgayHetHan <= GETDATE()
end
go

CREATE TRIGGER CoTheMuonSach
ON MuonSach
FOR INSERT
AS
BEGIN
DECLARE @madgtv varchar(6) 
	SELECT @madgtv = MaDocGia FROM inserted
	IF (SELECT COUNT (*) FROM TheThanhVien WHERE MaDocGia = @madgtv) < 1
    BEGIN
        RAISERROR('Độc giả chưa tạo thẻ thành viên !', 16, 1);
        ROLLBACK;
    END;
END;

CREATE TRIGGER muontoida
ON MuonSach
FOR INSERT
AS
BEGIN
	DECLARE @madg varchar (6)
	SELECT @madg = MaDocGia FROM inserted
	IF (SELECT COUNT (*) FROM MuonSach WHERE MaDocGia = @madg) > 1
	BEGIN
		RAISERROR('Thẻ đang được mươn không thể mượn tiếp !', 16, 1)
		ROLLBACK
	END
END

CREATE TRIGGER ngaymuontoida
ON MuonSach
FOR INSERT
AS
BEGIN
	DECLARE @ngaymuontoida int, @ngaymuon date , @ngayhentra date
	Select @ngaymuon =  NgayMuon from inserted
	Select @ngayhentra =  NgayHenTra from inserted
	SET @ngaymuontoida = DATEDIFF(DD,@ngaymuon, @ngayhentra) 
	IF (@ngaymuontoida >= 15)
	BEGIN
		RAISERROR('Thời hạn mượn sách tối đa la 15 ngày!', 16, 1)
		ROLLBACK
	END
END



drop trigger CoTheMuonSach
select * from muonsach