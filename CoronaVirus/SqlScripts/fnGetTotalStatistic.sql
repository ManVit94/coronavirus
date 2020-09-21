create function [dbo].[fnGetTotalStatistic]()
returns table
as
return
(
	select 
		sum(vs.Confirmed) as TotalConfirmed,
		sum(vs.Deaths) as TotalDeaths,
		sum(vs.Recovered) as TotalRecovered,
		sum(vs.Confirmed) - (select sum(Confirmed) from VirusStatistics where ReportDate = dateadd(day, -2, cast(getutcdate() as date))) as NewConfirmed,
		sum(vs.Deaths) - (select sum(Deaths) from VirusStatistics where ReportDate = dateadd(day, -2, cast(getutcdate() as date))) as NewDeaths,
		sum(vs.Recovered) - (select sum(Recovered) from VirusStatistics where ReportDate = dateadd(day, -2, cast(getutcdate() as date))) as NewRecovered,
		round(cast(sum(vs.Deaths) as float) * 100/(sum(vs.Deaths)+sum(vs.Recovered)), 4) as DeathRate
	from VirusStatistics vs
	where vs.ReportDate = dateadd(day, -1, cast(getutcdate() as date)) 
)