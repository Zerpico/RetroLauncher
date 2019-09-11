--жанры
CREATE TABLE [dbo].[gb_genres] 
(
	[genre_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[genre_name] nvarchar(50)  NOT NULL
) ON [PRIMARY]
GO
--платформы/приставки
CREATE TABLE [dbo].[gb_platforms] 
(
	[platform_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[platform_name] nvarchar(50)  NOT NULL, 
	[alias] nvarchar(10)  NOT NULL
) ON [PRIMARY]
GO
--сама база игр
CREATE TABLE [dbo].[gb_games] 
(
	[game_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[game_name] nvarchar(100)  NOT NULL, 
	[name_second] nvarchar(100) , 
	[name_other] nvarchar(100) , 
	[platform_id] int NOT NULL, 
	[genre_id] int NOT NULL, 
	[year] int, 
	[developer] nvarchar(100) , 
	[annotation] nvarchar(2000) , 
	FOREIGN KEY ([genre_id])
		REFERENCES [dbo].[gb_genres] ([genre_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([platform_id])
		REFERENCES [dbo].[gb_platforms] ([platform_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION
) ON [PRIMARY]
GO
--индекс для жанров
CREATE NONCLUSTERED INDEX [I_gb_games_genre_id]
	ON [dbo].[gb_games] ([genre_id])
	WITH (PAD_INDEX=OFF
	,STATISTICS_NORECOMPUTE=OFF
	,IGNORE_DUP_KEY=OFF
	,ALLOW_ROW_LOCKS=ON
	,ALLOW_PAGE_LOCKS=ON) ON [PRIMARY]
GO
--индекс для платформ
CREATE NONCLUSTERED INDEX [I_gb_games_platform_id]
	ON [dbo].[gb_games] ([game_id])
	WITH (PAD_INDEX=OFF
	,STATISTICS_NORECOMPUTE=OFF
	,IGNORE_DUP_KEY=OFF
	,ALLOW_ROW_LOCKS=ON
	,ALLOW_PAGE_LOCKS=ON) ON [PRIMARY]
GO
--таблица ссылко или файлов на игры
CREATE TABLE [dbo].[gb_links] 
(
	[link_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[game_id] int NOT NULL, 
	[url] nvarchar(1000)  NOT NULL, 
	[type_url] int NOT NULL, 
	FOREIGN KEY ([game_id])
		REFERENCES [dbo].[gb_games] ([game_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION
) ON [PRIMARY]
GO
--таблица пользователей
CREATE TABLE [dbo].[rg_users] 
(
	[user_id] int NOT NULL IDENTITY(0,1) PRIMARY KEY CLUSTERED, 
	[user_name] nvarchar(50) , 
	[email] nvarchar(100) , 
	[machine_sid] uniqueidentifier NOT NULL
) ON [PRIMARY]
GO
--индекс на guid юзера 
CREATE NONCLUSTERED INDEX [I_gb_users_sid]
	ON [dbo].[rg_users] ([machine_sid])
	WITH (PAD_INDEX=OFF
	,STATISTICS_NORECOMPUTE=OFF
	,IGNORE_DUP_KEY=OFF
	,ALLOW_ROW_LOCKS=ON
	,ALLOW_PAGE_LOCKS=ON) ON [PRIMARY]
GO

--журнал скачиваний
CREATE TABLE [dbo].[lg_downloads] 
(
	[download_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[game_id] int NOT NULL, 
	[user_id] int NOT NULL, 
	[dt] datetime NOT NULL, 
	FOREIGN KEY ([user_id])
		REFERENCES [dbo].[rg_users] ([user_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([game_id])
		REFERENCES [dbo].[gb_games] ([game_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION
) ON [PRIMARY]
GO
--журнал рейтингов
CREATE TABLE [dbo].[lg_ratings] 
(
	[rating_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, 
	[game_id] int NOT NULL, 
	[user_id] int NOT NULL, 
	[rating] int NOT NULL, 
	[dt] datetime NOT NULL, 
	FOREIGN KEY ([user_id])
		REFERENCES [dbo].[rg_users] ([user_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([game_id])
		REFERENCES [dbo].[gb_games] ([game_id])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	CHECK ([rating]>=(1) AND [rating]<=(5))
) ON [PRIMARY]