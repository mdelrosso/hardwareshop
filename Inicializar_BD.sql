USE [master]
GO
/****** Object:  Database [HS]    Script Date: 28/10/2017 21:04:12 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'HS')
BEGIN
CREATE DATABASE [HS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HS', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HS.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HS_log', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HS_log.ldf' , SIZE = 1088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [HS] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HS] SET ARITHABORT OFF 
GO
ALTER DATABASE [HS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HS] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HS] SET  MULTI_USER 
GO
ALTER DATABASE [HS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [HS]
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_AGREGAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_AGREGAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_AGREGAR]
	@nombre varchar(50), @descripcion varchar(255)=''''
AS
BEGIN
	SET NOCOUNT OFF;

    INSERT INTO PERMISO VALUES(@nombre, @descripcion, 0);
    
    RETURN SCOPE_IDENTITY();
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_ELIMINAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_ELIMINAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_ELIMINAR]
	@id int, @eliminado bit=1
AS
BEGIN
	SET NOCOUNT OFF;

    UPDATE PERMISO 
		SET Per_Eliminado=@eliminado
		WHERE Per_Id=@id;
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_FAMILIA_AGREGAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_FAMILIA_AGREGAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_FAMILIA_AGREGAR]
	@padreId int, @hijoId int
AS
BEGIN
	SET NOCOUNT OFF;

	IF (SELECT COUNT(0) FROM PERMISOFAMILIA 
		WHERE Pefa_PermisoPadreId=@padreId AND Pefa_PermisoHijoId=@hijoId)=0 BEGIN
		
		-- solo agregarlo si no existe ya la relacion
		INSERT INTO PERMISOFAMILIA VALUES(@padreId, @hijoId);
		
	END
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_FAMILIA_ELIMINAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_FAMILIA_ELIMINAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_FAMILIA_ELIMINAR]
	@padreId int, @hijoId int=NULL
AS
BEGIN
	SET NOCOUNT OFF;
	
	DELETE FROM PERMISOFAMILIA 
		WHERE Pefa_PermisoPadreId=@padreId
			AND (Pefa_PermisoHijoId=@hijoId OR @hijoId IS NULL);
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_FAMILIA_LISTAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_FAMILIA_LISTAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_FAMILIA_LISTAR]
	@padreId int=NULL, @hijoId int=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @padreId IS NOT NULL BEGIN
    -- Insert statements for procedure here
	SELECT per.Per_Id, per.Per_Nombre, per.Per_Descripcion, per.Per_Eliminado, 
		(SELECT COUNT(0) FROM PERMISOFAMILIA WHERE Pefa_PermisoHijoId=Per_Id) AS Cant_Hijos
		FROM PERMISO per LEFT JOIN PERMISOFAMILIA  pefa ON (per.Per_Id=pefa.Pefa_PermisoHijoId)
		WHERE (per.Per_Id=@hijoId OR @hijoId IS NULL)
			AND (pefa.Pefa_PermisoPadreId=@padreId OR @padreId IS NULL)
		ORDER BY Per_Nombre;
	END
	ELSE BEGIN
		SELECT per.Per_Id, per.Per_Nombre, per.Per_Descripcion, per.Per_Eliminado, 
		(SELECT COUNT(0) FROM PERMISOFAMILIA WHERE Pefa_PermisoHijoId=Per_Id) AS Cant_Hijos
		FROM PERMISO per LEFT JOIN PERMISOFAMILIA  pefa ON (per.Per_Id=pefa.Pefa_PermisoPadreId)
		WHERE (pefa.Pefa_PermisoPadreId=@padreId OR @padreId IS NULL)
			AND (pefa.Pefa_PermisoHijoId=@hijoid OR @hijoid IS NULL)
		ORDER BY Per_Nombre;
	END
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_LISTAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_LISTAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_LISTAR]
	@nombre varchar(50)=NULL, @descripcion varchar(255)=NULL,
	@eliminado bit=NULL, @id int=NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Per_Id, Per_Nombre, Per_Descripcion, Per_Eliminado, 
		(SELECT COUNT(0) FROM PERMISOFAMILIA WHERE Pefa_PermisoPadreId=Per_Id) AS Cant_Hijos
		FROM PERMISO 
		WHERE (Per_Id=@id OR @id IS NULL)
			AND (Per_Nombre LIKE ''%''+@nombre+''%'' OR @nombre IS NULL)
			AND (Per_Descripcion LIKE ''%''+@descripcion+''%'' OR @descripcion IS NULL)
			AND (Per_Eliminado=@eliminado OR @eliminado IS NULL)
		ORDER BY Per_Nombre;
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_PERMISO_MODIFICAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_PERMISO_MODIFICAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_PERMISO_MODIFICAR]
	@nombre varchar(50), @descripcion varchar(255)='''', @eliminado int=0, @id int
AS
BEGIN
	SET NOCOUNT OFF;

    UPDATE PERMISO 
		SET Per_Nombre=@nombre, Per_Descripcion=@descripcion, Per_Eliminado=@eliminado
		WHERE Per_Id=@id;
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_USUARIO_PERMISO_AGREGAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_USUARIO_PERMISO_AGREGAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_USUARIO_PERMISO_AGREGAR]
	@usuarioId int, @permisoId int
AS
BEGIN
	SET NOCOUNT OFF;
	IF (SELECT COUNT(0) FROM USUARIOPERMISO
		WHERE UsPe_UsuarioId=@usuarioId AND UsPe_PermisoId=@permisoId) = 0 BEGIN
		
		INSERT INTO USUARIOPERMISO VALUES(@usuarioId, @permisoId);
    END
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_USUARIO_PERMISO_ELIMINAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_USUARIO_PERMISO_ELIMINAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_USUARIO_PERMISO_ELIMINAR]
	@usuarioId int, @permisoId int
AS
BEGIN
	SET NOCOUNT OFF;
	DELETE FROM USUARIOPERMISO
		WHERE UsPe_UsuarioId=@usuarioId AND UsPe_PermisoId=@permisoId
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[HS_USUARIO_PERMISO_LISTAR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HS_USUARIO_PERMISO_LISTAR]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HS_USUARIO_PERMISO_LISTAR]
	@usuarioId int, @permisoId int=NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Insert statements for procedure here
	SELECT per.Per_Id, per.Per_Nombre, per.Per_Descripcion, per.Per_Eliminado, 
		(SELECT COUNT(0) FROM PERMISOFAMILIA WHERE Pefa_PermisoPadreId=Per_Id) AS Cant_Hijos
		FROM PERMISO per LEFT JOIN  USUARIOPERMISO uspe ON (uspe.UsPe_PermisoId=per.Per_Id)		
		WHERE UsPe_UsuarioId=@usuarioId 
			AND (UsPe_PermisoId=@permisoId OR @permisoId IS NULL)
		ORDER BY per.Per_Nombre
END
' 
END
GO
/****** Object:  Table [dbo].[BITACORA]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BITACORA]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BITACORA](
	[IDBITACORA] [bigint] IDENTITY(1,1) NOT NULL,
	[DIGITOHORIZONTAL] [bigint] NOT NULL,
	[LOG] [nvarchar](255) NOT NULL,
	[ELIMINADO] [bit] NOT NULL,
 CONSTRAINT [PK_BITACORA] PRIMARY KEY CLUSTERED 
(
	[IDBITACORA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DIGITOVERIFICADOR]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DIGITOVERIFICADOR]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DIGITOVERIFICADOR](
	[IDDIGITOVERIFICADOR] [bigint] IDENTITY(1,1) NOT NULL,
	[DIGITOVERTICAL] [bigint] NOT NULL,
	[TABLANOMBRE] [varchar](100) NOT NULL,
	[DIGITOHORIZONTAL] [bigint] NULL,
 CONSTRAINT [PK_DIGITOVERIFICADOR] PRIMARY KEY CLUSTERED 
(
	[IDDIGITOVERIFICADOR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HARDWARE]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HARDWARE]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HARDWARE](
	[Hard_Id] [int] IDENTITY(1,1) NOT NULL,
	[Hard_Descripcion] [nvarchar](500) NOT NULL,
	[Hard_Precio] [money] NOT NULL,
	[Hard_Eliminado] [bit] NULL,
	[DIGITOHORIZONTAL] [bigint] NULL,
 CONSTRAINT [PK_HARDWARE] PRIMARY KEY CLUSTERED 
(
	[Hard_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PERMISO]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PERMISO]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PERMISO](
	[Per_Id] [int] IDENTITY(1,1) NOT NULL,
	[Per_Nombre] [varchar](50) NOT NULL,
	[Per_Descripcion] [varchar](255) NULL,
	[Per_Eliminado] [bit] NULL,
 CONSTRAINT [PK_PERMISO] PRIMARY KEY CLUSTERED 
(
	[Per_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PERMISOFAMILIA]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PERMISOFAMILIA]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PERMISOFAMILIA](
	[Pefa_PermisoPadreId] [int] NOT NULL,
	[Pefa_PermisoHijoId] [int] NOT NULL,
 CONSTRAINT [PK_PERMISOFAMILIA] PRIMARY KEY CLUSTERED 
(
	[Pefa_PermisoPadreId] ASC,
	[Pefa_PermisoHijoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USUARIO]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[USUARIO](
	[Usu_Id] [int] IDENTITY(1,1) NOT NULL,
	[Usu_Nombre] [varchar](16) NOT NULL,
	[Usu_Clave] [nvarchar](512) NOT NULL,
	[Usu_Email] [varchar](50) NOT NULL,
	[Usu_Bloqueado] [bit] NOT NULL,
	[Usu_Eliminado] [bit] NULL,
	[DIGITOHORIZONTAL] [bigint] NULL,
 CONSTRAINT [PK_USUARIO] PRIMARY KEY CLUSTERED 
(
	[Usu_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USUARIOPERMISO]    Script Date: 28/10/2017 21:04:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USUARIOPERMISO]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[USUARIOPERMISO](
	[UsPe_UsuarioId] [int] NOT NULL,
	[UsPe_PermisoId] [int] NOT NULL,
 CONSTRAINT [PK_USUARIOPERMISO] PRIMARY KEY CLUSTERED 
(
	[UsPe_UsuarioId] ASC,
	[UsPe_PermisoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[BITACORA] ON 

INSERT [dbo].[BITACORA] ([IDBITACORA], [DIGITOHORIZONTAL], [LOG], [ELIMINADO]) VALUES (10067, 28313, N'28/10/2017 21:02:14|webmaster|Login con Exito|3', 0)
SET IDENTITY_INSERT [dbo].[BITACORA] OFF
SET IDENTITY_INSERT [dbo].[DIGITOVERIFICADOR] ON 

INSERT [dbo].[DIGITOVERIFICADOR] ([IDDIGITOVERIFICADOR], [DIGITOVERTICAL], [TABLANOMBRE], [DIGITOHORIZONTAL]) VALUES (1, 781, N'BITACORA', 2996)
INSERT [dbo].[DIGITOVERIFICADOR] ([IDDIGITOVERIFICADOR], [DIGITOVERTICAL], [TABLANOMBRE], [DIGITOHORIZONTAL]) VALUES (2, 156, N'USUARIO', 2540)
INSERT [dbo].[DIGITOVERIFICADOR] ([IDDIGITOVERIFICADOR], [DIGITOVERTICAL], [TABLANOMBRE], [DIGITOHORIZONTAL]) VALUES (3, 212, N'DIGITOVERIFICADOR', 11695)
INSERT [dbo].[DIGITOVERIFICADOR] ([IDDIGITOVERIFICADOR], [DIGITOVERTICAL], [TABLANOMBRE], [DIGITOHORIZONTAL]) VALUES (4, 270, N'HARDWARE', 3033)
SET IDENTITY_INSERT [dbo].[DIGITOVERIFICADOR] OFF
SET IDENTITY_INSERT [dbo].[HARDWARE] ON 

INSERT [dbo].[HARDWARE] ([Hard_Id], [Hard_Descripcion], [Hard_Precio], [Hard_Eliminado], [DIGITOHORIZONTAL]) VALUES (1, N'NOTEBOOK 15" HP 250 G6 i5-7200U 4GB HD 1TERA ESPAÑOL  ', 11453.6200, 0, 98383)
INSERT [dbo].[HARDWARE] ([Hard_Id], [Hard_Descripcion], [Hard_Precio], [Hard_Eliminado], [DIGITOHORIZONTAL]) VALUES (2, N'IMPRESORA HP M402DNE LASER MONOCROMATICA C5J91A-L  ', 6492.6900, 0, 86672)
INSERT [dbo].[HARDWARE] ([Hard_Id], [Hard_Descripcion], [Hard_Precio], [Hard_Eliminado], [DIGITOHORIZONTAL]) VALUES (3, N'DISCO RIGIDO 10TERA WESTERN DIGITAL RED NAS (WD100EFAX) SATA III', 1757.6900, 0, 137052)
INSERT [dbo].[HARDWARE] ([Hard_Id], [Hard_Descripcion], [Hard_Precio], [Hard_Eliminado], [DIGITOHORIZONTAL]) VALUES (4, N'PLACA DE VIDEO RADEON GIGABYTE RX 560 GAMING OC 2GB GDDR5 128bit PCIE', 2758.9100, 0, 159955)
INSERT [dbo].[HARDWARE] ([Hard_Id], [Hard_Descripcion], [Hard_Precio], [Hard_Eliminado], [DIGITOHORIZONTAL]) VALUES (5, N'TECLADO SENTEY TECHNUS GAMER GAMING GS-5750 ', 355.7300, 0, 65216)
SET IDENTITY_INSERT [dbo].[HARDWARE] OFF
SET IDENTITY_INSERT [dbo].[PERMISO] ON 

INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (1, N'ADMINISTRACION', N'Administracion del Sistema', 0)
INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (2, N'ADMIN_USUARIOS', N'Administracion de usuarios', 0)
INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (3, N'ADMIN_PERMISOS', N'Administracion de Permisos', 0)
INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (4, N'ADMIN_PERFILES', N'Administracion de Perfiles', 0)
INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (5, N'USUARIOFINAL', N'Funcionalidades del Sistema', 0)
INSERT [dbo].[PERMISO] ([Per_Id], [Per_Nombre], [Per_Descripcion], [Per_Eliminado]) VALUES (6, N'ADMIN_NEGOCIO', N'Administracion del negocio. Actualizador de precios', 0)
SET IDENTITY_INSERT [dbo].[PERMISO] OFF
INSERT [dbo].[PERMISOFAMILIA] ([Pefa_PermisoPadreId], [Pefa_PermisoHijoId]) VALUES (1, 2)
INSERT [dbo].[PERMISOFAMILIA] ([Pefa_PermisoPadreId], [Pefa_PermisoHijoId]) VALUES (1, 3)
INSERT [dbo].[PERMISOFAMILIA] ([Pefa_PermisoPadreId], [Pefa_PermisoHijoId]) VALUES (1, 4)
SET IDENTITY_INSERT [dbo].[USUARIO] ON 

INSERT [dbo].[USUARIO] ([Usu_Id], [Usu_Nombre], [Usu_Clave], [Usu_Email], [Usu_Bloqueado], [Usu_Eliminado], [DIGITOHORIZONTAL]) VALUES (1, N'webmaster', N'ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413', N'webmaster@gmail.com', 0, 0, 617743)
INSERT [dbo].[USUARIO] ([Usu_Id], [Usu_Nombre], [Usu_Clave], [Usu_Email], [Usu_Bloqueado], [Usu_Eliminado], [DIGITOHORIZONTAL]) VALUES (2, N'admin', N'ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413', N'admin2@gmail.com', 0, 0, 608635)
INSERT [dbo].[USUARIO] ([Usu_Id], [Usu_Nombre], [Usu_Clave], [Usu_Email], [Usu_Bloqueado], [Usu_Eliminado], [DIGITOHORIZONTAL]) VALUES (3, N'cliente', N'ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413', N'cliente@gmail.com', 0, 0, 612073)
SET IDENTITY_INSERT [dbo].[USUARIO] OFF
INSERT [dbo].[USUARIOPERMISO] ([UsPe_UsuarioId], [UsPe_PermisoId]) VALUES (1, 1)
INSERT [dbo].[USUARIOPERMISO] ([UsPe_UsuarioId], [UsPe_PermisoId]) VALUES (2, 6)
INSERT [dbo].[USUARIOPERMISO] ([UsPe_UsuarioId], [UsPe_PermisoId]) VALUES (3, 5)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PERMISOFAMILIA_PERMISO]') AND parent_object_id = OBJECT_ID(N'[dbo].[PERMISOFAMILIA]'))
ALTER TABLE [dbo].[PERMISOFAMILIA]  WITH CHECK ADD  CONSTRAINT [FK_PERMISOFAMILIA_PERMISO] FOREIGN KEY([Pefa_PermisoPadreId])
REFERENCES [dbo].[PERMISO] ([Per_Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PERMISOFAMILIA_PERMISO]') AND parent_object_id = OBJECT_ID(N'[dbo].[PERMISOFAMILIA]'))
ALTER TABLE [dbo].[PERMISOFAMILIA] CHECK CONSTRAINT [FK_PERMISOFAMILIA_PERMISO]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PERMISOFAMILIA_PERMISO1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PERMISOFAMILIA]'))
ALTER TABLE [dbo].[PERMISOFAMILIA]  WITH CHECK ADD  CONSTRAINT [FK_PERMISOFAMILIA_PERMISO1] FOREIGN KEY([Pefa_PermisoHijoId])
REFERENCES [dbo].[PERMISO] ([Per_Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PERMISOFAMILIA_PERMISO1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PERMISOFAMILIA]'))
ALTER TABLE [dbo].[PERMISOFAMILIA] CHECK CONSTRAINT [FK_PERMISOFAMILIA_PERMISO1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_USUARIOPERMISO_PERMISO]') AND parent_object_id = OBJECT_ID(N'[dbo].[USUARIOPERMISO]'))
ALTER TABLE [dbo].[USUARIOPERMISO]  WITH CHECK ADD  CONSTRAINT [FK_USUARIOPERMISO_PERMISO] FOREIGN KEY([UsPe_PermisoId])
REFERENCES [dbo].[PERMISO] ([Per_Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_USUARIOPERMISO_PERMISO]') AND parent_object_id = OBJECT_ID(N'[dbo].[USUARIOPERMISO]'))
ALTER TABLE [dbo].[USUARIOPERMISO] CHECK CONSTRAINT [FK_USUARIOPERMISO_PERMISO]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_USUARIOPERMISO_USUARIO]') AND parent_object_id = OBJECT_ID(N'[dbo].[USUARIOPERMISO]'))
ALTER TABLE [dbo].[USUARIOPERMISO]  WITH CHECK ADD  CONSTRAINT [FK_USUARIOPERMISO_USUARIO] FOREIGN KEY([UsPe_UsuarioId])
REFERENCES [dbo].[USUARIO] ([Usu_Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_USUARIOPERMISO_USUARIO]') AND parent_object_id = OBJECT_ID(N'[dbo].[USUARIOPERMISO]'))
ALTER TABLE [dbo].[USUARIOPERMISO] CHECK CONSTRAINT [FK_USUARIOPERMISO_USUARIO]
GO
USE [master]
GO
ALTER DATABASE [HS] SET  READ_WRITE 
GO
