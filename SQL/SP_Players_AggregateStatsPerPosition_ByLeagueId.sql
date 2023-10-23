ALTER PROCEDURE Players_AggregateStatsPerPosition_ByLeagueId
    @LeagueId INT,
    @PositionId INT
AS
BEGIN
        SELECT p.Name, p.Height, p.DateOfBirth, c.Name, ps.Name, t1.Name AS 'Team',
        COUNT(*) AS 'Game Played',
        SUM(bs.Points) AS 'Points',
        SUM(bs.[ShotMade1Pt]) AS '1Pt Made',
        SUM(bs.[ShotAttempted1Pt]) AS '1Pt Attempted',
        SUM(bs.[ShotMade2Pt]) AS '2Pt Made',
        SUM(bs.[ShotAttempted2Pt]) AS '2Pt Attempted',
        SUM(bs.[ShotMade3Pt]) AS '3Pt Made',
        SUM(bs.[ShotAttempted3Pt]) AS '3Pt Attempted',
        SUM(bs.[DefensiveRebounds]) AS 'Defensive Rebounds',
        SUM(bs.[OffensiveRebounds]) AS 'Offensive Rebounds',
        SUM(bs.[TotalRebounds]) AS 'Rebounds',
        SUM(bs.[Assists]) AS 'Assists',
        SUM(bs.[Steals]) AS 'Steals',
        SUM(bs.[Turnover]) AS 'Turnover',
        SUM(bs.[InFavoureOfBlock]) AS 'In Favoure Block',
        SUM(bs.[AgainstBlock]) AS 'Against Block',
        SUM(bs.[CommittedFoul]) AS 'Committed Foul',
        SUM(bs.[ReceivedFoul]) AS 'Draw Foul',
        SUM(bs.[RankValue]) AS 'Index',
        SUM(bs.PlusMinus) AS '+/-'
        FROM Players p
        JOIN RosterItems ri ON p.Id = ri.PlayerId
        LEFT JOIN Countries c ON p.CountryId = c.Id
        JOIN BoxScores bs ON bs.RosterItemId = ri.Id
        LEFT JOIN Positions ps ON p.PositionId = ps.Id
        LEFT JOIN Teams t1 ON ri.TeamId = t1.Id
        WHERE ri.TeamId IN (SELECT t2.Id
                           FROM SeasonResources sr
                           LEFT JOIN Teams t2 ON sr.TeamId = t2.Id
                           WHERE sr.LeagueId = @LeagueId)
          AND p.PositionId = @PositionId
          AND ri.LeagueId = @LeagueId
        GROUP BY p.Name, p.Height, p.DateOfBirth, c.Name, ps.Name, t1.Name
        ORDER BY 'Game Played' DESC
END
