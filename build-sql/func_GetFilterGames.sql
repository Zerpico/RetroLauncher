-- ============================================= 
-- Author: zerpico 
-- Create date: 27.09.2019 
-- Description: функция возвращает список игр по указаным фильтрам 
-- @param count INT Количество строк в возвращаемой таблице 
-- @param skip INT Количество строк которые следует пропустить 
-- @param name NVARCHAR(200) Имя для поиска 
-- @param genre NVARCHAR(200) Название жанра для поиска 
-- @param platform INT Название жанра для поиска 
-- @param orderbyname INT Включить сортировку по имени 
-- @param orderbyplatform INT Включить сортировку по платформе 
-- @param orderbyrating INT Включить сортировку по рейтингу 
-- @param orderbydownload INT Включить сортировку по скачиваниям 
-- =============================================
CREATE FUNCTION [dbo].[GetFilterGames] (
    @count INT= 100,
    @skip INT= 0,
    @name NVARCHAR(200) = '',
    @genresplit NVARCHAR(200) = '',
    @platformsplit NVARCHAR(200) = '',
    @orderbyname INT = 0,
    @orderbyplatform INT = 1,
    @orderbyrating INT = 0,
    @orderbydownload INT = 0
) 
RETURNS @result TABLE 
(
    GameId int, Name NVARCHAR(200),
    NameSecond NVARCHAR(200),
    NameOther NVARCHAR(200),    
    YEAR INT, developer NVARCHAR(100),
    Downloads INT, Rating NUMERIC(15, 2),
    PlatformId INT, PlatformName NVARCHAR(100),
    ALIAS NVARCHAR(100),
    GenreId INT,
    GenreName NVARCHAR(100),
    LinkId INT, url NVARCHAR(1000),
    TypeUrl INT
)
AS 
BEGIN 
DECLARE @noOrder 	INT= 0 
DECLARE @genres 	TABLE (id int );
DECLARE @platforms	TABLE (id int );
DECLARE @allgenres		BIT = 1,
		@allplatforms	BIT = 1;
 -- нач. значения
 IF (@orderbyname = 0)
    AND (@orderbyplatform = 0)
    AND (@orderbyrating = 0)
    AND (@orderbydownload = 0)
 SET @noOrder = 1; 
 
 -- сплитим
 INSERT INTO @genres
 SELECT cast(value as int) FROM STRING_SPLIT(@genresplit, ',') where value > 0
 
 INSERT INTO @platforms
 SELECT cast(value as int) FROM STRING_SPLIT(@platformsplit, ',') where value > 0
 
 IF (SELECT COUNT(*) FROM @genres) > 0
 SET @allgenres = 0;
 
 IF (SELECT COUNT(*) FROM @platforms) > 0
 SET @allplatforms = 0;
 
-- результирующая таблица
DECLARE @result_noorder TABLE 
(
  GameId int, Name NVARCHAR(200),
  NameSecond NVARCHAR(200),
  NameOther NVARCHAR(200),  
  YEAR INT, developer NVARCHAR(100),
  Downloads INT, Rating NUMERIC(15, 2),
  PlatformId INT, PlatformName NVARCHAR(100),
  ALIAS NVARCHAR(100),
  GenreId INT,
  GenreName NVARCHAR(100),
  LinkId INT, 
  url NVARCHAR(1000),
  TypeUrl INT
); 
-- чё-то ищем, и вставляем
INSERT INTO @result_noorder (gameid, name, NameSecond, NameOther, YEAR, developer, PlatformId, PlatformName, ALIAS, GenreId, GenreName)
SELECT gb.game_id AS GameId,
       gb.game_name AS Name,
       gb.name_second AS NameSecond,
       gb.name_other AS NameOther,       
       gb.year,
       gb.developer,
       pl.platform_id AS PlatformId,
       pl.platform_name AS PlatformName,
       pl.alias,
       gnr.genre_id AS GenreId,
       gnr.genre_name AS GenreName
FROM gb_games gb
JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
JOIN gb_platforms pl ON gb.platform_id = pl.platform_id
WHERE CASE
          WHEN @name IS NOT NULL AND ((gb.game_name LIKE '%'+@name+'%') OR (gb.name_second LIKE '%'+@name+'%') OR (gb.name_other LIKE '%'+@name+'%')) THEN 1
          WHEN @name IS NULL THEN 1
          ELSE 0
      END = 1
  AND CASE
          WHEN @allgenres != 1 AND gnr.genre_id in (SELECT * FROM @genres) THEN 1
          WHEN @allgenres = 1 THEN 1
          ELSE 0
      END = 1
  AND CASE
          WHEN @allplatforms != 1 AND pl.platform_id in (SELECT * FROM @platforms) THEN 1
          WHEN @allplatforms = 1 THEN 1
          ELSE 0
      END = 1 
      
-- обновляем линки только обложек
UPDATE T
SET t.LinkId = lnks.link_id,
    t.url = lnks.url,
    t.TypeUrl = lnks.type_url
FROM @result_noorder T 
OUTER APPLY
  (SELECT lnk.link_id,
          lnk.url,
          lnk.type_url
   FROM gb_links lnk
   WHERE lnk.game_id = t.gameid
   AND lnk.type_url = 2 -- нам нужна только обложка. скрины и ромы не нужны
) lnks;
-- обновляем кол-во скачиваний по игре
UPDATE T
SET t.Downloads = down.Downloads
FROM @result_noorder T 
OUTER APPLY
  (SELECT COUNT(*) AS Downloads
   FROM lg_downloads down
   WHERE down.game_id = t.gameid ) down 
   
-- обновляем рейтинг по игре
UPDATE T
SET t.Rating = rat.rating
FROM @result_noorder T 
OUTER APPLY
(-- ебанина с подсчётом среднего рейтинга
SELECT CAST(AVG(CAST(COALESCE(rat.rating, 0) AS NUMERIC(18, 8))) AS NUMERIC(18, 2)) AS rating
   FROM lg_ratings rat
   WHERE rat.game_id = t.gameid 
) rat -- выводим результат
INSERT INTO @result
SELECT *
FROM @result_noorder t
ORDER BY CASE
             WHEN @orderbyname = 1 THEN t.Name
	     ELSE ''
         END,
         CASE
             WHEN @orderbyplatform = 1 THEN t.PlatformId
             WHEN @orderbydownload = 1 THEN t.Downloads
             WHEN @orderbyrating = 1 THEN t.Rating
         END DESC 

	     
OFFSET @skip ROWS 
FETCH NEXT @count ROWS ONLY;
RETURN;
END
