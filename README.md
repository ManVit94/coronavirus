Gets statistic from https://github.com/CSSEGISandData/COVID-19/tree/master/csse_covid_19_data/csse_covid_19_daily_reports
Saves statistic to CoronaVirusDb

Function in Database:
    Table-valued Functions:
        fnGetStatisticByCountry - returns last 180 days statistic for selected country
        Usage: select * from fnGetStatisticByCountry('Ukraine')

        fnGetTotalStatistic - returns total yesterday's statistic (sum of values)
        Usage: select * from fnGetTotalStatistic()

        fnGetCountriesStatistic - returns yesterday's statistic for top 120 countries ordered by confirmed cases descending
        Usage: select * from fnGetCountriesStatistic()

Stored procedures in Database:
    spGetTotalStatistic {n} - returns total statistic (sum of values) for last {n} days (but not more than 30, if {n} is not provided or > 30 returns 1 day)
    Usage: exec spGetTotalStatistic 5
