CREATE PROCEDURE [dbo].[GetUserWebCamImages]
AS
BEGIN
    SELECT anu.Id as UserId, wci.Id as WebCamImageId, anu.UserName, anu.Email, wci.ImageName, wci.CaptureTime 
    FROM WebCamImages wci
    INNER JOIN AspNetUsers anu ON wci.UserId = anu.Id
    ORDER BY wci.Id ASC
END
GO
