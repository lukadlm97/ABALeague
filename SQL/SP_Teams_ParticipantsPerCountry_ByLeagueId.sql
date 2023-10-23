CREATE PROCEDURE Teams_ParticipantsPerCountry_ByLeagueId
    @LeagueId INT
AS
BEGIN
    SELECT c.[Name], COUNT(DISTINCT t.Id) AS Participants
    FROM RosterItems ri
    JOIN Teams t ON ri.TeamId = t.Id
    JOIN Countries c ON t.CountryId = c.Id
    WHERE LeagueId = @LeagueId
    GROUP BY c.[Name]
END
