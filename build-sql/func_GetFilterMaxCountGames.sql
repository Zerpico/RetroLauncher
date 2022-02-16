-- ============================================= 
-- Author: zerpico 
-- Create date: 27.09.2019 
-- Description: функция возвращает макс. количество найденых игр по указаным фильтрам 
-- @param count INT Количество строк в возвращаемой таблице 
-- @param skip INT Количество строк которые следует пропустить 
-- @param name NVARCHAR(200) Имя для поиска 
-- @param genre NVARCHAR(200) Название жанра для поиска 
-- @param platform INT Название жанра для поиска 
-- =============================================
CREATE FUNCTION [dbo].[GetFilterMaxCountGames] (
    @name NVARCHAR(200) = '',
    @genre NVARCHAR(200) = '',
    @platform NVARCHAR(200) = ''  
) 
RETURNS int  
AS 
BEGIN 
DECLARE @genres 	TABLE (id int );
DECLARE @platforms	TABLE (id int );
DECLARE @allgenres		BIT = 1,
		@allplatforms	BIT = 1;
		
 -- сплитим
 INSERT INTO @genres
 SELECT cast(value as int) FROM STRING_SPLIT(@genre, ',') where value > 0;
 
 INSERT INTO @platforms
 SELECT cast(value as int) FROM STRING_SPLIT(@platform, ',') where value > 0;
 
 IF (SELECT COUNT(*) FROM @genres) > 0
 SET @allgenres = 0;
 
 IF (SELECT COUNT(*) FROM @platforms) > 0
 SET @allplatforms = 0;
-- Возвращаемая переменная
DECLARE @result int;
SELECT @result = COUNT(gb.game_id)
FROM gb_games gb
JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
JOIN gb_platforms pl ON gb.platform_id = pl.platform_id
WHERE CASE
          WHEN @name IS NOT NULL AND gb.game_name LIKE '%'+@name+'%' THEN 1
          WHEN @name IS NULL THEN 1
          ELSE 0
      END = 1
  AND CASE
          WHEN @allgenres != 1 AND gnr.genre_id in (SELECT id FROM @genres) THEN 1
          WHEN @allgenres = 1 THEN 1
          ELSE 0
      END = 1
  AND CASE
          WHEN @allplatforms != 1 AND pl.platform_id in (SELECT id FROM @platforms) THEN 1
          WHEN @allplatforms = 1 THEN 1
          ELSE 0
      END = 1 
      
RETURN @result;
END
