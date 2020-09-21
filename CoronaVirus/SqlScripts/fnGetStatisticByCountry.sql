create function [dbo].[fnGetStatisticByCountry](@country varchar(50))
returns table
as
return
(
	select top 180
		vs.ReportDate, 
		vs.Confirmed,
		vs.Deaths,
		vs.Recovered,
		vs.Confirmed - lag(vs.Confirmed) over (order by vs.ReportDate) as ConfirmedPerDay,
		vs.Deaths - lag(vs.Deaths) over (order by vs.ReportDate) as DeathsPerDay,
		vs.Recovered - lag(vs.Recovered) over (order by vs.ReportDate) as RecoveredPerDay
	from Countries c
	join VirusStatistics vs on c.Id = vs.CountryId
	where c.Name = @country
	order by vs.ReportDate desc
)