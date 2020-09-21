create function [dbo].[fnGetCountriesStatistic]()
returns table
as
return
(
	select top 120
		c.Name as Country,
		vs.Confirmed,
		vs.Deaths,
		vs.Recovered,
		vs.Confirmed - vs1.Confirmed as NewConfirmed,
		vs.Deaths - vs1.Deaths as NewDeaths,
		vs.Recovered - vs1.Recovered as NewRecovered,
		round(cast(vs.Deaths as float) * 100/(vs.Recovered + vs.Deaths), 4) as DeathRate
	from Countries c
	join VirusStatistics vs on c.Id = vs.CountryId
	join (select * from VirusStatistics where ReportDate = dateadd(day, -2, cast(getutcdate() as date))) vs1 on vs.CountryId = vs1.CountryId
	where vs.ReportDate = dateadd(day, -1, cast(getutcdate() as date))
		and vs.Recovered + vs.Deaths > 0
	order by vs.Confirmed desc
)