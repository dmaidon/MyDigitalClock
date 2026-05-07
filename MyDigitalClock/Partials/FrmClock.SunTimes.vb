' Last Edit: May 07, 2026 12:52 | Synopsis: Restored WinForms sunrise and sunset fetching and caching behavior.
Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports System.Text.Json

Partial Class FrmClock

    Private sunriseTime As DateTime = DateTime.MinValue
    Private sunsetTime As DateTime = DateTime.MinValue
    Private lastSunFetchDate As Date = Date.MinValue

    Private Sub LoadOrFetchSunTimes()
        Try
            If File.Exists(SunTimesFile) Then
                Dim jsonString As String = File.ReadAllText(SunTimesFile)
                Dim sunData As SunTimesData = JsonSerializer.Deserialize(Of SunTimesData)(jsonString, JsonOptions)
                Dim today As Date = Date.Today

                If sunData IsNot Nothing AndAlso sunData.FetchDate = today.ToString("yyyy-MM-dd") Then
                    If DateTime.TryParseExact(sunData.SunriseLocal, "yyyy-MM-dd HH:mm:ss",
                                             CultureInfo.InvariantCulture, DateTimeStyles.None, sunriseTime) AndAlso
                       DateTime.TryParseExact(sunData.SunsetLocal, "yyyy-MM-dd HH:mm:ss",
                                             CultureInfo.InvariantCulture, DateTimeStyles.None, sunsetTime) Then
                        lastSunFetchDate = today
                        Return
                    End If
                End If
            End If
        Catch ex As Exception
        End Try

        Task.Run(AddressOf FetchSunriseSunsetAsync)
    End Sub

    Private Async Function FetchSunriseSunsetAsync() As Task
        Try
            Dim today As Date = Date.Today
            If lastSunFetchDate = today Then
                Return
            End If

            Dim apiUrl As String = $"https://api.sunrise-sunset.org/json?lat={Latitude}&lng={Longitude}&formatted=0"
            Dim response As HttpResponseMessage = Await HttpClient.GetAsync(apiUrl)
            response.EnsureSuccessStatusCode()

            Dim jsonString As String = Await response.Content.ReadAsStringAsync()

            Using document As JsonDocument = JsonDocument.Parse(jsonString)
                Dim root = document.RootElement
                Dim results = root.GetProperty("results")

                Dim sunriseStr As String = results.GetProperty("sunrise").GetString()
                Dim sunsetStr As String = results.GetProperty("sunset").GetString()

                Dim sunriseUtc As DateTime
                Dim sunsetUtc As DateTime

                If DateTime.TryParse(sunriseStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, sunriseUtc) AndAlso
                   DateTime.TryParse(sunsetStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, sunsetUtc) Then

                    sunriseTime = sunriseUtc.ToLocalTime()
                    sunsetTime = sunsetUtc.ToLocalTime()
                    lastSunFetchDate = today

                    Dim sunData As New SunTimesData With {
                        .FetchDate = today.ToString("yyyy-MM-dd"),
                        .SunriseLocal = sunriseTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        .SunsetLocal = sunsetTime.ToString("yyyy-MM-dd HH:mm:ss")
                    }

                    Dim sunDataJson As String = JsonSerializer.Serialize(sunData, JsonOptions)
                    File.WriteAllText(SunTimesFile, sunDataJson)

                    Dim debugPath As String = Path.Combine(LogDir, "sunrise_sunset_log.txt")
                    Dim debugText As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Sunrise: {sunriseTime:HH:mm}, Sunset: {sunsetTime:HH:mm}{Environment.NewLine}"
                    File.AppendAllText(debugPath, debugText)
                End If
            End Using
        Catch ex As Exception
        End Try
    End Function

End Class
