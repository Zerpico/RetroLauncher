--функции для поиска
CREATE FUNCTION [dbo].[GetGameById] 
(
    @id INT = 2000
)
RETURNS @result TABLE  
(GameId       INT,
 Name         NVARCHAR(200),
 NameSecond   NVARCHAR(200),
 NameOther    NVARCHAR(200), 
 year         INT,
 developer    NVARCHAR(100),
 annotation    NVARCHAR(2000),
 Downloads    INT,
 Rating       NUMERIC(15, 2),
 PlatformId   INT,
 PlatformName NVARCHAR(100),
 alias        NVARCHAR(100),
 GenreId      INT,
 GenreName    NVARCHAR(100),
 LinkId       INT,
 url          NVARCHAR(1000),
 TypeUrl      INT
)
AS  
BEGIN
INSERT INTO @result
SELECT 
    gb.game_id as GameId,
    gb.game_name as Name,
    gb.name_second as NameSecond,
    gb.name_other as NameOther,
    gb.year,
    gb.developer,
    gb.annotation, 
    0,0,
    pl.platform_id as PlatformId,
    pl.platform_name as PlatformName,
    pl.alias, 
    gnr.genre_id as GenreId,
    gnr.genre_name as GenreName,
    lnk.link_id as LinkId,
    lnk.url,
    lnk.type_url as TypeUrl
FROM gb_games gb
JOIN gb_genres gnr ON gnr.genre_id = gb.genre_id
JOIN gb_platforms pl ON gb.platform_id = pl.platform_id
JOIN gb_links lnk ON lnk.game_id = gb.game_id
WHERE gb.game_id = @id;

-- обновляем кол-во скачиваний по игре
UPDATE T
SET  t.Downloads = down.Downloads
FROM @result T
OUTER APPLY
(
    SELECT COUNT(*) AS Downloads	-- кол-во скачиваний
    FROM lg_downloads down
    WHERE down.game_id = t.gameid
) down

-- обновляем рейтинг по игре
UPDATE T
SET  t.Rating = rat.rating
FROM @result T
OUTER APPLY
(
	-- подсчёт среднего рейтинга
    SELECT CAST(AVG(CAST(COALESCE(rat.rating,0) AS NUMERIC(18, 8))) AS NUMERIC(18, 2)) AS rating
    FROM lg_ratings rat
    WHERE rat.game_id = t.gameid
) rat
RETURN  
END;