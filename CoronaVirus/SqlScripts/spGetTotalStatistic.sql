create proc [dbo].[spGetTotalStatistic](@days int = null)
as
begin
	if (@days is null or @days > 30)
		set @days = 1

	create table #TotalCoronaStatisticTemp 
		(
			TotalConfirmed int,
			TotalDeaths int,
			TotalRecovered int,
			NewConfirmed int,
			NewDeaths int,
			NewRecovered int,
			DeathRate float
		)
	declare @i int = 1
	while @i <= @days
		begin 
			insert into #TotalCoronaStatisticTemp
			select 
			sum(vs.Confirmed) as TotalConfirmed,
			sum(vs.Deaths) as TotalDeaths,
			sum(vs.Recovered) as TotalRecovered,
			sum(vs.Confirmed) - (select sum(Confirmed) from VirusStatistics where ReportDate = dateadd(day, (@i + 1) * -1, cast(getutcdate() as date))) as NewConfirmed,
			sum(vs.Deaths) - (select sum(Deaths) from VirusStatistics where ReportDate = dateadd(day, (@i + 1) * -1, cast(getutcdate() as date))) as NewDeaths,
			sum(vs.Recovered) - (select sum(Recovered) from VirusStatistics where ReportDate = dateadd(day, (@i + 1) * -1, cast(getutcdate() as date))) as NewRecovered,
			round(cast(sum(vs.Deaths) as float) * 100/(sum(vs.Deaths)+sum(vs.Recovered)), 4) as DeathRate
			from VirusStatistics vs
			where vs.ReportDate = dateadd(day, @i * -1, cast(getutcdate() as date))

			set  @i = @i + 1
		end
	select * from #TotalCoronaStatisticTemp
	drop table #TotalCoronaStatisticTemp
end