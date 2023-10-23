CREATE PROCEDURE Players_ParticipantsPerCountry_ByLeagueId
    @LeagueId INT
AS
BEGIN
    SELECT c.[Name], COUNT(p.Id) AS Participants
    FROM RosterItems ri
    JOIN Players p ON ri.PlayerId = p.Id
    JOIN Countries c ON p.CountryId = c.Id
    WHERE LeagueId = @LeagueId
    GROUP BY c.[Name]
END
